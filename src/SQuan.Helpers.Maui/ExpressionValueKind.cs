// ExpressionValueKind.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents the lifecycle state of an <see cref="IExpressionNode"/>.
/// </summary>
public enum ExpressionValueKind
{
	/// <summary>
	/// The node has not yet been initialized or assigned a value or expression.
	/// </summary>
	Uninitialized,

	/// <summary>
	/// The node has been initialized with an expression, but the expression has not yet been parsed to produce an expression tree.
	/// </summary>
	PendingParsing,

	/// <summary>
	/// The node is in the process of being reset to the default expression.
	/// </summary>
	PendingReset,

	/// <summary>
	/// The node has been reset to the default expression.
	/// </summary>
	Default,

	/// <summary>
	/// The node represents a value restored from a persistent store, such as the inbox, drafts, or outbox folders.
	/// </summary>
	Folder,

	/// <summary>
	/// The node represents a value provided by the user.
	/// </summary>
	UserInput,

	/// <summary>
	/// The node has been initialized with an expression, but the expression has not yet been evaluated to produce a value.
	/// </summary>
	PendingCalculation,

	/// <summary>
	/// The node's expression has been successfully evaluated, and the node's value is up to date with respect to its dependencies.
	/// </summary>
	Calculated,

	/// <summary>
	/// The node's expression failed to parse, and therefore the node cannot be evaluated.
	/// </summary>
	ParseError,

	/// <summary>
	/// The node's expression was parsed successfully, but an error occurred during evaluation (e.g. due to invalid input values or a runtime exception in a function).
	/// </summary>
	CalculateError,

	/// <summary>
	/// The node's expression was parsed successfully, but the node is currently in an error state and cannot be evaluated (e.g. due to a dependency on another node that is in an error state).
	/// </summary>
	ResetError,
}
