// ThemePage.xaml.cs

using SQuan.Helpers.Maui;

namespace SQuan.Helpers.Sample;

public partial class ThemePage : ContentPage
{
	public ImageSource Sun { get; } = "sun32.png";
	public ImageSource Moon { get; } = "moon32.png";

	public ThemePage()
	{
		InitializeComponent();

		AppThemeMethodExtensions.SetAppTheme(
			logoImage,
			Image.SourceProperty,
			BindingBase.Create(static (ImageSource s) => s, BindingMode.OneWay, source: Sun),
			BindingBase.Create(static (ImageSource s) => s, BindingMode.OneWay, source: Moon));
	}

	void OnToggleTheme(object sender, EventArgs e)
	{
		AppThemeManager.Current.UserAppTheme = AppThemeManager.Current.UserAppTheme switch
		{
			AppTheme.Light => AppTheme.Dark,
			AppTheme.Dark => AppTheme.Light,
			_ => AppTheme.Dark
		};
	}
}
