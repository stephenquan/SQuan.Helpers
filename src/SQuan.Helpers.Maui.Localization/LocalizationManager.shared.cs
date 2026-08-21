// LocalizationManager.shared.cs

using System.Globalization;
using SQuan.Helpers.Internals;
namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality for managing and accessing localized resources.
/// </summary>
public partial class LocalizationManager : ObservableObject
{
	/// <summary>
	/// Gets or sets the options for localization behavior in the application.
	/// </summary>
	[Obsolete("This property is not currently used and may be removed in future versions.")]
	public static LocalizationOptions Options { get; set; } = new LocalizationOptions();

	/// <summary>
	/// Gets or sets the resolver used to provide localized resources.
	/// </summary>
	[ObservableProperty]
	public partial LocalizeResolver? ResolverX { get; set; }

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
	/// Occurs when the installed UI culture of the application changes.
	/// </summary>
	[Obsolete("This event is not currently used and may be removed in future versions.")]
	public event EventHandler? InstalledUICultureChanged;

	/// <summary>
	/// Occurs when the current culture of the application changes.
	/// </summary>
	[Obsolete("This event is not currently used and may be removed in future versions.")]
	public event EventHandler? CurrentCultureChanged;

	/// <summary>
	/// Occurs when the current UI culture of the application changes.
	/// </summary>
	[Obsolete("This event is not currently used and may be removed in future versions.")]
	public event EventHandler? CurrentUICultureChanged;

	/// <summary>Gets or sets a value indicating whether the current UI culture should follow the installed UI culture.</summary>
	[Obsolete("This property is not currently used and may be removed in future versions.")]
	[ObservableProperty]
	public partial bool FollowInstalledUICulture { get; set; }

	/// <summary>Gets the current installed UI culture.</summary>
	[Obsolete("This property is not currently used and may be removed in future versions.")]
	public CultureInfo InstalledUICulture
	{
		get => CultureInfo.InstalledUICulture;
	}

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
	/// Checks for changes in current culture settings.
	/// </summary>
	[Obsolete("This method is not currently used and may be removed in future versions.")]
	public void Poll()
	{
	}

	/// <summary>
	/// Gets the delegate used to resolve localized string resources based on a specified key.
	/// </summary>
	[Obsolete("This property is not currently used and may be removed in future versions.")]
	public LocalizeResolver StringResourceResolver { get; } = new LocalizeResolver((key, culture) => key);

	/// <summary>
	/// Gets a localized string for the specified key using the current culture.
	/// </summary>
	/// <param name="key">The key of the string resource.</param>
	/// <param name="culture">The culture to use for localization.</param>
	/// <param name="args">Optional arguments for string formatting.</param>
	/// <returns>The localized string.</returns>
	public string? GetString(string key, CultureInfo? culture = null, params object?[] args)
	{
		CultureInfo? _culture = culture ?? CultureInfo.CurrentUICulture;
		string? localizedString = null;
		if (LocalizationProvider is not null)
		{
			localizedString = LocalizationProvider(key, _culture);
		}

		if (!string.IsNullOrEmpty(localizedString) && args.Length > 0)
		{
			localizedString = string.Format(localizedString, args);
		}

		return localizedString;
	}
}
