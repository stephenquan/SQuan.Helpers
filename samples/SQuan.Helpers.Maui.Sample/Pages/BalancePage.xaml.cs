using System.Globalization;

namespace SQuan.Helpers.Maui.Sample;

public partial class BalancePage : ContentPage
{
	public List<CultureInfo> SupportedCultures { get; } =
	[
		new CultureInfo("en-US"),
		new CultureInfo("en-GB"),
		new CultureInfo("fr-FR"),
		new CultureInfo("zh-CN"),
	];

	int cultureIndex = 0;

	public CultureInfo CurrentCulture => SupportedCultures[cultureIndex];

	public BalancePage()
	{
		BindingContext = this;
		InitializeComponent();
	}

	void ChangeCulture(object sender, EventArgs e)
	{
		cultureIndex = (cultureIndex + 1) % SupportedCultures.Count;
		OnPropertyChanged(nameof(CurrentCulture));
	}
}
