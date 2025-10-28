// MapView.cs

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace SQuan.Helpers.Sample;

/// <summary>
/// Implementation of a custom MapView using SKCanvasView.
/// </summary>
public partial class MapView : SKCanvasView
{
	/// <summary>
	/// Gets the map extent as a NetTopologySuite Envelope.
	/// </summary>
	public NetTopologySuite.Geometries.Envelope MapExtent { get; } = new();

	/// <summary>
	/// Computes the effective map extent based on the current map extent, view dimensions, and map units.
	/// </summary>
	public NetTopologySuite.Geometries.Envelope EffectiveMapExtent
	{
		get
		{
			NetTopologySuite.Geometries.Envelope result = new();
			var mapPoint = ToMapPoint(0, 0);
			result.Init(mapPoint.X, mapPoint.Y, mapPoint.X, mapPoint.Y);
			mapPoint = ToMapPoint(Width, 0);
			result.ExpandToInclude(mapPoint.X, mapPoint.Y);
			mapPoint = ToMapPoint(0, Height);
			result.ExpandToInclude(mapPoint.X, mapPoint.Y);
			mapPoint = ToMapPoint(Width, Height);
			result.ExpandToInclude(mapPoint.X, mapPoint.Y);
			return result;
		}
	}

	/// <summary>
	/// Event raised when the map is pressed.
	/// </summary>
	public event EventHandler<MapViewEventArgs>? Pressed;

	/// <summary>
	/// Event raised when the map is released.
	/// </summary>
	public event EventHandler<MapViewEventArgs>? Released;

	/// <summary>
	/// Event raised when the map is tapped.
	/// </summary>
	public event EventHandler<MapViewEventArgs>? Tapped;

	/// <summary>
	/// Initializes a new instance of the <see cref="MapView"/> class.
	/// </summary>
	public MapView()
	{
		// Recognize pointer pressed and released events.
		PointerGestureRecognizer pointerGestureRecognizer = new();
		pointerGestureRecognizer.PointerPressed += (s, e) =>
		{
			if (e.GetPosition(this) is Point viewPoint)
			{
				Pressed?.Invoke(this, new MapViewEventArgs(viewPoint, ToMapPoint(viewPoint.X, viewPoint.Y)));
			}
		};
		pointerGestureRecognizer.PointerReleased += (s, e) =>
		{
			if (e.GetPosition(this) is Point viewPoint)
			{
				Released?.Invoke(this, new MapViewEventArgs(viewPoint, ToMapPoint(viewPoint.X, viewPoint.Y)));
			}
		};
		GestureRecognizers.Add(pointerGestureRecognizer);

		// Recognize touch gestures.
		TapGestureRecognizer touchGestureRecognizer = new();
		touchGestureRecognizer.Tapped += (s, e) =>
		{
			if (e.GetPosition(this) is Point viewPoint)
			{
				Tapped?.Invoke(this, new MapViewEventArgs(viewPoint, ToMapPoint(viewPoint.X, viewPoint.Y)));
			}
		};
		GestureRecognizers.Add(touchGestureRecognizer);

		// React to size changes to recalculate map units.
		PropertyChanged += (s, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(Width):
				case nameof(Height):
					CalculateMapUnits();
					break;
			}
		};
	}

	/// <summary>
	/// Sets the map extent to the specified coordinates.
	/// </summary>
	/// <param name="minX">The minimum X (longitude) value of the map extent.</param>
	/// <param name="minY">The minimum Y (latitude) value of the map extent.</param>
	/// <param name="maxX">The maximum X (longitude) value of the map extent.</param>
	/// <param name="maxY">The maximum Y (latitude) value of the map extent.</param>
	public MapView SetMapExtent(double minX, double minY, double maxX, double maxY)
	{
		MapExtent.Init(minX, maxX, minY, maxY);
		CalculateMapUnits();
		InvalidateSurface();
		return this;
	}

	/// <summary>
	/// Gets or sets the current map units (scale).
	/// </summary>
	public double MapUnits { get; set; } = 1.0;

	/// <summary>
	/// Gets the current map units (scale) based on the map extent and view dimensions.
	/// </summary>
	public MapView CalculateMapUnits()
	{
		MapUnits = (Width <= 1 || Height <= 1)
			? 1.0
			: Math.Max(MapExtent.Width / Width, MapExtent.Height / Height);
		return this;
	}

	/// <summary>
	/// Converts screen coordinates to map coordinates within the current map extent.
	/// </summary>
	/// <param name="viewX">The x-coordinate on the screen, in pixels.</param>
	/// <param name="viewY">The y-coordinate on the screen, in pixels.</param>
	/// <returns>A <see cref="NetTopologySuite.Geometries.Point"/> representing the corresponding map coordinates.</returns>
	public Point ToMapPoint(double viewX, double viewY)
	{
		double mapX = (MapExtent.MinX + MapExtent.MaxX) / 2 + (viewX - Width / 2) * MapUnits;
		double mapY = (MapExtent.MinY + MapExtent.MaxY) / 2 - (viewY - Height / 2) * MapUnits;
		return new Point(mapX, mapY);
	}

	/// <summary>
	/// Converts map coordinates to view coordinates.
	/// </summary>
	/// <remarks>This method transforms map coordinates into view coordinates based on the current map extent and
	/// view dimensions.</remarks>
	/// <param name="mapX">The X-coordinate in map units to be converted.</param>
	/// <param name="mapY">The Y-coordinate in map units to be converted.</param>
	/// <returns>A <see cref="Point"/> representing the corresponding view coordinates.</returns>
	public Point ToViewPoint(double mapX, double mapY)
	{
		double viewX = Width / 2 + (mapX - (MapExtent.MinX + MapExtent.MaxX) / 2) / MapUnits;
		double viewY = Height / 2 - (mapY - (MapExtent.MinY + MapExtent.MaxY) / 2) / MapUnits;
		return new Point(viewX, viewY);
	}

	/// <summary>
	/// Zooms the map view by the specified zoom factor, keeping the center of the current map extent fixed.
	/// </summary>
	/// <param name="zoomFactor">The factor by which to zoom the map. Values greater than 1 zoom in, values less than 1 zoom out.</param>
	public MapView Zoom(double zoomFactor)
	{
		double centerX = (MapExtent.MinX + MapExtent.MaxX) / 2;
		double centerY = (MapExtent.MinY + MapExtent.MaxY) / 2;
		double newWidth = MapExtent.Width / zoomFactor;
		double newHeight = MapExtent.Height / zoomFactor;
		SetMapExtent(
			centerX - newWidth / 2,
			centerY - newHeight / 2,
			centerX + newWidth / 2,
			centerY + newHeight / 2);
		InvalidateSurface();
		return this;
	}

	/// <summary>
	/// Pans the map view to center on the specified coordinates.
	/// </summary>
	/// <param name="mapX">The X-coordinate of the new center point of the map.</param>
	/// <param name="mapY">The Y-coordinate of the new center point of the map.</param>
	public MapView PanTo(double mapX, double mapY)
	{
		double halfWidth = MapExtent.Width / 2;
		double halfHeight = MapExtent.Height / 2;
		SetMapExtent(
			mapX - halfWidth,
			mapY - halfHeight,
			mapX + halfWidth,
			mapY + halfHeight);
		InvalidateSurface();
		return this;
	}

	/// <summary>
	/// Pans the view to the specified map point.
	/// </summary>
	/// <param name="mapPoint">The <see cref="Point"/> representing the target location to pan to.</param>
	public MapView PanTo(Point mapPoint)
		=> PanTo(mapPoint.X, mapPoint.Y);

	/// <summary>
	/// Draws the specified geometry onto the provided canvas using the given color.
	/// </summary>
	/// <remarks>Currently, only polygon geometries are supported. Other geometry types will not be drawn.</remarks>
	/// <param name="canvas">The canvas on which to draw the geometry. Cannot be null.</param>
	/// <param name="geometry">The geometry to be drawn. Must be a valid NetTopologySuite geometry object.</param>
	/// <param name="text">The text label associated with the geometry.</param>
	/// <param name="color">The color to use for drawing the geometry.</param>
	public MapView DrawMapGeometry(SKCanvas canvas, NetTopologySuite.Geometries.Geometry? geometry, string text, Color color)
	{
		switch (geometry)
		{
			case NetTopologySuite.Geometries.Point mapPoint:
				return DrawMapPoint(canvas, mapPoint, text, color);
			case NetTopologySuite.Geometries.Polygon mapPolygon:
				return DrawMapPolygon(canvas, mapPolygon, text, color);
		}
		return this;
	}

	/// <summary>
	/// Draws a point on the specified canvas at the location corresponding to the given map point, with an accompanying
	/// text label and specified color.
	/// </summary>
	/// <param name="canvas">The canvas on which to draw the point and text. Cannot be null.</param>
	/// <param name="mapPoint">The geographical point to be converted to a view point and drawn on the canvas. Cannot be null.</param>
	/// <param name="text">The text label to display next to the drawn point. Cannot be null or empty.</param>
	/// <param name="color">The color used to draw the point and text.</param>
	public MapView DrawMapPoint(SKCanvas canvas, NetTopologySuite.Geometries.Point mapPoint, string text, Color color)
	{
		Point viewPoint = ToViewPoint(mapPoint.X, mapPoint.Y);
		using SKPaint paint = new SKPaint
		{
			Color = color.ToSKColor(),
			IsAntialias = true
		};
		var dpiScale = (float)(DeviceDisplay.Current.MainDisplayInfo.Density);
		canvas.DrawCircle(
			(float)(viewPoint.X * dpiScale),
			(float)(viewPoint.Y * dpiScale),
			5 * dpiScale, paint);
		using SKFont font = new SKFont { Size = 12 * dpiScale };
		canvas.DrawText(
			text,
			(float)((viewPoint.X + 12) * dpiScale),
			(float)((viewPoint.Y + 12) * dpiScale),
			font,
			paint);
		return this;
	}

	/// <summary>
	/// Draws a polygon on the specified canvas.
	/// </summary>
	/// <param name="canvas">The canvas on which the polygon will be drawn. Cannot be null.</param>
	/// <param name="mapPolygon">The polygon geometry to be drawn. Must be a valid NetTopologySuite polygon object.</param>
	/// <param name="text">The text label associated with the geometry.</param>
	/// <param name="color">The color of the polygon.</param>
	public MapView DrawMapPolygon(SKCanvas canvas, NetTopologySuite.Geometries.Polygon mapPolygon, string text, Color color)
	{
		List<Point> points = [];
		for (int i = 0; i < mapPolygon.ExteriorRing.Coordinates.Length; i++)
		{
			var point = mapPolygon.ExteriorRing.Coordinates[i];
			points.Add(ToViewPoint(point.X, point.Y));
		}
		return DrawPolygon(canvas, points, color);
	}

	/// <summary>
	/// Draws a polygon on the specified canvas using the provided points and color.
	/// </summary>
	/// <remarks>The method uses anti-aliasing to smooth the edges of the polygon. The polygon is drawn with a
	/// stroke width of 2 units.</remarks>
	/// <param name="canvas">The canvas on which to draw the polygon. Must not be null.</param>
	/// <param name="viewPoints">A list of points defining the vertices of the polygon. Must contain at least two points.</param>
	/// <param name="color">The color to use for the polygon's outline.</param>
	public MapView DrawPolygon(SKCanvas canvas, List<Point> viewPoints, Color color)
	{
		if (viewPoints.Count < 2)
		{
			return this;
		}
		using SKPaint paint = new SKPaint
		{
			Style = SKPaintStyle.Stroke,
			Color = color.ToSKColor(),
			IsAntialias = true,
			StrokeWidth = 2
		};
		using SKPath path = new SKPath();
		var dpiScale = (float)(DeviceDisplay.Current.MainDisplayInfo.Density);
		path.MoveTo(
			(float)(viewPoints[0].X * dpiScale),
			(float)(viewPoints[0].Y * dpiScale));
		for (int i = 1; i < viewPoints.Count; i++)
		{
			path.LineTo(
				(float)(viewPoints[i].X * dpiScale),
				(float)(viewPoints[i].Y * dpiScale));
		}
		path.Close();
		canvas.DrawPath(path, paint);
		return this;
	}
}
