// LocalizePage.xaml.cs

using System.Globalization;
using SQuan.Helpers.Maui.Localization;
using SQuan.Helpers.Maui.Mvvm;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

namespace SQuan.Helpers.Sample;

/// <summary>
/// Demonstrates localization feature with <see cref="LocalizationManager"/>.
/// </summary>
public partial class LocalizePage : ContentPage
{
	/// <summary>Simple counter to demonstrate localized string with pluralization.</summary>
	[ObservableProperty] public partial int Count { get; internal set; } = 0;

	/// <summary>Get or set the first variable (X0) for expression evaluation.</summary>

	[ObservableProperty] public partial double? X0 { get; set; } = 3.0;

	/// <summary>Get or set the second variable (X1) for expression evaluation.</summary>
	[ObservableProperty] public partial double? X1 { get; set; } = 4.0;

	/// <summary>Get or set the mathematical expression to evaluate.</summary>
	[ObservableProperty] public partial string Expression { get; set; } = "x0 * x1";

	/// <summary>Get or set the user date.</summary>
	[ObservableProperty] public partial DateTime? UserDate { get; set; }

	/// <summary>
	/// Get the list of supported cultures for localization.
	/// </summary>
	public List<CultureInfo> SupportedCultures { get; } =
	[
		new CultureInfo("en-US"),
		new CultureInfo("fr-FR"),
		new CultureInfo("zh-CN"),
		new CultureInfo("ar-AR"),
	];

	/// <summary>Get or set the current culture index in the <see cref="SupportedCultures"/> list.</summary>
	public int CultureIndex { get; set; } = 0;

	/// <summary>Get the current <see cref="LocalizationManager"/> instance.</summary>
	public LocalizationManager LM { get; } = LocalizationManager.Current;

	/// <summary>Initializes a new instance of the <see cref="LocalizePage"/> class.</summary>

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

	/// <summary>Cycles through the supported cultures and changes the current UI culture.</summary>
	[RelayCommand]
	public void ToggleCulture()
	{
		LM.CurrentUICulture = SupportedCultures[CultureIndex = (CultureIndex + 1) % SupportedCultures.Count];
	}

	/// <summary>Sets the current UI culture to the system's installed UI culture.</summary>
	[RelayCommand]
	public void SystemCulture()
	{
		LM.FollowInstalledUICulture = true;
	}
}
