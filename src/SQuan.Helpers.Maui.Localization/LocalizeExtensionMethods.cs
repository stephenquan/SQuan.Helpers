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
	public static BindableObject Localize(this BindableObject bindable, BindableProperty targetProperty, string key, params object?[] args)
	{
		bindable.SetBinding(targetProperty, LocalizeBinding.Create(key, args));
		return bindable;
	}
}
