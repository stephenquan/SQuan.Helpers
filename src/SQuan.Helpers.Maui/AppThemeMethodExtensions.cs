namespace SQuan.Helpers.Maui;

/// <summary>
/// Sets an application theme-specific value for a specified bindable property.
/// </summary>
public static class AppThemeMethodExtensions
{
	/// <summary>
	/// Sets a binding on the specified <see cref="BindableObject"/> that applies different values  based on the
	/// application's current theme (light or dark).
	/// </summary>
	/// <remarks>This method uses an <see cref="AppThemeBinding"/> to dynamically update the target property  based
	/// on the application's theme. The binding ensures that the appropriate value is applied  when the theme
	/// changes.</remarks>
	/// <param name="bindable">The <see cref="BindableObject"/> on which the binding will be set. Cannot be <see langword="null"/>.</param>
	/// <param name="targetProperty">The <see cref="BindableProperty"/> to bind to. This specifies the property of the <paramref name="bindable"/> 
	/// object that will be updated based on the theme.</param>
	/// <param name="light">The value to apply when the application is using the light theme. Can be <see langword="null"/>.</param>
	/// <param name="dark">The value to apply when the application is using the dark theme. Can be <see langword="null"/>.</param>
	/// <returns>The same <see cref="BindableObject"/> instance passed as <paramref name="bindable"/>,  allowing for method
	/// chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="bindable"/> is <see langword="null"/>.</exception>
	public static BindableObject SetAppTheme(this BindableObject bindable, BindableProperty targetProperty, object? light, object? dark)
	{
		if (bindable is null)
		{
			throw new ArgumentNullException(nameof(bindable));
		}

		bindable.SetBinding(targetProperty, AppThemeBinding.Create(light, dark));

		return bindable;
	}
}
