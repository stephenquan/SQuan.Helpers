namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides an extension method for creating a binding that supports right-to-left text layout.
/// </summary>
/// <remarks>This extension is typically used in XAML to create bindings that adapt to right-to-left text layouts.
/// It implements the <see cref="IMarkupExtension{T}"/> interface to provide a binding instance.</remarks>
[AcceptEmptyServiceProvider]
[ContentProperty(nameof(Converter))]
public class RightToLeftExtension : IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Gets or sets the value converter used to transform data between the source and target bindings.
	/// </summary>
	public IValueConverter? Converter { get; set; }

	/// <summary>
	/// Gets or sets an optional parameter to be passed to the converter during a binding operation.
	/// </summary>
	/// <remarks>The <see cref="ConverterParameter"/> is typically used to provide additional context or
	/// configuration to the converter. For example, it can be used to specify a format string or a threshold value that
	/// the converter uses during the conversion process.</remarks>
	public object? ConverterParameter { get; set; }

	/// <summary>
	/// Provides a binding object that can be used in XAML to bind data to a target property.
	/// </summary>
	/// <remarks>This method is typically used in XAML scenarios to create and return a binding object. The returned
	/// binding is configured to support right-to-left data flow.</remarks>
	/// <param name="serviceProvider">An object that provides services for the markup extension. This parameter is typically used to resolve services
	/// such as the target object and property where the binding is applied.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the binding to be applied.</returns>
	public BindingBase ProvideValue(IServiceProvider serviceProvider)
		=> RightToLeftBinding.Create(Converter, ConverterParameter);

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> ProvideValue(serviceProvider);
}
