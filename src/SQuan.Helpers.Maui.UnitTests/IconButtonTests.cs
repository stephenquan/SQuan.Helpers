// IconButtonTests.cs

using Microsoft.Maui.Graphics.Converters;

namespace SQuan.Helpers.Maui.UnitTests;

public partial class IconButtonTests : BaseTest
{
	[Theory]
	[InlineData("Red", "Red")]
	[InlineData("Green", "Green")]
	[InlineData("Blue", "Blue")]
	public void IconButton_WhenTextColorIsSet_IconColorMatchesTextColor(string textColorName, string iconColorName)
	{
		ColorTypeConverter converter = new ColorTypeConverter();
		Color? textColor = (Color?)(converter.ConvertFromInvariantString(textColorName));
		Assert.NotNull(textColor);
		Color? iconColor = (Color?)(converter.ConvertFromInvariantString(iconColorName));
		Assert.NotNull(iconColor);
		var button = new IconButton();
		button.TextColor = textColor;
		Assert.Equal(iconColor.ToString(), button.IconColor.ToString());
	}

	[Fact]
	public void IconButton_IconDefaultValue_IsSetCorrectly()
	{
		var button = new IconButton();
		Assert.Equal(1, button.Icon.Length);
		Assert.NotNull(button.Icon);
		Assert.Equal(char.ConvertToUtf32(LucideIcons.Smile, 0), char.ConvertToUtf32(button.Icon, 0));
		Assert.Equal(LucideIcons.Smile, button.Icon);
	}

	[Fact]
	public void IconButton_IconColorDefaultValue_IsSetCorrectly()
	{
		var button = new IconButton();
		Assert.Equal(Colors.Black.ToString(), button.IconColor.ToString());
	}

	[Fact]
	public void IconButton_IconFontFamilyDefaultValue_IsSetCorrectly()
	{
		var button = new IconButton();
		Assert.Equal(LucideIcons.FontFamily, button.IconFontFamily);
	}

	[Fact]
	public void IconButton_IconSizeDefaultValue_IsSetCorrectly()
	{
		var button = new IconButton();
		Assert.Equal(24.0, button.IconSize);
	}
}
