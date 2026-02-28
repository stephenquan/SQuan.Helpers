// AspectViewTests.cs

using SQuan.Helpers.Maui.UnitTests.Mocks;

namespace SQuan.Helpers.Maui.UnitTests;

public class AspectViewTests
{
	[Fact]
	public async Task AspectView_WithChild_RespectsAspectRatio()
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var aspectView = new AspectView() { WidthRequest = 300, HeightRequest = 200 };
		TaskCompletionSource<bool> tcs = new();
		var grid = new Grid();
		grid.Loaded += (s, e) => tcs.SetResult(true);
		aspectView.Content = grid;
		Assert.Equal(1, aspectView.AspectRatio);
		await tcs.Task;
		Assert.Equal(200, grid.Width);
		Assert.Equal(200, grid.Height);
	}
}