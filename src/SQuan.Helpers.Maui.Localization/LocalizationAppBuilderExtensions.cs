// LocalizationAppBuilderExtensions.cs

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Configures the application to support localization by adding the required localization services.
/// </summary>
public static class LocalizationAppBuilderExtensions
{
	/// <summary>
	/// Initializes localization services in the Maui application.
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="options"></param>
	/// <param name="resourceTypes">Optional resource types to register for localization. If none are provided, the method will still register the localization services.</param>
	/// <returns></returns>
	[Obsolete("Use UseSQuanHelpersMauiLocalization instead.")]
	public static MauiAppBuilder UseSQuanHelperMauiLocalization(this MauiAppBuilder builder, LocalizationOptions? options = null, params Type[] resourceTypes)
	{
		builder.Services.AddLocalization();
		if (options is not null)
		{
			LocalizationManager.Options = options;
		}
		LocalizationManager.RegisterStringResource(resourceTypes);
		return builder;
	}

	/// <summary>
	/// Initializes localization services in the Maui application using a specific resource type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="options"></param>
	/// <param name="builder"></param>
	/// <returns></returns>
	[Obsolete("Use UseSQuanHelpersMauiLocalization instead.")]
	public static MauiAppBuilder UseSQuanHelperMauiLocalization<T>(this MauiAppBuilder builder, LocalizationOptions? options = null)
	{
		return UseSQuanHelperMauiLocalization(builder, options, typeof(T));
	}

	/// <summary>
	/// Initializes localization services in the Maui application.
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="options"></param>
	/// <param name="resourceTypes">Optional resource types to register for localization. If none are provided, the method will still register the localization services.</param>
	/// <returns></returns>
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization(this MauiAppBuilder builder, LocalizationOptions? options = null, params Type[] resourceTypes)
	{
		builder.Services.AddLocalization();
		if (options is not null)
		{
			LocalizationManager.Options = options;
		}
		LocalizationManager.RegisterStringResource(resourceTypes);
		return builder;
	}

	/// <summary>
	/// Initializes localization services in the Maui application using a specific resource type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="options"></param>
	/// <param name="builder"></param>
	/// <returns></returns>
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization<T>(this MauiAppBuilder builder, LocalizationOptions? options = null)
	{
		return UseSQuanHelpersMauiLocalization(builder, options, typeof(T));
	}
}
