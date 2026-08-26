// InputMaskBehavior.shared.cs

using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides behavior for input views that enforces input masking based on a regular expression or a set of allowed keys.
/// </summary>
public partial class InputMaskBehavior : PlatformBehavior<InputView>
{
	/// <summary>
	/// Gets or sets the regular expression that defines the allowed input pattern.
	/// If specified, the behavior will prevent any input that does not match this regular expression.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial string Regex { get; set; }

	/// <summary>
	/// Gets or sets a string containing the allowed characters for input. This is used on platforms that support key filtering (e.g., Android).
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial string Keys { get; set; } = string.Empty;
}

