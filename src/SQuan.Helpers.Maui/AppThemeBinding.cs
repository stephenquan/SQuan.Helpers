namespace SQuan.Helpers.Maui;


/// <summary>
/// Creates a binding that dynamically selects a value based on the application's current theme.
/// </summary>
/// <remarks>This method creates a binding that reacts to changes in the application's theme. It combines the
/// provided light and dark values or bindings with a theme binding that tracks the application's requested theme. The
/// resulting <see cref="MultiBinding"/> uses an <see cref="AppThemeConverter"/> to determine which value or binding to
/// apply based on the current theme.  If either light or dark is already a <see
/// cref="BindingBase"/>, it will be used directly. Otherwise, a new binding is created for the provided
/// value.</remarks>
public static class AppThemeBinding
{
	/// <summary>
	/// Creates a binding that dynamically selects between light and dark themes based on the application's requested
	/// theme.
	/// </summary>
	/// <remarks>The returned binding uses the application's current theme to determine whether to apply the light
	/// or dark theme. If <paramref name="light"/> or <paramref name="dark"/> is not a binding, it will be converted into a
	/// one-way binding.</remarks>
	/// <param name="light">An optional binding or value representing the light theme. If not a binding, the value will be wrapped in a
	/// binding.</param>
	/// <param name="dark">An optional binding or value representing the dark theme. If not a binding, the value will be wrapped in a binding.</param>
	/// <returns>A <see cref="MultiBinding"/> that combines the theme selection logic with the provided light and dark theme
	/// bindings or values.</returns>
	public static BindingBase Create(object? light = null, object? dark = null)
	{
		BindingBase themeBinding = BindingBase.Create(static (AppThemeManager m) => m.RequestedTheme, BindingMode.OneWay, source: AppThemeManager.Current);
		BindingBase lightBinding = light is BindingBase _lightBinding ? _lightBinding : BindingBase.Create(static (object? o) => o, BindingMode.OneWay, source: light);
		BindingBase darkBinding = dark is BindingBase _darkBinding ? _darkBinding : BindingBase.Create(static (object? o) => o, BindingMode.OneWay, source: dark);
		return new MultiBinding
		{
			Bindings = new BindingBase[]
			{
				themeBinding,
				lightBinding,
				darkBinding
			},
			Converter = new AppThemeConverter(),
			Mode = BindingMode.OneWay
		};
	}
}
