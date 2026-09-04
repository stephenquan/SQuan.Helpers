// InputMode.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Defines the input extras mask mode for the InputExtrasBehavior.
/// </summary>
public enum InputMode
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

	/// <summary>
	/// Only input matching the specified pattern is allowed.
	/// </summary>
	Pattern,
}
