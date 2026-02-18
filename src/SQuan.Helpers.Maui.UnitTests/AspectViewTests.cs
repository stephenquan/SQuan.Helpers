// AspectViewTests.cs

using SQuan.Helpers.Maui.UnitTests.Mocks;

namespace SQuan.Helpers.Maui.UnitTests;

public class AspectViewTests
{
	[Fact]
	public void AspectView_WithChild_RespectsAspectRatio()
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var aspectView = new AspectView() { WidthRequest = 300, HeightRequest = 200 };
		var grid = new Grid();
		aspectView.Content = grid;
		Assert.Equal(1, aspectView.AspectRatio);
		Assert.Equal(200, grid.Width);
		Assert.Equal(200, grid.Height);
	}
}