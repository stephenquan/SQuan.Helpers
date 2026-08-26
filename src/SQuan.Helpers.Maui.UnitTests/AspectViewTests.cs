// AspectViewTests.cs

namespace SQuan.Helpers.Maui.UnitTests;

public partial class AspectViewTests : BaseTest
{
	[Theory]
	[InlineData(-1, 1)]
	[InlineData(0, 1)]
	[InlineData(1, 1)]
	[InlineData(2, 2)]
	public void AspectView_AspectRatio_CoercesInvalidValues(double aspectRatio, double expected)
	{
		var aspectView = new AspectView
		{
			AspectRatio = aspectRatio
		};

		Assert.Equal(expected, aspectView.AspectRatio);
	}
}