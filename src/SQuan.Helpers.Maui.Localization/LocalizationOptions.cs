namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides configuration options for localization behavior in the application.
/// </summary>
public class LocalizationOptions
{
	/// <summary>
	/// Gets or sets a value indicating whether the application should automatically follow changes to the installed UI
	/// culture.
	/// </summary>
	public bool FollowInstalledUICultureChanges { get; set; } = true;
}
