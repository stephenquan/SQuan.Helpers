// AppThemeBindingBase.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides static methods for creating theme-aware bindings that dynamically select values based on the application's
/// current theme.
/// </summary>
public static class AppThemeBindingBase
{
	/// <summary>
	/// Creates a new binding for a Button's command parameter that dynamically selects a value based on the application's current theme.
	/// </summary>
	/// <param name="light">The value to use for the command parameter when the light theme is active. Can be null.</param>
	/// <param name="dark">The value to use for the command parameter when the dark theme is active. Can be null.</param>
	/// <param name="parent">The parent element for the Button. If null, the current application context is used as the parent.</param>
	/// <returns>A BindingBase instance that binds the Button's command parameter to the specified theme-dependent value.</returns>
	public static BindingBase Create(object? light, object? dark, Element? parent = null)
	{
		var b = new Button { Parent = parent ?? Application.Current };
		b.SetAppTheme(Button.CommandParameterProperty, light, dark);
		return BindingBase.Create(static (Button b) => b.CommandParameter, BindingMode.OneWay, source: b);
	}

	/// <summary>
	/// Creates a binding that dynamically selects between the specified light and dark bindings based on the current
	/// application theme.
	/// </summary>
	/// <param name="lightBinding">The binding to use when the application theme is set to light mode. Cannot be null.</param>
	/// <param name="darkBinding">The binding to use when the application theme is set to dark mode. Cannot be null.</param>
	/// <param name="parent">The parent element for the Button. If null, the current application context is used as the parent.</param>
	/// <returns>A binding that automatically applies either the light or dark binding depending on the active application theme.</returns>
	public static BindingBase Create(BindingBase lightBinding, BindingBase darkBinding, Element? parent = null)
		=> new MultiBinding
		{
			Bindings = [Create(AppTheme.Light, AppTheme.Dark, parent), lightBinding, darkBinding],
			Converter = new AppThemeConverter(),
			Mode = BindingMode.OneWay,
		};
}
