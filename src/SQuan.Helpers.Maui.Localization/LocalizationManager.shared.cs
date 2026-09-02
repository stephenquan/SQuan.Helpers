// LocalizationManager.shared.cs

using System.Globalization;
using SQuan.Helpers.Internals;
namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality for managing and accessing localized resources.
/// </summary>
public partial class LocalizationManager : InternalObservableObject
{
	/// <summary>
	/// Gets or sets the options for localization behavior in the application.
	/// </summary>
	[Obsolete("This property is not currently used and may be removed in future versions.")]
	public static LocalizationOptions Options { get; set; } = new LocalizationOptions();

	/// <summary>
	/// Gets or sets the resolver used to provide localized resources.
	/// </summary>
	[Obsolete("This property is not currently used and may be removed in future versions.")]
	[InternalObservableProperty]
	public partial LocalizeResolver? Resolver { get; set; }

	/// <summary>
	/// Gets the current instance of the <see cref="LocalizationManager"/>.
	/// </summary>
	public static LocalizationManager Current { get; } = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="LocalizationManager"/> class.
	/// </summary>
	LocalizationManager()
	{
	}

	/// <summary>
	/// Gets or sets the delegate used to provide localized strings based on a specified key and culture.
	/// </summary>
	public Func<string, CultureInfo?, string?>? LocalizationProvider { get; set; }

	/// <summary>
	/// Gets or sets the current culture used by the application.
	/// </summary>
	public CultureInfo CurrentCulture
	{
		get;
		set => SetProperty(ref field, CultureInfo.CurrentCulture = value);
	} = CultureInfo.CurrentCulture;

	/// <summary>
	/// Gets or sets the current UI culture used by the application.
	/// </summary>
	public CultureInfo CurrentUICulture
	{
		get;
		set => SetProperty(ref field, CultureInfo.CurrentUICulture = value);
	} = CultureInfo.CurrentUICulture;

	/// <summary>
	/// Gets a localized string for the specified key using the current UI culture and current culture, with optional formatting arguments.
	/// </summary>
	/// <param name="key">The key of the string resource.</param>
	/// <param name="args">Optional arguments for string formatting.</param>
	/// <returns>The localized string.</returns>
	public string? GetString(string key, params object?[] args)
		=> GetString(key, CultureInfo.CurrentUICulture, CultureInfo.CurrentCulture, args);

	/// <summary>
	/// Gets a localized string for the specified key using the current culture.
	/// </summary>
	/// <param name="key">The key of the string resource.</param>
	/// <param name="culture">The culture to use for localization.</param>
	/// <param name="args">Optional arguments for string formatting.</param>
	/// <returns>The localized string.</returns>
	public string? GetString(string key, CultureInfo? culture = null, params object?[] args)
		=> GetString(key, culture, CultureInfo.CurrentCulture, args);

	/// <summary>
	/// Gets a localized string for the specified key using the provided UI culture and culture, with optional formatting arguments.
	/// </summary>
	/// <param name="currentUICulture">The current UI culture to use for localization.</param>
	/// <param name="currentCulture">The current culture to use for string formatting.</param>
	/// <param name="key">The key of the string resource.</param>
	/// <param name="args">Optional arguments for string formatting.</param>
	/// <returns>The localized string.</returns>
	public string? GetString(string key, CultureInfo? currentUICulture, CultureInfo? currentCulture, params object?[] args)
	{
		string? localizedString = LocalizationProvider?.Invoke(key, currentUICulture ?? CultureInfo.CurrentUICulture);
		return !string.IsNullOrEmpty(localizedString) && args.Length > 0
			? string.Format(currentCulture ?? CultureInfo.CurrentCulture, localizedString, args)
			: localizedString;
	}
}
