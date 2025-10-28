// MapViewEventArgs.cs

namespace SQuan.Helpers.Sample;

/// <summary>
/// Provides data for events related to map view interactions.
/// </summary>
public class MapViewEventArgs : EventArgs
{
	/// <summary>
	/// Gets or sets the point in view coordinates where the event occurred.
	/// </summary>
	public Point ViewPoint { get; set; }

	/// <summary>
	/// Gets or sets the corresponding point in map coordinates.
	/// </summary>
	public Point MapPoint { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MapViewEventArgs"/> class.
	/// </summary>
	/// <param name="viewPoint">The point in view coordinates where the event occurred.</param>
	/// <param name="mapPoint">The corresponding point in map coordinates.</param>
	public MapViewEventArgs(Point viewPoint, Point mapPoint)
	{
		ViewPoint = viewPoint;
		MapPoint = mapPoint;
	}
}
