// InputMaskBehavior.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides behavior for input views that enforces input masking based on a regular expression or a set of allowed keys.
/// </summary>
[Obsolete("InputMaskBehavior is deprecated. Please use attached properties InputExtras.InputMask and InputExtras.InputPattern and set to the desired value instead.")]
public partial class InputMaskBehavior : PlatformBehavior<InputView>
{
}
