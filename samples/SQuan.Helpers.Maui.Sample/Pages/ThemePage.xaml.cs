namespace SQuan.Helpers.Maui.Sample;

public partial class ThemePage : ContentPage
{
	public string Sun { get; } = "sun32.png";
	public string Moon { get; } = "moon32.png";

	public ThemePage()
	{
		InitializeComponent();

		logoImage.SetAppTheme(
			Image.SourceProperty,
			BindingBase.Create(static (string s) => s, BindingMode.OneWay, source: Sun),
			BindingBase.Create(static (string s) => s, BindingMode.OneWay, source: Moon));
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
