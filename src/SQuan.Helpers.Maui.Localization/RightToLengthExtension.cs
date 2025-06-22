namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides an extension method for creating a binding that supports right-to-left text layout.
/// </summary>
/// <remarks>This extension is typically used in XAML to create bindings that adapt to right-to-left text layouts.
/// It implements the <see cref="IMarkupExtension{T}"/> interface to provide a binding instance.</remarks>
public class RightToLengthExtension : IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Provides a binding object that can be used in XAML to bind data to a target property.
	/// </summary>
	/// <remarks>This method is typically used in XAML scenarios to create and return a binding object. The returned
	/// binding is configured to support right-to-left data flow.</remarks>
	/// <param name="serviceProvider">An object that provides services for the markup extension. This parameter is typically used to resolve services
	/// such as the target object and property where the binding is applied.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the binding to be applied.</returns>
	public BindingBase ProvideValue(IServiceProvider serviceProvider)
		=> RightToLeftBinding.Create();

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> ProvideValue(serviceProvider);
}
