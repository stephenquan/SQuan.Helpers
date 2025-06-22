namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a factory method for creating a binding that reflects the current right-to-left text direction state.
/// </summary>
public static class RightToLeftBinding
{
	/// <summary>
	/// Creates a new binding that reflects the right-to-left text direction of the current UI culture.
	/// </summary>
	public static BindingBase Create()
		=> BindingBase.Create(
			static (LocalizationManager lm) => lm.CurrentUICulture.TextInfo.IsRightToLeft,
			BindingMode.OneWay,
			source: LocalizationManager.Current);
}
