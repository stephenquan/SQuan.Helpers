// FlowDirectionBinding.cs

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a binding that determines the flow direction based on the current UI culture's text direction.
/// </summary>
[Obsolete("Use a binding to LocalizationManager.Current.CurrentUICulture.TextInfo.IsRightToLeft with BoolToObjectConverter instead.")]
public static class FlowDirectionBinding
{
	/// <summary>
	/// Creates a binding that determines the flow direction based on the current UI culture's text direction.
	/// </summary>
	/// <returns>This method is obsolete and will always return null.</returns>
	[Obsolete("Use a binding to LocalizationManager.Current.CurrentUICulture.TextInfo.IsRightToLeft with BoolToObjectConverter instead.")]
	public static BindingBase? Create()
		=> null;
}
