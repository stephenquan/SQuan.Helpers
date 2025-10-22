// MvvmUnitTests.cs

using System.Globalization;
using SQuan.Helpers.Maui.UnitTests.Mocks;

namespace SQuan.Helpers.Maui.UnitTests;

public class MvvmUnitTests
{
	[Theory]
	[InlineData(0, 0)]
	[InlineData(42, 1)]
	public void OnPropertyChanged_WhenBindablePropertyInitialized_EventCountIsCorrect(int initializer, int expectedChangeCount)
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var view = new CustomContentView() { Magic = initializer };
		Assert.Equal(initializer, view.Magic);
		Assert.Equal(expectedChangeCount, view.MagicChangedCount);
		view.Magic++;
		Assert.Equal(initializer + 1, view.Magic);
		Assert.Equal(expectedChangeCount + 1, view.MagicChangedCount);
	}

	[Fact]
	public void DefaultCultureInitializer_EventCountIsCorrect()
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var view = new CustomContentView();
		Assert.Null(view.Culture);
		Assert.Equal(0, view.CultureChangedCount);
	}

	[Theory]
	[InlineData("fr-FR", 1)]
	public void NonDefaultCultureInitializer_EventCountIsCorrect(string cultureName, int expectedCount)
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var view = new CustomContentView() { Culture = new CultureInfo(cultureName) };
		Assert.Equal(cultureName, view.Culture?.Name);
		Assert.Equal(expectedCount, view.CultureChangedCount);
	}
}
