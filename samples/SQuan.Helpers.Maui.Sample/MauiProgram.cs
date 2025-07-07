using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using SQuan.Helpers.Maui.Localization;
using SQuan.Helpers.Maui.Sample.Resources.Strings;

namespace SQuan.Helpers.Maui.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup()
			.UseSQuanHelperMauiLocalization<AppStrings>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		Routing.RegisterRoute(nameof(CountPage), typeof(CountPage));
		Routing.RegisterRoute(nameof(CardPage), typeof(CardPage));
		Routing.RegisterRoute(nameof(BalancePage), typeof(BalancePage));
		Routing.RegisterRoute(nameof(LocalizePage), typeof(LocalizePage));
		Routing.RegisterRoute(nameof(ThemePage), typeof(ThemePage));
		Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));

		return builder.Build();
	}
}
