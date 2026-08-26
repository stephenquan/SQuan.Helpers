// SpatialData.cs

namespace SQuan.Helpers.Sample;

public partial class SpatialData
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Color { get; set; } = "Black";
	public byte[] Geometry { get; set; } = Array.Empty<byte>();
}
