// LucideTests.cs

namespace SQuan.Helpers.Maui.UnitTests;

public class LucideTests
{
	[Fact]
	public void LucideIcons_FontFile_IsATrueTypeFontFileName()
	{
		Assert.NotNull(SQuan.Helpers.Maui.LucideIcons.FontFile);
		Assert.NotEmpty(SQuan.Helpers.Maui.LucideIcons.FontFile);
		Assert.EndsWith(".ttf", SQuan.Helpers.Maui.LucideIcons.FontFile, StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public void LucideIcons_FontFamily_IsNotNullOrEmpty()
	{
		Assert.NotNull(SQuan.Helpers.Maui.LucideIcons.FontFamily);
		Assert.NotEmpty(SQuan.Helpers.Maui.LucideIcons.FontFamily);
	}

	[Fact]
	public void LucideIcons_SmileAndGlobe_AreDistinctIcons()
	{
		Assert.NotNull(SQuan.Helpers.Maui.LucideIcons.Smile);
		Assert.NotNull(SQuan.Helpers.Maui.LucideIcons.Globe);
		Assert.NotEmpty(SQuan.Helpers.Maui.LucideIcons.Smile);
		Assert.NotEmpty(SQuan.Helpers.Maui.LucideIcons.Globe);
		Assert.NotEqual(SQuan.Helpers.Maui.LucideIcons.Smile, SQuan.Helpers.Maui.LucideIcons.Globe);
	}
}
