namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a binding that determines the flow direction based on the current UI culture's text direction.
/// </summary>
public static class FlowDirectionBinding
{
	/// <summary>
	/// Creates a binding that determines the flow direction based on the current UI culture's text direction.
	/// </summary>
	/// <remarks>This method is intended to simplify the creation of bindings for UI elements that need to adjust
	/// their flow direction dynamically based on the current localization settings.</remarks>
	/// <returns>A <see cref="BindingBase"/> instance configured to bind the flow direction to the right-to-left setting of the
	/// current UI culture. The binding uses one-way mode and applies a <see cref="RightToLeftToFlowDirectionConverter"/>.</returns>
	public static BindingBase Create()
		=> RightToLeftBinding.Create(converter: new RightToLeftToFlowDirectionConverter());
}
