// InputViewExtras.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides attached properties and helper methods for customizing input views, such as setting border thickness and
/// input mask types, in MAUI applications.
/// </summary>
public partial class InputViewExtras
{
	#region BorderThickness Attached Property
	/// <summary>
	/// Attached property for BorderThickness.
	/// </summary>
	public static readonly BindableProperty BorderThicknessProperty = BindableProperty.CreateAttached("BorderThickness", typeof(int), typeof(InputViewExtras), 1, BindingMode.OneWay,
		coerceValue: (b, v) => InvokeActionOnHelper(b, v, (platform) => platform.BorderThickness = (int)v));

	/// <summary>
	/// Gets the thickness of the border around the input control.
	/// </summary>
	/// <param name="view">An Entry or Editor control.</param>
	/// <returns>The border thickness of the input control.</returns>
	public static int GetBorderThickness(BindableObject view) => (int)view.GetValue(BorderThicknessProperty);

	/// <summary>
	/// Sets the thickness of the border around the input control.
	/// </summary>
	/// <param name="view">An Entry or Editor control.</param>
	/// <param name="value">The new border thickness of the input control.</param>
	public static void SetBorderThickness(BindableObject view, int value) => view.SetValue(BorderThicknessProperty, value);
	#endregion

	#region MaskType Attached Property
	/// <summary>
	/// Attached property for MaskType.
	/// </summary>
	public static readonly BindableProperty MaskTypeProperty = BindableProperty.CreateAttached("MaskType", typeof(InputViewMaskKind), typeof(InputViewExtras), InputViewMaskKind.None, BindingMode.OneWay,
		coerceValue: (b, v) => InvokeActionOnHelper(b, v, (platform) => platform.MaskType = (InputViewMaskKind)v));

	/// <summary>
	/// Gets the type of input mask applied to the input control.
	/// </summary>
	/// <param name="view">An Entry or Editor control.</param>
	/// <returns>The type of input mask applied to the input control.</returns>
	public static InputViewMaskKind GetMaskType(BindableObject view) => (InputViewMaskKind)view.GetValue(MaskTypeProperty);

	/// <summary>
	/// Sets the type of input mask to be applied to the input control.
	/// </summary>
	/// <param name="view">An Entry or Editor control.</param>
	/// <param name="value">The new input mask type to be applied to the input control.</param>
	public static void SetMaskType(BindableObject view, InputViewMaskKind value) => view.SetValue(MaskTypeProperty, value);
	#endregion

	#region InputViewHelper Attached Property
	/// <summary>
	/// Attached property for InputViewHelper.
	/// </summary>
	public static readonly BindableProperty InputViewHelperProperty = BindableProperty.CreateAttached("InputViewHelper", typeof(InputViewHelper), typeof(InputViewExtras), null, BindingMode.OneWay);

	/// <summary>
	/// Gets the InputViewHelper associated with the specified view.
	/// </summary>
	/// <param name="view"></param>
	/// <returns></returns>
	public static InputViewHelper? GetInputViewHelper(BindableObject view) => (InputViewHelper?)view.GetValue(InputViewHelperProperty);

	/// <summary>
	/// Sets the InputViewHelper for the specified view.
	/// </summary>
	/// <param name="view"></param>
	/// <param name="value"></param>
	public static void SetInputViewHelper(BindableObject view, InputViewHelper? value) => view.SetValue(InputViewHelperProperty, value);
	#endregion

	#region InvokeActionOnHelper Method
	static object? InvokeActionOnHelper(BindableObject view, object? value, Action<InputViewHelper> action)
	{
		if (view is InputView inputView)
		{
			InputViewHelper? platform = GetInputViewHelper(view);
			if (platform is null)
			{
				platform = new InputViewHelper(inputView);
				SetInputViewHelper(view, platform);
			}
			action(platform);
		}
		return value;
	}
	#endregion
}
