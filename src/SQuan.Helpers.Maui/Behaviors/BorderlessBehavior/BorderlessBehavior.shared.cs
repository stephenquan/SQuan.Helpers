// BorderlessBehavior.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Applies a platform-specific behavior that removes the border from an input view control.
/// </summary>
[Obsolete("BorderlessBehavior is deprecated. Please use attached property InputExtras.BorderThickness and set to 0 instead.")]
public partial class BorderlessBehavior : Behavior<InputView>
{
	/// <inheritdoc/>
	protected override void OnAttachedTo(InputView bindable)
	{
		base.OnAttachedTo(bindable);
		InputExtras.SetBorderThickness(bindable, 0);
	}
}
