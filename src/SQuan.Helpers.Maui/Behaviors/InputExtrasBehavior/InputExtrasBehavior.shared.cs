// InputExtrasBehavior.shared.cs

using System.Text.RegularExpressions;
using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// 
/// </summary>
public partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	/// <summary>
	/// Gets or sets the border thickness of the input view control.
	/// </summary>
	[BindableProperty(PropertyChangedMethodName = nameof(OnBorderThicknessChanged))]
	public partial double BorderThickness { get; set; } = 1.0;

	/// <summary>
	/// Gets or sets the input mask mode for the input view control.
	/// </summary>
	[BindableProperty(PropertyChangedMethodName = nameof(OnMaskModeChanged))]
	public partial InputMaskMode MaskMode { get; set; } = InputMaskMode.None;

	[GeneratedRegex("^[-]?\\d*$")]
	private static partial Regex IntegerRegex();

	[GeneratedRegex("^[-]?\\d*([.,]\\d*)?$")]
	private static partial Regex DecimalRegex();


	static void OnBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((InputExtrasBehavior)bindable).UpdateBorderThickness();

	static void OnMaskModeChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((InputExtrasBehavior)bindable).UpdateMaskMode();

	partial void UpdateBorderThickness();

	partial void UpdateMaskMode();
}


/// <summary>
/// Defines the input extras mask mode for the InputExtrasBehavior.
/// </summary>
public enum InputMaskMode
{
	/// <summary>
	/// No input mask is applied.
	/// </summary>
	None = 0,

	/// <summary>
	/// Only integer input is allowed.
	/// </summary>
	Integer,

	/// <summary>
	/// Only decimal input is allowed.
	/// </summary>
	Decimal,
}