// BaseBindableObjectMarkupExtension.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a base class to simplify the creation of markup extensions that are also bindable objects in .NET MAUI.
/// </summary>
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public abstract class BaseBindableObjectMarkupExtension : BindableObject, IMarkupExtension<BindingBase>
{
	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> ProvideValue(serviceProvider);

	/// <summary>
	/// Implementation of the ProvideValue method from IMarkupExtension interface.
	/// This method is called when the markup extension is used in XAML.
	/// </summary>
	/// <param name="serviceProvider">The service provider used to retrieve services.</param>
	/// <returns>The binding for the markup extension.</returns>
	public BindingBase ProvideValue(IServiceProvider serviceProvider)
	{
		// To simplify the usage of the markup extension, we automatically set the BindingContext of the extension
		// to the target object if it is a BindableObject and the BindingContext is not already set.
		if (!IsSet(BindingContextProperty)
			&& serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget
			&& provideValueTarget.TargetObject is BindableObject targetObject)
		{
			this.SetBinding(BindingContextProperty, static (BindableObject b) => b.BindingContext, BindingMode.OneWay, source: targetObject);
		}
		return ProvideBindingValue(serviceProvider);
	}

	/// <summary>
	/// The derived class focuses on returning a multi-binding that mashes the values of the bindable properties into a single value.
	/// This method is called by ProvideValue to get the binding for the markup extension.
	/// </summary>
	/// <param name="serviceProvider">The service provider used to retrieve services.</param>
	/// <returns>The binding for the markup extension.</returns>
	public abstract BindingBase ProvideBindingValue(IServiceProvider serviceProvider);
}
