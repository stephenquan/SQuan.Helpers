using SQuan.Helpers.Maui.UnitTests.Mocks;
using Microsoft.Maui.Graphics.Converters;
namespace SQuan.Helpers.Maui.UnitTests;

public class BalanceViewTests
{
	[Theory]
	[InlineData(123.45, "Green")]
	[InlineData(-432.10, "Red")]
	public void BalanceView_SetsTextColor_BasedOnBalance(double balance, string expectedColorName)
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var control = new SQuan.Helpers.Sample.BalanceView()
		{
			Balance = balance
		};
		ColorTypeConverter converter = new ColorTypeConverter();
		Color? expectedColor = (Color?)(converter.ConvertFromInvariantString(expectedColorName));
		Assert.NotNull(expectedColor);
		Assert.Equal(expectedColor, control.TextColor);
	}

	[Theory]
	[InlineData(123.45, "en-US", "$123.45")]
	[InlineData(-432.10, "fr-FR", "-432,10 €")]
	[InlineData(0, "de-DE", "0,00 €")]
	[InlineData(1000.00, "ja-JP", "￥1,000")]
	[InlineData(-999.99, "zh-CN", "-¥999.99")]
	public void BalanceView_SetsText_BasedOnBalanceAndCulture(double balance, string cultureName, string expectedText)
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var control = new SQuan.Helpers.Sample.BalanceView
		{
			Balance = balance,
			Culture = new System.Globalization.CultureInfo(cultureName)
		};
		Assert.Equal(expectedText, control.Text);
	}

	[Fact]
	public void BalanceView_DefaultValues_AreSetCorrectly()
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var control = new SQuan.Helpers.Sample.BalanceView()
		{
			Culture = new System.Globalization.CultureInfo("en-US")
		};
		Assert.Equal(0.0, control.Balance);
		Assert.Equal("en-US", control.Culture.Name);
		Assert.Equal(Colors.Green, control.TextColor);
		Assert.Equal("$0.00", control.Text);
	}

	[Theory]
	[InlineData(101.0, "en-US", 101.0, "en-US", 0, 0)]
	[InlineData(102.0, "fr-FR", 102.0, "de-DE", 0, 0)]
	[InlineData(103.0, "en-US", -50.0, "fr-FR", 2, 1)]
	[InlineData(104.0, "en-US", -50.0, "en-AU", 1, 1)]
	public void BalanceView_PropertyChanged_UpdatesTextAndTextColor(int initialBalance, string initialCulture, int newBalance, string newCulture, int expectedTextChanges, int expectedTextColorChanges)
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var control = new SQuan.Helpers.Sample.BalanceView
		{
			Balance = initialBalance,
			Culture = new System.Globalization.CultureInfo(initialCulture)
		};
		int textChangedCount = 0;
		int textColorChangedCount = 0;
		control.PropertyChanged += (s, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(control.Text):
					textChangedCount++;
					break;
				case nameof(control.TextColor):
					textColorChangedCount++;
					break;
			}
		};
		control.Balance = newBalance;
		control.Culture = new System.Globalization.CultureInfo(newCulture);
		Assert.Equal(expectedTextChanges, textChangedCount);
		Assert.Equal(expectedTextColorChanges, textColorChangedCount);
	}
}
