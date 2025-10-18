using NetTopologySuite.IO;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SQuan.Helpers.Sample;

public static partial class SKCanvasExtensions
{
	public static void DrawSpatialData(this SkiaSharp.SKCanvas canvas, IEnumerable<SpatialData> sampleData, MapToViewTransform transform)
	{
		foreach (var item in sampleData)
		{
			DrawSampleData(canvas, item, transform);
		}
	}

	public static void DrawSampleData(this SkiaSharp.SKCanvas canvas, SpatialData sampleData, MapToViewTransform transform)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(sampleData.Geometry)
				&& new WKTReader() is WKTReader wktReader
				&& wktReader.Read(sampleData.Geometry) is NetTopologySuite.Geometries.Geometry geometry
				&& Color.TryParse(sampleData.Color, out Color color))
			{
				DrawGeometry(canvas, sampleData.Name, color, geometry, transform);
			}
		}
		catch (Exception)
		{
		}
	}

	public static void DrawGeometry(this SkiaSharp.SKCanvas canvas, string name, Color color, NetTopologySuite.Geometries.Geometry geometry, MapToViewTransform transform)
	{
		List<Point> points = [];
		switch (geometry)
		{
			case NetTopologySuite.Geometries.Point pt:
				DrawPoint(canvas, name, color, pt.X, pt.Y, transform);
				break;
			case NetTopologySuite.Geometries.Polygon polygon:
				for (int i = 0; i < polygon.ExteriorRing.Coordinates.Length; i++)
				{
					var point = polygon.ExteriorRing.Coordinates[i];
					points.Add(new Point(point.X, point.Y));
				}
				DrawPolygon(canvas, "", color, points, transform);
				break;
			default:
				// Handle other geometry types if needed
				break;
		}
	}

	public static void DrawPoint(this SkiaSharp.SKCanvas canvas, string name, Color color, double x, double y, MapToViewTransform transform)
	{
		PointF viewPoint = transform.ToView(x, y);
		canvas.DrawCircle(viewPoint.X, viewPoint.Y, 5, new SKPaint { Color = color.ToSKColor(), IsAntialias = true });
		canvas.DrawText(name, viewPoint.X + 12, viewPoint.Y + 12, new SKFont { Size = 12 }, new SKPaint { Color = color.ToSKColor(), IsAntialias = true });
	}

	public static void DrawPolygon(this SkiaSharp.SKCanvas canvas, string name, Color color, List<Point> points, MapToViewTransform transform)
	{
		if (points.Count < 2)
		{
			return;
		}
		List<PointF> viewPoints = [];
		using var paint = new SKPaint
		{
			Color = color.ToSKColor(),
			IsStroke = true,
			StrokeWidth = 1,
			IsAntialias = true
		};
		using var path = new SKPath();
		for (int i = 0; i < points.Count; i++)
		{
			var pt = points[i];
			PointF viewPoint = transform.ToView(pt.X, pt.Y);
			if (i == 0)
			{
				path.MoveTo(new SKPoint(viewPoint.X, viewPoint.Y));
			}
			else
			{
				path.LineTo(new SKPoint(viewPoint.X, viewPoint.Y));
			}
		}
		path.Close();
		canvas.DrawPath(path, paint);
	}
}
