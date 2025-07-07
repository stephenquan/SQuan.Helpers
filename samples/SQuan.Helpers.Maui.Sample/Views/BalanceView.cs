using System.Globalization;
using CommunityToolkit.Maui.Markup;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class BalanceView : Label
{
	[BindableProperty] public partial double Balance { get; set; } = 0.0;
	[BindableProperty] public partial CultureInfo Culture { get; set; } = CultureInfo.InstalledUICulture;

	public BalanceView()
	{
		this
			.Bind(
				Label.TextColorProperty,
				static (BalanceView ctx) => ctx.Balance,
				source: this,
				convert: static (double balance) => balance < 0 ? Colors.Red : Colors.Green)
			.Bind(
				Label.TextProperty,
				binding1: BindingBase.Create(static (BalanceView ctx) => ctx.Balance, BindingMode.OneWay, source: this),
				binding2: BindingBase.Create(static (BalanceView ctx) => ctx.Culture, BindingMode.OneWay, source: this),
				convert: static ((double balance, CultureInfo? culture) v) => v.balance.ToString("C", v.culture)
			);
	}
}
