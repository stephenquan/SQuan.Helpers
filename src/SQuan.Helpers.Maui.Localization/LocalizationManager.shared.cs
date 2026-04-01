// LocalizationManager.shared.cs

using System.Globalization;
using Microsoft.Extensions.Localization;
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
	public partial LocalizeResolver? Resolver { get; set; }

	/// <summary>
	/// Gets the collection of registered resources and their associated localization information.
	/// </summary>
	static Dictionary<Type, LocalizationStringResourceInfo> StringResources { get; } = new();

	internal static void RegisterStringResource(params Type[] resourceTypes)
	{
		if (resourceTypes is null || resourceTypes.Length == 0)
		{
			return;
		}

		foreach (Type? resourceType in resourceTypes)
		{
			if (resourceType is null)
			{
				continue;
			}

			StringResources[resourceType] = new LocalizationStringResourceInfo
			{
				Localizer = null,
				IsInitialized = false
			};
		}
	}

	internal static void RegisterStringResource<T>()
	{
		RegisterStringResource(typeof(T));
	}

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
	public partial CultureInfo CurrentCulture { get; set; } = CultureInfo.CurrentCulture;
	public partial CultureInfo CurrentCulture
	{
		get => field;
		set => SetProperty(ref field, CultureInfo.CurrentCulture = value);
	}

	/// <summary>
	/// Gets or sets the current UI culture used by the application.
	/// </summary>
	public partial CultureInfo CurrentUICulture { get; set; } = CultureInfo.CurrentUICulture;
	public partial CultureInfo CurrentUICulture
	{
		get => field;
		set => SetProperty(ref field, CultureInfo.CurrentUICulture = value);
	}

	/// <summary>
	/// Checks for changes in current culture settings.
	/// </summary>
	[Obsolete("This method is not currently used and may be removed in future versions.")]
	public void Poll()
	{
	}

	static string GetResourceString(string key, CultureInfo culture)
	{
		var services = IPlatformApplication.Current?.Services;

		if (string.IsNullOrEmpty(key))
		{
			return string.Empty;
		}

		foreach (Type? resourceType in StringResources.Keys)
		{
			if (resourceType is null)
			{
				continue;
			}

			var info = StringResources[resourceType];
			if (!info.IsInitialized)
			{
				if (services is not null)
				{
					var stringLocalizerType = typeof(IStringLocalizer<>).MakeGenericType(new Type[] { resourceType });
					info.Localizer = (IStringLocalizer?)services.GetService(stringLocalizerType);
				}
				info.IsInitialized = true;
			}

			if (info.Localizer is IStringLocalizer stringLocalizer)
			{
				var localizedString = stringLocalizer.GetString(key);
				if (!localizedString.ResourceNotFound)
				{
					return localizedString.Value;
				}
			}
		}

		return string.Empty;
	}

	/// <summary>
	/// Gets the delegate used to resolve localized string resources based on a specified key.
	/// </summary>
	public LocalizeResolver StringResourceResolver { get; } = GetResourceString;

	/// <summary>
	/// Gets a localized string for the specified key using the current culture.
	/// </summary>
	/// <param name="key">The key of the string resource.</param>
	/// <param name="culture">The culture to use for localization.</param>
	/// <param name="args">Optional arguments for string formatting.</param>
	/// <returns>The localized string.</returns>
	public string GetString(string key, CultureInfo? culture = null, params object?[]? args)
	{
		CultureInfo? _culture = culture ?? CultureInfo.CurrentUICulture;
		var resolver = Resolver ?? StringResourceResolver;
		if (resolver(key, _culture) is not string value)
		{
			return string.Empty;
		}

		if (args is not null && args.Length > 0)
		{
			value = string.Format(value, args);
		}

		return value;
	}
}
