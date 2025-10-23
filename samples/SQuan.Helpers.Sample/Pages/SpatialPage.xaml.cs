// SpatialPage.xaml.cs

using System.Text.RegularExpressions;
using SkiaSharp;
using SQLite;
using SQuan.Helpers.SQLiteSpatial;

namespace SQuan.Helpers.Sample;

#pragma warning disable CA1001 // Suppress IDisposable warning for SQLiteConnection

public partial class SpatialPage : ContentPage
{
	SQLiteSpatialConnection db = new(":memory:");
	NetTopologySuite.Geometries.Geometry? selection;
	bool loaded = false;

	public SpatialPage()
	{
		InitializeComponent();
		mapView.SetMapExtent(-125, 24.3, -66.9, 49.4); // USA extent
		Dispatcher.Dispatch(async () => await PostInitialize());
	}

	async Task PostInitialize()
	{
		// Load the spatial schema into the in-memory database.
		await LoadSchema(db, "schema.sql");
		await LoadSchema(db, "usa_cities.sql");
		await LoadSchema(db, "usa_states.sql");

		// Do some test spatial queries
		double? area_50_units = db.ExecuteScalar<double?>("SELECT ST_Area('POLYGON((10 10,20 10,20 20,10 10))')");
		string? centroid_at_16_13 = db.ExecuteScalar<string?>("SELECT ST_Centroid('POLYGON((10 10,20 10,20 20,10 10))')");
		string? circle_buffer = db.ExecuteScalar<string?>("SELECT ST_Buffer('POINT(10 10)', 5)");
		double? distance_5_units = db.ExecuteScalar<double?>("SELECT ST_Distance('POINT(0 0)', 'POINT(3 4)')");
		double? area_100_units = db.ExecuteScalar<double?>("SELECT ST_Area(ST_Envelope(ST_Buffer('POINT(10 10)', 5)))");

		// Retrieve cities in order of distance, starting with those nearest to Los Angeles.
		var results = db.Query<SpatialData>("SELECT * FROM UsaCities ORDER BY ST_Distance(WKT, 'POINT(-118.243683 34.052235)')");
		foreach (var result in results)
		{
			System.Diagnostics.Trace.WriteLine("City (spatial sort): " + result.Name);
		}

		// Create a selection 1.0 degree (111 km) buffer around Los Angeles, CA.
		selection = new NetTopologySuite.Geometries.Point(-118.2437, 34.0522).Buffer(1.0);

		// Finalize loading.
		loaded = true;
		mapView.InvalidateSurface();
	}

	[GeneratedRegex(@"^[^\s]+.*;\s*$")]
	private partial Regex sqlEndRegex();

	async Task LoadSchema(SQLiteConnection db, string fileName)
	{
		using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
		db.RunInTransaction(() =>
		{
			using (var reader = new System.IO.StreamReader(stream))
			{
				string sql = string.Empty;
				string? line = null;
				while ((line = reader.ReadLine()) is not null)
				{
					if (line.StartsWith("--") || string.IsNullOrWhiteSpace(line))
					{
						continue;
					}

					sql += line + "\n";
					if (sqlEndRegex().IsMatch(line))
					{
						db.Execute(sql);
						sql = string.Empty;
					}
				}
			}
		});
	}

	void mapView_PaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
	{
		if (!loaded)
		{
			return;
		}

		SKSurface surface = e.Surface;
		SKCanvas canvas = surface.Canvas;

		var mapExtent = mapView.EffectiveMapExtent;

		canvas.Clear();
		int statesDrawn = DrawSpatialData(canvas, $@"
SELECT s.*
FROM   UsaStates s,
       UsaStates_rtree r
WHERE  r.MinX <= {mapExtent.MaxX} AND r.MaxX >= {mapExtent.MinX}
AND    r.MinY <= {mapExtent.MaxY} AND r.MaxY >= {mapExtent.MinY}
AND    s.Id = r.Id");
		mapView.DrawMapGeometry(canvas, selection, "", Colors.Orange);
		int citiesDrawn = DrawSpatialData(canvas, $@"
SELECT c.*
FROM   UsaCities c,
       UsaCities_rtree r
WHERE  r.MinX <= {mapExtent.MaxX} AND r.MaxX >= {mapExtent.MinX}
AND    r.MinY <= {mapExtent.MaxY} AND r.MaxY >= {mapExtent.MinY}
AND    c.Id = r.Id");
		System.Diagnostics.Trace.WriteLine($"Drawn {statesDrawn} states and {citiesDrawn} cities.");
	}

	int DrawSpatialData(SKCanvas canvas, string sqlQuery)
		=> db.Query<SpatialData>(sqlQuery)
			.Select(item => mapView.DrawMapGeometry(
				canvas,
				item.WKT.ToGeometry(),
				item.Name,
				Color.FromArgb(item.Color)))
			.Count();

	void OnReset(object sender, EventArgs e) => mapView.SetMapExtent(-125, 24.3, -66.9, 49.4);
	void OnZoomIn(object sender, EventArgs e) => mapView.Zoom(2.0);
	void OnZoomOut(object sender, EventArgs e) => mapView.Zoom(0.5);
	void OnMapTapped(object sender, MapViewEventArgs e) => mapView.PanTo(e.MapPoint);
}
