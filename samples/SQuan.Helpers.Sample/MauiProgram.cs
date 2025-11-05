// MauiProgram.cs

using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SQuan.Helpers.Maui;
using SQuan.Helpers.Maui.Localization;
using SQuan.Helpers.Sample.Resources.Strings;

namespace SQuan.Helpers.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup()
			.UseSkiaSharp()
			.UseSQuanHelpersMaui()
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
		SamplesHelper.RegisterSample("Popup Demo", nameof(PopupPage), typeof(PopupPage));
		SamplesHelper.RegisterSample("Spatial Demo", nameof(SpatialPage), typeof(SpatialPage));

		return builder.Build();
	}
}
