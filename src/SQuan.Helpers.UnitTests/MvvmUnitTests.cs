// MvvmUnitTests.cs

using SQuan.Helpers.Maui.UnitTests.Mocks;

namespace SQuan.Helpers.Maui.UnitTests;

public class MvvmUnitTests
{
	[Fact]
	public void OnPropertyChanged_WhenBindablePropertyInitialized_EventCountIsCorrect()
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var view = new CustomContentView();
		Assert.Equal(42, view.Magic);
		Assert.Equal(1, view.MagicChangedCount);
		view.Magic = 100;
		Assert.Equal(100, view.Magic);
		Assert.Equal(2, view.MagicChangedCount);
	}
}
