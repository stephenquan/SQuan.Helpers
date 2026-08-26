// LocalizePage.xaml.cs

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using SQuan.Helpers.Maui;
using SQuan.Helpers.Maui.Localization;
using SQuan.Helpers.Maui.Mvvm;
using SQuan.Helpers.Sample.Resources.Strings;
using BindablePropertyAttribute = CommunityToolkit.Maui.BindablePropertyAttribute;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

namespace SQuan.Helpers.Sample;

[SuppressMessage(
	"Design",
	"CA1001:Types that own disposable fields should be disposable",
	Justification = "The CancellationTokenSource is created and disposed through the page lifecycle in OnAppearing and OnDisappearing.")]
public partial class LocalizePage : ContentPage
{
	CancellationTokenSource? cts;
	ExpressionManager em;
	bool initialized = false;

	public FormContext FormContext { get; }
	[ObservableProperty] public partial int Count { get; internal set; } = 0;
	[BindableProperty] public partial CultureInfo ScopedUICulture { get; set; } = cultureEN;
	[BindableProperty] public partial CultureInfo FormUICulture { get; set; } = cultureEN;
	static readonly CultureInfo cultureEN = new CultureInfo("en-US");
	static readonly CultureInfo cultureFR = new CultureInfo("fr-FR");
	static readonly CultureInfo cultureZH = new CultureInfo("zh-CN");
	static readonly CultureInfo cultureAR = new CultureInfo("ar-AR");

	public LocalizePage()
	{
		em = new ExpressionManager();
		FormContext = new(em);

		LocalizationManager.Current.LocalizationProvider = (key, culture) =>
		{
			if (key.StartsWith("/"))
			{
				return FormContext.GetIText(key, culture ?? CultureInfo.CurrentUICulture);
			}

			return AppStrings.ResourceManager.GetString(key, culture ?? CultureInfo.CurrentUICulture);
		};

		BindingContext = this;

		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (!initialized)
		{
			initialized = true;
			cts = new CancellationTokenSource();
			em.StartWorkLoop(cts.Token);
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		cts?.Cancel();
		cts?.Dispose();
		cts = null;
		initialized = false;
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
