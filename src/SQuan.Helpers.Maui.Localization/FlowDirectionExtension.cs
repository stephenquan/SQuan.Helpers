namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a markup extension that creates a binding for determining the flow direction based on the current UI
/// culture's text direction.
/// </summary>
[AcceptEmptyServiceProvider]
public class FlowDirectionExtension : IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Provides a binding that determines the flow direction based on the current UI culture's text direction.
	/// </summary>
	/// <returns>A <see cref="BindingBase"/> instance configured to bind the flow direction to the right-to-left setting of the
	/// current UI culture. The binding uses one-way mode and applies a <see cref="RightToLeftToFlowDirectionConverter"/>.</returns>
	public BindingBase ProvideValue(IServiceProvider serviceProvider)
		=> FlowDirectionBinding.Create();
	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> ProvideValue(serviceProvider);
}
