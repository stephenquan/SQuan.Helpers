namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality to create a binding that dynamically determines a rotation value on the Y-axis based on the
/// right-to-left (RTL) layout setting of the user interface.
/// </summary>
/// <remarks>This class is useful for scenarios where an element, such as an image, needs to be flipped
/// horizontally depending on the layout direction. The binding evaluates to <see langword="180.0"/> for right-to-left
/// (RTL) layouts and <see langword="0.0"/> for left-to-right (LTR) layouts.</remarks>
public static class RotationYBinding
{
	/// <summary>
	/// Creates a new instance of a binding that provides a value based on the right-to-left (RTL) layout setting.
	/// </summary>
	/// <remarks>This method is useful for scenarios where an image needs to dynamically flipped on the Y-axis based
	/// on the current layout direction of the user interface. It evaluates to <see langword="180.0"/> when the layout is right-to-left
	/// direction of the user interface.</remarks>
	/// <returns>A <see cref="BindingBase"/> instance that evaluates to <see langword="180.0"/> when the layout is right-to-left
	/// (RTL), or <see langword="0.0"/> when the layout is left-to-right (LTR).</returns>
	public static BindingBase Create()
		=> RightToLeftBinding.Create(converter: new RightToLeftToRotationYConverter());
}
