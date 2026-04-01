// LocalizationAppBuilderExtensions.shared.cs

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
	[Obsolete("This method is deprecated. Use a different overload that accepts a LocalizeResolver or register string resources separately.")]
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
	/// Configures the Maui application to use localization services with the specified localization options.
	/// </summary>
	/// <typeparam name="T">The type of the resource to register for localization.</typeparam>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="options">Optional localization options.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use a different overload that accepts a LocalizeResolver or register string resources separately.")]
	public static MauiAppBuilder UseSQuanHelperMauiLocalization<T>(this MauiAppBuilder builder, LocalizationOptions? options = null)
	{
		return UseSQuanHelperMauiLocalization(builder, options, typeof(T));
	}

	/// <summary>
	/// Configures the Maui application to use localization services with the specified localization options and resource types.
	/// </summary>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="options">Optional localization options.</param>
	/// <param name="resourceTypes">Optional resource types to register for localization. If none are provided, the method will still register the localization services.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use a different overload that accepts a LocalizeResolver or register string resources separately.")]
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
	/// Configures the Maui application to use localization services with the specified localization options.
	/// </summary>
	/// <typeparam name="T">The type of the resource to register for localization.</typeparam>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="options">Optional localization options.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	[Obsolete("This method is deprecated. Use a different overload that accepts a LocalizeResolver or register string resources separately.")]
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization<T>(this MauiAppBuilder builder, LocalizationOptions? options = null)
	{
		return UseSQuanHelpersMauiLocalization(builder, options, typeof(T));
	}

	/// <summary>
	/// Configures the MAUI application to use SQuanHelpers for localization with a specified localization resolver.
	/// </summary>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="resolver">The localization resolver that determines how localization resources are resolved within the application.</param>
	/// <returns>The same MAUI app builder instance, enabling further configuration chaining.</returns>
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization(this MauiAppBuilder builder, LocalizeResolver resolver)
	{
		builder.Services.AddLocalization();
		LocalizationManager.Current.Resolver = resolver;
		return builder;
	}

	/// <summary>
	/// Configures the Maui application to use localization services with the specified string resource type.
	/// </summary>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <param name="stringResource">The type that contains the string resources for localization,
	/// which will be registered with the localization manager.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization(this MauiAppBuilder builder, Type stringResource)
	{
		builder.Services.AddLocalization();
		LocalizationManager.RegisterStringResource(stringResource);
		return builder;
	}

	/// <summary>
	/// Configures the Maui application to use localization services with the specified string resource type.
	/// </summary>
	/// <typeparam name="T">The type that contains the string resources for localization,
	/// which will be registered with the localization manager.</typeparam>
	/// <param name="builder">The Maui application builder to configure with SQuanHelpers localization support.</param>
	/// <returns>The same MauiAppBuilder instance, allowing for method chaining.</returns>
	public static MauiAppBuilder UseSQuanHelpersMauiLocalization<T>(this MauiAppBuilder builder)
	{
		return UseSQuanHelpersMauiLocalization(builder, typeof(T));
	}
}
