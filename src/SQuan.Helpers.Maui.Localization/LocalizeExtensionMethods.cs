namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides extension methods for localization functionality.
/// </summary>
public static class LocalizeExtensionMethods
{
	/// <summary>
	/// Applies a localized binding to the specified bindable property of a <see cref="BindableObject"/>.
	/// </summary>
	/// <remarks>This method creates a binding for the specified property using a localized string retrieved by the
	/// provided resource key. If <paramref name="args"/> are supplied, they will be used to format the localized
	/// string.</remarks>
	/// <param name="bindable">The <see cref="BindableObject"/> to which the localization binding will be applied.</param>
	/// <param name="targetProperty">The bindable property that will receive the localized value.</param>
	/// <param name="key">The resource key used to retrieve the localized string.</param>
	/// <param name="args">Optional arguments to format the localized string.</param>
	/// <returns>The <see cref="BindableObject"/> with the applied localization binding, allowing for method chaining.</returns>
	public static BindableObject Localize(this BindableObject bindable, BindableProperty targetProperty, string key, params object?[]? args)
	{
		bindable.SetBinding(targetProperty, LocalizeBinding.Create(key, args));
		return bindable;
	}

	/// <summary>
	/// Applies a localized binding to the specified bindable property of a <see cref="BindableObject"/>.
	/// </summary>
	/// <remarks>This method creates a localized binding by combining the provided localization key and optional
	/// arguments. The resulting binding is applied to the specified bindable property of the target object.</remarks>
	/// <param name="bindable">The <see cref="BindableObject"/> to which the localized binding will be applied.</param>
	/// <param name="targetProperty">The <see cref="BindableProperty"/> that will receive the localized value.</param>
	/// <param name="keyBinding">The binding that provides the localization key.</param>
	/// <param name="args">Optional arguments to format the localized value. These arguments will be used to replace placeholders in the
	/// localized string, if applicable.</param>
	/// <returns>The <see cref="BindableObject"/> with the applied localized binding, allowing for method chaining.</returns>
	public static BindableObject Localize(this BindableObject bindable, BindableProperty targetProperty, BindingBase keyBinding, params object?[]? args)
	{
		bindable.SetBinding(targetProperty, LocalizeBinding.Create(keyBinding, args));
		return bindable;
	}
}
