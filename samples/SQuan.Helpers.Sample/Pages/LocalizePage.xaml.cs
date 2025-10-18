using System.Globalization;
using SQuan.Helpers.Maui.Localization;
using SQuan.Helpers.Maui.Mvvm;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

namespace SQuan.Helpers.Sample;

public partial class LocalizePage : ContentPage
{
	[ObservableProperty] public partial int Count { get; set; } = 0;

	[ObservableProperty] public partial double? X1 { get; set; } = 10.0;

	[ObservableProperty] public partial string Expression { get; set; } = "x0 * x1";

	[ObservableProperty] public partial DateTime? Modified { get; set; }

	public List<CultureInfo> SupportedCultures { get; } =
	[
		new CultureInfo("en-US"),
		new CultureInfo("fr-FR"),
		new CultureInfo("zh-CN"),
		new CultureInfo("ar-AR"),
	];

	public int CultureIndex { get; set; } = 0;

	public LocalizationManager LM { get; } = LocalizationManager.Current;

	public LocalizePage()
	{
		BindingContext = this;

		InitializeComponent();

		/*
		CounterBtn.Localize(
			Button.TextProperty,
			"BTN_CLICKED_N_TIMES",
			BindingBase.Create(static (LocalizePage m) => m.Count, BindingMode.OneWay, source: this));
		*/
	}

	[RelayCommand]
	void IncrementCounter()
	{
		Count++;

		/*
		if (Count == 1)
		{
			CounterBtn.Localize(Button.TextProperty, "BTN_CLICKED_1_TIME", Count);
		}
		else
		{
			CounterBtn.Localize(Button.TextProperty, "BTN_CLICKED_N_TIMES", Count);
		}
		*/

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	[RelayCommand]
	void ChangeUICulture()
	{
		LM.CurrentUICulture = SupportedCultures[CultureIndex = (CultureIndex + 1) % SupportedCultures.Count];
	}
}
