namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a markup extension that creates a binding for the Y-axis rotation property.
/// </summary>
/// <remarks>This extension is used to bind a value to the Y-axis rotation property in XAML.  It returns a <see
/// cref="BindingBase"/> instance that can be used in data binding scenarios.</remarks>
[AcceptEmptyServiceProvider]
public class RotationYExtension : IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Provides a binding object that can be used in XAML to bind a property to a rotation value on the Y-axis.
	/// </summary>
	/// <param name="serviceProvider">An object that provides services for the markup extension. This parameter is typically used in XAML scenarios and
	/// can be ignored in most cases.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the binding for the rotation value on the Y-axis.</returns>
	public BindingBase ProvideValue(IServiceProvider serviceProvider)
		=> RotationYBinding.Create();

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> ProvideValue(serviceProvider);
}
