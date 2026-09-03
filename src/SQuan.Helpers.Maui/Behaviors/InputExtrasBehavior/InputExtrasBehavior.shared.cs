// InputExtrasBehavior.shared.cs

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

	static void OnBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
	{
		((InputExtrasBehavior)bindable).UpdateBorderThickness();
	}

	partial void UpdateBorderThickness();
}
