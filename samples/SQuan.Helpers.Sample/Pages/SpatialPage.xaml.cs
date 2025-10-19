// SpatialPage.xaml.cs

using CsvHelper;
using SkiaSharp;
using SQLite;
using SQuan.Helpers.SQLiteSpatial;

namespace SQuan.Helpers.Sample;

#pragma warning disable CA1001 // Suppress IDisposable warning for SQLiteConnection

public partial class SpatialPage : ContentPage
{
	SQLiteSpatialConnection db = new(":memory:");
	MapToViewTransform transform = new();
	NetTopologySuite.Geometries.Geometry? selection;
	bool loaded = false;

	public class UsaStates : SpatialData;
	public class UsaCities : SpatialData;

	public SpatialPage()
	{
		InitializeComponent();
		Dispatcher.Dispatch(async () => await PostInitialize());
	}

	async Task PostInitialize()
	{
		// Load sample spatial data from CSV files into the in-memory databases
		await LoadFromCsv<UsaStates>(db, "usa_states.csv"); // all US states
		await LoadFromCsv<UsaCities>(db, "usa_cities.csv"); // some US cities

		// Experiment with creating spatial indexes.
		db.Execute("CREATE INDEX IX_UsaStates_Name ON UsaStates (Name)");
		db.Execute("CREATE INDEX IX_UsaStates_Geometry ON UsaStates (SP_S(Geometry) DESC, SP_Y(Geometry), SP_X(Geometry), SP_H2(Geometry), SP_W2(Geometry))");
		db.Execute("CREATE INDEX IX_UsaCities_Geometry ON UsaCities (SP_Y(Geometry), SP_X(Geometry))");

		// Do some test spatial queries
		double? area_50_units = db.ExecuteScalar<double?>("SELECT ST_Area('POLYGON((10 10,20 10,20 20,10 10))')");
		string? centroid_at_16_13 = db.ExecuteScalar<string?>("SELECT ST_Centroid('POLYGON((10 10,20 10,20 20,10 10))')");
		string? circle_buffer = db.ExecuteScalar<string?>("SELECT ST_Buffer('POINT(10 10)', 5)");
		double? distance_5_units = db.ExecuteScalar<double?>("SELECT ST_Distance('POINT(0 0)', 'POINT(3 4)')");
		double? area_100_units = db.ExecuteScalar<double?>("SELECT ST_Area(ST_Envelope(ST_Buffer('POINT(10 10)', 5)))");

		// Retrieve cities in order of distance, starting with those nearest to Los Angeles.
		var results = db.Query<SpatialData>("SELECT * FROM UsaCities ORDER BY ST_Distance(Geometry, 'POINT(-118.243683 34.052235)')");
		foreach (var result in results)
		{
			System.Diagnostics.Trace.WriteLine("City (spatial sort): " + result.Name);
		}

		// Search UsaCities using IX_UsaCities_Geometry spatial index.
		results = db.Query<SpatialData>("""
SELECT *
FROM   UsaCities
WHERE  SP_Y(Geometry) BETWEEN   32.534231 AND   42.009659
AND    SP_X(Geometry) BETWEEN -124.410607 AND -114.134458
""");
		foreach (var result in results)
		{
			System.Diagnostics.Trace.WriteLine("City (spatial index): " + result.Name);
		}

		// Search UsaStates forcing the use of IX_UsaStates_Geometry spatial index.
		results = db.Query<SpatialData>("""
SELECT  *
FROM    UsaStates
INDEXED BY IX_UsaStates_Geometry
WHERE   SP_Y(Geometry) BETWEEN   32.534231 - SP_H2(Geometry) AND   42.009659 + SP_H2(Geometry)
AND     SP_X(Geometry) BETWEEN -124.410607 - SP_W2(Geometry) AND -114.134458 + SP_W2(Geometry)
""");
		foreach (var result in results)
		{
			System.Diagnostics.Trace.WriteLine("State (spatial index): " + result.Name);
		}

		// Create a selection 1.0 degree (111 km) buffer around Los Angeles, CA.
		selection = new NetTopologySuite.Geometries.Point(-118.2437, 34.0522).Buffer(1.0);

		// Finalize loading.
		loaded = true;
		canvasView.InvalidateSurface();
	}

	async Task LoadFromCsv<T>(SQLiteConnection db, string fileName)
	{
		using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
		using var reader = new StreamReader(stream);
		using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
		db.RunInTransaction(() =>
		{
			db.CreateTable<T>();
			foreach (var record in csv.GetRecords<T>())
			{
				db.Insert(record);
			}
		});
	}

	void canvasView_PaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
	{
		if (!loaded)
		{
			return;
		}

		if (transform.FollowMapExtent)
		{
			transform.UpdateScale(canvasView.Width, canvasView.Height);
		}
		transform.UpdateCenter(canvasView.Width, canvasView.Height);

		SKSurface surface = e.Surface;
		SKCanvas canvas = surface.Canvas;
		canvas.Clear();
		canvas.DrawSpatialData(db.Query<SpatialData>("SELECT * FROM UsaStates"), transform);
		if (selection is not null)
		{
			canvas.DrawGeometry("", Colors.Orange, selection, transform);
		}
		canvas.DrawSpatialData(db.Query<SpatialData>("SELECT * FROM UsaCities"), transform);
	}

	void OnReset(object sender, EventArgs e)
	{
		transform.MapExtent = Rect.FromLTRB(-125, 24.3, -66.9, 49.4); // USA extent
		transform.FollowMapExtent = true;
		canvasView.InvalidateSurface();
	}

	void OnZoomIn(object sender, EventArgs e)
	{
		transform.ScaleBy(2);
		canvasView.InvalidateSurface();
	}

	void OnZoomOut(object sender, EventArgs e)
	{
		transform.ScaleBy(0.5);
		canvasView.InvalidateSurface();
	}

	void OnMapPressed(object sender, PointerEventArgs e)
	{
		if (e.GetPosition(canvasView) is Point pt)
		{
			PointF mapPoint = transform.ToMap(pt.X, pt.Y);
			transform.PanTo(mapPoint);
			transform.ScaleBy(2.0);
			canvasView.InvalidateSurface();
		}
	}
}
