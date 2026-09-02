// RgbaToColorConverter.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// A converter that converts RGBA values to a <see cref="Color"/> object.
/// </summary>
public class RgbaToColorConverter : FuncToMultiConverter<int, int, int, int, Color>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RgbaToColorConverter"/> class.
	/// </summary>
	public RgbaToColorConverter() : base(Color.FromRgba) { }
}
