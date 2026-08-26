// MvvmUnitTests.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.UnitTests;

public partial class MvvmUnitTests : BaseTest
{
	[Theory]
	[InlineData(0, 1)]
	[InlineData(42, 0)]
	public void OnPropertyChanged_WhenBindablePropertyInitialized_EventCountIsCorrect(int initializer, int expectedChangeCount)
	{
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
		var view = new CustomContentView();
		Assert.Null(view.Culture);
		Assert.Equal(0, view.CultureChangedCount);
	}

	[Theory]
	[InlineData("fr-FR", 1)]
	public void NonDefaultCultureInitializer_EventCountIsCorrect(string cultureName, int expectedCount)
	{
		var view = new CustomContentView() { Culture = new CultureInfo(cultureName) };
		Assert.Equal(cultureName, view.Culture?.Name);
		Assert.Equal(expectedCount, view.CultureChangedCount);
	}
}
