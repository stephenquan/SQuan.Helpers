namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Specifies flags that indicate which culture settings should be changed during a culture update operation.
/// </summary>
[Flags]
public enum CultureFlags
{
	/// <summary>
	/// Indicates that no culture settings have changed.
	/// </summary>
	None = 0,

	/// <summary>
	/// Indicates that the installed UI culture is changing or has changed.
	/// </summary>
	InstalledUICulture = 1,

	/// <summary>
	/// Indicates that the current culture is changing or has changed.
	/// </summary>
	/// <remarks>This value indicates that formatting operations will use the culture settings of the current
	/// thread.</remarks>
	CurrentCulture = 2,

	/// <summary>
	/// Indicates that the current UI culture is changing or has changed.
	/// </summary>
	CurrentUICulture = 4,

	/// <summary>
	/// Indicates that there may be multiple culture flags that have changed.
	/// </summary>
	All = InstalledUICulture | CurrentCulture | CurrentUICulture
}
