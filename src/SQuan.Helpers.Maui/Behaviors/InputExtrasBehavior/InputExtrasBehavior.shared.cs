// InputExtrasBehavior.shared.cs

using System.Text.RegularExpressions;
using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a behavior that provides additional features for input view controls, such as border thickness and input masking.
/// </summary>
partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	/// <summary>
	/// Gets or sets the border thickness of the input view control.
	/// </summary>
	[BindableProperty(PropertyChangedMethodName = nameof(OnBorderThicknessChanged))]
	public partial double BorderThickness { get; set; } = 1.0;

	/// <summary>
	/// Gets or sets the input mask mode for the input view control.
	/// </summary>
	[BindableProperty(PropertyChangedMethodName = nameof(OnInputMaskChanged))]
	public partial InputMode InputMode { get; set; } = InputMode.None;

	/// <summary>
	/// Gets or sets the input pattern for the input view control.
	/// </summary>
	[BindableProperty]
	public partial string InputPattern { get; set; } = string.Empty;

	[GeneratedRegex("^[-]?\\d*$")]
	internal static partial Regex IntegerRegex();

	[GeneratedRegex("^[-]?\\d*([.,]\\d*)?$")]
	internal static partial Regex DecimalRegex();


	static void OnBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((InputExtrasBehavior)bindable).UpdateBorderThickness();

	static void OnInputMaskChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((InputExtrasBehavior)bindable).UpdateInputMask();

	partial void UpdateBorderThickness();

	partial void UpdateInputMask();
}
