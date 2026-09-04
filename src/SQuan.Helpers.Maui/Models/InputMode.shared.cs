// InputMode.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Defines how text input is constrained when using the InputExtras.InputMode attached property.
/// </summary>
public enum InputMode
{
	/// <summary>
	/// No input mode is applied.
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

	/// <summary>
	/// Only input matching the specified pattern is allowed.
	/// </summary>
	Pattern,
}
