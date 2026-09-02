// ThemePage.xaml.cs

using CommunityToolkit.Maui.Markup;
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
				BindingBase.Create(static (ThemePage ctx) => ctx.Sun, BindingMode.OneWay, source: this),
				BindingBase.Create(static (ThemePage ctx) => ctx.Moon, BindingMode.OneWay, source: this),
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
