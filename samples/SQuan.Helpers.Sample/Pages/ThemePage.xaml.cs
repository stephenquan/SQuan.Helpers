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

		logoImage.Bind(
			Image.SourceProperty,
			AppThemeBindingBase.Create(
				BindingBase.Create(static (object o) => o, BindingMode.OneWay, source: Sun),
				BindingBase.Create(static (object o) => o, BindingMode.OneWay, source: Moon),
				this));
	}

	void OnToggleTheme(object sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(Application.Current);
		Application.Current.UserAppTheme = Application.Current.UserAppTheme switch
		{
			AppTheme.Dark => AppTheme.Light,
			_ => AppTheme.Dark,
		};
	}
}
