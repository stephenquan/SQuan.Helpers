using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Localization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality for managing and accessing localized resources.
/// </summary>
public partial class LocalizationManager : ObservableObject
{
	/// <summary>
	/// Gets or sets the options for localization behavior in the application.
	/// </summary>
	public static LocalizationOptions Options { get; set; } = new LocalizationOptions();

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
				Stringlocalizer = null,
				IsInitialized = false
			};
		}
	}

	internal static void RegisterStringResource<T>()
	{
		RegisterStringResource(typeof(T));
	}

	static LocalizationManager? instance = null;

	/// <summary>
	/// Gets the current instance of the <see cref="LocalizationManager"/>.
	/// </summary>
	public static LocalizationManager Current
	{
		get
		{
			if (instance is null)
			{
				instance = new LocalizationManager();
			}
			return instance;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="LocalizationManager"/> class.
	/// </summary>
	public LocalizationManager()
	{
		IDispatcher? dispatcher = Application.Current?.Dispatcher;
		if (dispatcher is not null)
		{
			var _timer = dispatcher.CreateTimer();
			_timer.Interval = TimeSpan.FromSeconds(1);
			_timer.Tick += (s, e) =>
			{
				this.Poll(CultureFlags.All);
			};
			_timer.Start();
		}
	}

	/// <summary>
	/// Occurs when the installed UI culture of the application changes.
	/// </summary>
	public event EventHandler? InstalledUICultureChanged;

	/// <summary>
	/// Occurs when the current culture of the application changes.
	/// </summary>
	public event EventHandler? CurrentCultureChanged;

	/// <summary>
	/// Occurs when the current UI culture of the application changes.
	/// </summary>
	public event EventHandler? CurrentUICultureChanged;

	string? installedUICultureName = CultureInfo.InstalledUICulture.Name;

	/// <summary>Gets the current installed UI culture.</summary>
	public CultureInfo InstalledUICulture
	{
		get => CultureInfo.InstalledUICulture;
	}

	string? currentCultureName = CultureInfo.CurrentCulture.Name;

	/// <summary>
	/// Gets or sets the current culture used by the application.
	/// </summary>
	public CultureInfo CurrentCulture
	{
		get => CultureInfo.CurrentCulture;
		set
		{
			if (value is not null && value.Name != CultureInfo.CurrentCulture.Name)
			{
				CultureInfo.CurrentCulture = value;
				currentCultureName = value.Name;
				OnPropertyChanged(nameof(CurrentCulture));
				WeakReferenceMessenger.Default.Send(new CultureChangedMessage(CultureFlags.CurrentCulture));
				CurrentCultureChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	string? currentUICultureName = CultureInfo.CurrentUICulture.Name;

	/// <summary>
	/// Gets or sets the current UI culture used by the application.
	/// </summary>
	public CultureInfo CurrentUICulture
	{
		get => CultureInfo.CurrentUICulture;
		set
		{
			if (value is not null && value.Name != CultureInfo.CurrentUICulture.Name)
			{
				CultureInfo.CurrentUICulture = value;
				currentUICultureName = value.Name;
				OnPropertyChanged(nameof(CurrentUICulture));
				WeakReferenceMessenger.Default.Send(new CultureChangedMessage(CultureFlags.CurrentUICulture));
				CurrentUICultureChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	/// <summary>
	/// Checks for changes in current culture settings.
	/// </summary>
	public void Poll(CultureFlags changingFlags = CultureFlags.All)
	{
		CultureFlags changedFlags = CultureFlags.None;

		if (changingFlags.HasFlag(CultureFlags.InstalledUICulture))
		{
			CultureInfo.InstalledUICulture.ClearCachedData();
			if (installedUICultureName is null || installedUICultureName != CultureInfo.InstalledUICulture.Name)
			{
				installedUICultureName = CultureInfo.InstalledUICulture.Name;
				OnPropertyChanged(nameof(InstalledUICulture));
				InstalledUICultureChanged?.Invoke(this, EventArgs.Empty);
				changedFlags |= CultureFlags.InstalledUICulture;

				if (Options.FollowInstalledUICultureChanges)
				{
					CurrentUICulture = CultureInfo.InstalledUICulture;
					changingFlags |= CultureFlags.CurrentUICulture;
				}
			}
		}

		if (changingFlags.HasFlag(CultureFlags.CurrentCulture))
		{
			CultureInfo.CurrentCulture.ClearCachedData();
			if (currentCultureName is null || currentCultureName != CultureInfo.CurrentCulture.Name)
			{
				currentCultureName = CultureInfo.CurrentCulture.Name;
				OnPropertyChanged(nameof(CurrentCulture));
				CurrentCultureChanged?.Invoke(this, EventArgs.Empty);
				changedFlags |= CultureFlags.CurrentCulture;
			}
		}

		if (changingFlags.HasFlag(CultureFlags.CurrentUICulture))
		{
			CultureInfo.CurrentUICulture.ClearCachedData();
			if (currentUICultureName is null || currentUICultureName != CultureInfo.CurrentUICulture.Name)
			{
				currentUICultureName = CultureInfo.CurrentUICulture.Name;
				OnPropertyChanged(nameof(CurrentUICulture));
				CurrentUICultureChanged?.Invoke(this, EventArgs.Empty);
				changedFlags |= CultureFlags.CurrentUICulture;
			}
		}

		if (changedFlags != CultureFlags.None)
		{
			WeakReferenceMessenger.Default.Send(new CultureChangedMessage(changedFlags));
		}
	}

	/// <summary>
	/// Gets a localized string for the specified key using the current culture.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="culture"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	public string GetString(string key, CultureInfo? culture = null, params object[] args)
	{
		CultureInfo? activeCulture = culture ?? CultureInfo.CurrentUICulture;
		var services = IPlatformApplication.Current?.Services;

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
					info.Stringlocalizer = (IStringLocalizer?)services.GetService(stringLocalizerType);
				}
				info.IsInitialized = true;
			}

			if (info.Stringlocalizer is IStringLocalizer stringLocalizer)
			{
				var localizedString = stringLocalizer.GetString(key);
				if (!localizedString.ResourceNotFound)
				{
					if (args is not null && args.Length > 0)
					{
						return string.Format(localizedString.Value, args);
					}

					return localizedString.Value;
				}
			}
		}

		return $"[{activeCulture.Name}] {key}";
	}
}

