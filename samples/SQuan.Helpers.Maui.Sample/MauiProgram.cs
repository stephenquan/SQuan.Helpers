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

		SamplesHelper.RegisterSample("Count Demo", nameof(CountPage), typeof(CountPage));
		SamplesHelper.RegisterSample("Count Dynamic Demo", nameof(CountDynamicPage), typeof(CountDynamicPage));
		SamplesHelper.RegisterSample("Card Demo", nameof(CardPage), typeof(CardPage));
		SamplesHelper.RegisterSample("Balance Demo", nameof(BalancePage), typeof(BalancePage));
		SamplesHelper.RegisterSample("Localization Demo", nameof(LocalizePage), typeof(LocalizePage));
		SamplesHelper.RegisterSample("Theme Demo", nameof(ThemePage), typeof(ThemePage));
		SamplesHelper.RegisterSample("Search Demo", nameof(SearchPage), typeof(SearchPage));

		return builder.Build();
	}
}
