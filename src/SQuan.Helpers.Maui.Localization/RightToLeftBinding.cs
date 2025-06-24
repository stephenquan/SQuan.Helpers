namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a factory method for creating a binding that reflects the current right-to-left text direction state.
/// </summary>
public static class RightToLeftBinding
{
	/// <summary>
	/// Creates a new <see cref="BindingBase"/> instance that binds to the  right-to-left layout state of the current UI
	/// culture.
	/// </summary>
	/// <param name="converter">An optional <see cref="IValueConverter"/> to apply to the binding value.  If null, no conversion is applied.</param>
	/// <param name="converterParameter">An optional parameter to pass to the <paramref name="converter"/>.  This can be used to influence the conversion
	/// logic.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the  right-to-left layout state of the
	/// current UI culture.</returns>
	public static BindingBase Create(IValueConverter? converter = null, object? converterParameter = null)
		=> BindingBase.Create(
			static (LocalizationManager lm) => lm.CurrentUICulture.TextInfo.IsRightToLeft,
			BindingMode.OneWay,
			source: LocalizationManager.Current,
			converter: converter,
			converterParameter: converterParameter);
}
