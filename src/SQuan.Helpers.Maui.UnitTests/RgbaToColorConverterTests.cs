// RgbaToColorConverterTests.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.UnitTests;

public class RgbaToColorConverterTests
{
	[Theory]
	[InlineData(255, 0, 128, 64)]
	[InlineData(0, 255, 0, 255)]
	public void Convert_ValidRgbaValues_ReturnsExpectedColor(int red, int green, int blue, int alpha)
	{
		var converter = new RgbaToColorConverter();

		var result = converter.Convert([red, green, blue, alpha], typeof(Color), null, CultureInfo.InvariantCulture);

		var color = Assert.IsType<Color>(result);
		Assert.Equal(red / 255f, color.Red, 3);
		Assert.Equal(green / 255f, color.Green, 3);
		Assert.Equal(blue / 255f, color.Blue, 3);
		Assert.Equal(alpha / 255f, color.Alpha, 3);
	}

	[Fact]
	public void Convert_MissingAlphaValue_ReturnsNull()
	{
		var converter = new RgbaToColorConverter();

		var result = converter.Convert([255, 0, 128], typeof(Color), null, CultureInfo.InvariantCulture);

		Assert.Null(result);
	}
}
