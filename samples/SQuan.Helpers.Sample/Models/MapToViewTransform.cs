namespace SQuan.Helpers.Sample;

public class MapToViewTransform
{
	public Rect MapExtent { get; set; } = Rect.FromLTRB(-125, 24.3, -66.9, 49.4); // USA extent

	public bool FollowMapExtent { get; set; } = true;

	public PointF ViewCenter { get; set; } = new PointF(0, 0);

	public double ViewScale { get; set; } = 1.0;

	public void UpdateScale(double viewWidth, double viewHeight)
	{
		if (MapExtent.Width > 0 && MapExtent.Height > 0)
		{
			double scaleX = viewWidth / MapExtent.Width;
			double scaleY = viewHeight / MapExtent.Height;
			ViewScale = Math.Min(scaleX, scaleY) * 0.9;
		}
	}

	public void UpdateCenter(double viewWidth, double viewHeight)
	{
		ViewCenter = new PointF((float)(viewWidth / 2), (float)(viewHeight / 2));
	}

	public void SetMapExtent(double mapX, double mapY, double mapWidth, double mapHeight)
	{
		MapExtent = new Rect(mapX - mapWidth / 2, mapY - mapHeight / 2, mapWidth, mapHeight);
	}
	public void PanTo(double mapX, double mapY)
	{
		SetMapExtent(mapX, mapY, MapExtent.Width, MapExtent.Height);
	}

	public void PanTo(Point mapPoint) => PanTo(mapPoint.X, mapPoint.Y);

	public void ScaleBy(double zoomFactor)
	{
		SetMapExtent(
			MapExtent.Center.X,
			MapExtent.Center.Y,
			MapExtent.Width / zoomFactor,
			MapExtent.Height / zoomFactor);
	}

	public PointF ToView(double mapX, double mapY)
		=> new PointF(
			(float)(ViewCenter.X + (mapX - MapExtent.Center.X) * ViewScale),
			(float)(ViewCenter.Y - (mapY - MapExtent.Center.Y) * ViewScale));

	public Point ToMap(double viewX, double viewY)
		=> new Point(
			(viewX - ViewCenter.X) / ViewScale + MapExtent.Center.X,
			-(viewY - ViewCenter.Y) / ViewScale + MapExtent.Center.Y);
}
