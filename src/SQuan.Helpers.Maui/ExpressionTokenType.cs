// ExpressionTokenType.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Identifies the kind of token produced by the expression parser.
/// </summary>
public enum ExpressionTokenType
{
	/// <summary>
	/// A reference to another expression node.
	/// </summary>
	Node,

	/// <summary>
	/// A literal constant value.
	/// </summary>
	Constant,

	/// <summary>
	/// An operator applied to one or more operands.
	/// </summary>
	Operator,

	/// <summary>
	/// A function invocation.
	/// </summary>
	Function,
}
