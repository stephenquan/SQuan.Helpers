using System.Globalization;
using CommunityToolkit.Maui.Markup;
using SQuan.Helpers.Maui.Localization;
using SQuan.Helpers.Maui.Mvvm;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

namespace SQuan.Helpers.Maui.Sample;

public partial class LocalizePage : ContentPage
{
	[ObservableProperty] public partial int Count { get; set; } = 0;

	public List<CultureInfo> SupportedCultures { get; } =
	[
		new CultureInfo("en-US"),
		new CultureInfo("fr-FR"),
		new CultureInfo("zh-CN"),
	];

	public int CultureIndex { get; set; } = 0;

	public LocalizationManager LM { get; } = LocalizationManager.Current;

	public LocalizePage()
	{
		BindingContext = this;
		InitializeComponent();
		CounterBtn.Bind(
			Button.TextProperty,
			binding1: LocalizeBinding.Create("BTN_CLICKED_N_TIMES"),
			binding2: BindingBase.Create(static (LocalizePage m) => m.Count, BindingMode.OneWay, source: this),
			convert: ((string? str, int count) v) => !string.IsNullOrEmpty(v.str) ? string.Format(v.str, v.count) : string.Empty);
	}

	[RelayCommand]
	void IncrementCounter()
	{
		Count++;
		//CounterBtn.Localize(Button.TextProperty, "BTN_CLICKED_N_TIMES", Count);
		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	[RelayCommand]
	void ChangeUICulture()
	{
		LM.CurrentUICulture = SupportedCultures[CultureIndex = (CultureIndex + 1) % SupportedCultures.Count];
	}
}
