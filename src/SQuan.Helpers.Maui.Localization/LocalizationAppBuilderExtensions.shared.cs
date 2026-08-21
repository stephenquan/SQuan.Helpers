// LocalizationAppBuilderExtensions.shared.cs

using System.Globalization;
using Microsoft.Extensions.Localization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Configures the application to support localization by adding the required localization services.
/// </summary>
public static class LocalizationAppBuilderExtensions
{
	/// <summary>
	/// Configures the Maui application to use localization services with the specified localization options and resource types.
	/// </summary>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="options"></param>
	/// <param name="resourceTypes">Optional resource types to register for localization. If none are provided, the method will still register the localization services.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use the UseSQuanHelpersMauiLocalization(Func<string, CultureInfo?, string?>) overload instead.")]
	public static MauiAppBuilder UseSQuanHelperMauiLocalization(this MauiAppBuilder builder, LocalizationOptions? options = null, params Type[] resourceTypes)
		=> resourceTypes.Length > 0
			? UseSQuanHelperMauiLocalization(builder, options, resourceTypes[0])
			: UseSQuanHelperMauiLocalization(builder);

	/// <summary>
	/// Configures the Maui application to use localization services with the specified localization options.
	/// </summary>
	/// <typeparam name="T">The type of the resource to register for localization.</typeparam>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="options">Optional localization options.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use the UseSQuanHelpersMauiLocalization(Func<string, CultureInfo?, string?>) overload instead.")]
	public static MauiAppBuilder UseSQuanHelperMauiLocalization<T>(this MauiAppBuilder builder, LocalizationOptions? options = null)
		=> UseSQuanHelperMauiLocalization(builder, options, typeof(T));

	/// <summary>
	/// Configures the Maui application to use localization services with the specified localization options and resource types.
	/// </summary>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="options">Optional localization options.</param>
	/// <param name="resourceTypes">Optional resource types to register for localization. If none are provided, the method will still register the localization services.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use the UseSQuanHelpersMauiLocalization(Func<string, CultureInfo?, string?>) overload instead.")]
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization(this MauiAppBuilder builder, LocalizationOptions? options = null, params Type[] resourceTypes)
		=> resourceTypes.Length > 0
			? UseSQuanHelperMauiLocalization(builder, options, resourceTypes[0])
			: UseSQuanHelperMauiLocalization(builder);

	/// <summary>
	/// Configures the Maui application to use localization services with the specified localization options.
	/// </summary>
	/// <typeparam name="T">The type of the resource to register for localization.</typeparam>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="options">Optional localization options.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use the UseSQuanHelpersMauiLocalization(Func<string, CultureInfo?, string?>) overload instead.")]
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization<T>(this MauiAppBuilder builder, LocalizationOptions? options = null)
		=> UseSQuanHelpersMauiLocalization(builder, options, typeof(T));

	/// <summary>
	/// Configures the Maui application to use localization services with the specified string resource type.
	/// </summary>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="stringResource">The type that contains the string resources for localization,
	/// which will be registered with the localization manager.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use the UseSQuanHelpersMauiLocalization(Func<string, CultureInfo?, string?>) overload instead.")]
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization(this MauiAppBuilder builder, Type stringResource)
		=> builder.UseSQuanHelpersMauiLocalization(
			new Func<string, CultureInfo?, string?>((key, culture) =>
			{
				var stringLocalizerType = typeof(IStringLocalizer<>).MakeGenericType(new Type[] { stringResource });
				var stringLocalizer = (IStringLocalizer?)IPlatformApplication.Current?.Services.GetService(stringLocalizerType);
				var localizedString = stringLocalizer?.GetString(key);
				return localizedString?.Value;
			}));

	/// <summary>
	/// Configures the Maui application to use localization services with the specified string resource type.
	/// </summary>
	/// <typeparam name="T">The type that contains the string resources for localization,
	/// which will be registered with the localization manager.</typeparam>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use the UseSQuanHelpersMauiLocalization(Func<string, CultureInfo?, string?>) overload instead.")]
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization<T>(this MauiAppBuilder builder)
		=> UseSQuanHelpersMauiLocalization(builder, typeof(T));

	/// <summary>
	/// Configures the Maui application to use localization services with a custom localization provider function.
	/// </summary>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="localizationProvider">
	/// The custom localization provider function that determines how localization resources are resolved within the application.
	/// For resources you can supply a reference to the ResourceManager.GetString method.
	/// </param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization(this MauiAppBuilder builder, Func<string, CultureInfo?, string?> localizationProvider)
	{
		builder.Services.AddLocalization();
		LocalizationManager.Current.LocalizationProvider = localizationProvider;
		return builder;
	}
}
