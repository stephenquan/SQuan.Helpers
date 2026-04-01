// LocalizePage.xaml.cs

using System.Globalization;
using SQuan.Helpers.Maui.Localization;
using SQuan.Helpers.Maui.Mvvm;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

namespace SQuan.Helpers.Sample;

public partial class LocalizePage : ContentPage
{
	public FormContext FormContext { get; } = new();
	[ObservableProperty] public partial int Count { get; internal set; } = 0;
	[BindableProperty] public partial CultureInfo ScopedUICulture { get; set; } = cultureEN;
	[BindableProperty] public partial CultureInfo FormUICulture { get; set; } = cultureEN;
	static readonly CultureInfo cultureEN = new CultureInfo("en-US");
	static readonly CultureInfo cultureFR = new CultureInfo("fr-FR");
	static readonly CultureInfo cultureZH = new CultureInfo("zh-CN");
	static readonly CultureInfo cultureAR = new CultureInfo("ar-AR");

	public LocalizePage()
	{
		BindingContext = this;
		InitializeComponent();
	}

	[RelayCommand]
	void IncrementCounter()
	{
		Count++;
		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	[RelayCommand]
	public void ToggleAppCulture()
		=> LocalizationManager.Current.CurrentUICulture = ToggleCultureCore(LocalizationManager.Current.CurrentUICulture);

	[RelayCommand]
	public void SystemCulture()
		=> LocalizationManager.Current.CurrentUICulture = CultureInfo.InstalledUICulture;

	[RelayCommand]
	public void ToggleScopedCulture()
		=> ScopedUICulture = ToggleCultureCore(ScopedUICulture);

	[RelayCommand]
	public void ToggleFormCulture()
		=> FormUICulture = ToggleCultureCore(FormUICulture);

	CultureInfo ToggleCultureCore(CultureInfo culture)
		=> culture = culture.Name switch
		{
			"fr-FR" => cultureZH,
			"zh-CN" => cultureAR,
			"ar-AR" => cultureEN,
			_ => cultureFR,
		};
}
