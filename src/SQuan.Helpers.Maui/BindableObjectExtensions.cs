// BindableObjectExtensions.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides extension methods for bindable objects to facilitate fluent and intuitive data binding in user interface
/// development.
/// </summary>
public static class BindableObjectExtensions
{
	/// <summary>
	/// Sets the binding to the specified <paramref name="targetProperty"/> with the provided <paramref name="binding"/>.
	/// </summary>
	/// <typeparam name="TBindable">The type of the <paramref name="bindable"/> object.</typeparam>
	/// <param name="bindable">The bindable object to set the binding on.</param>
	/// <param name="targetProperty">The property to apply the binding to.</param>
	/// <param name="binding">The binding to apply to the <paramref name="targetProperty"/>.</param>
	/// <returns>The <paramref name="bindable"/> instance to allow for fluently building the user interface.</returns>
	public static TBindable Bind<TBindable>(this TBindable bindable, BindableProperty targetProperty, BindingBase binding) where TBindable : BindableObject
	{
		bindable.SetBinding(targetProperty, binding);
		return bindable;
	}
}
