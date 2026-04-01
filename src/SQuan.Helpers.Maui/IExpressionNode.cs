// IExpressionNode.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Defines the contract for a node within an expression graph.
/// An <see cref="IExpressionNode"/> represents a value or computed expression,
/// tracks its dependencies, exposes its parsed tokens, and participates in
/// evaluation and propagation within an <see cref="ExpressionManager"/>.
/// </summary>
public interface IExpressionNode
{
	/// <summary>
	/// Gets the unique reference string identifying this node within the expression graph.
	/// </summary>
	string NodeRef { get; }

	/// <summary>
	/// Gets or sets the current lifecycle state of the node's value, such as
	/// uninitialized, user‑provided, or computed.
	/// </summary>
	ExpressionValueKind ValueKind { get; set; }

	/// <summary>
	/// Gets the expected type of the node's value, if constrained.
	/// A null value indicates that the node accepts any type.
	/// </summary>
	Type? ValueType { get; }

	/// <summary>
	/// Gets the expression text associated with this node.
	/// This expression is parsed into <see cref="Tokens"/> for evaluation.
	/// </summary>
	string Expression { get; }

	/// <summary>
	/// Gets or sets a value indicating whether evaluating this node produces deterministic
	/// results. A node becomes non‑deterministic if its expression contains non‑deterministic functions.
	/// </summary>
	bool IsDeterministic { get; set; }

	/// <summary>
	/// Gets the parsed tokens representing the node's expression in evaluation order.
	/// </summary>
	List<ExpressionToken> Tokens { get; }

	/// <summary>
	/// Gets the default expression used when no explicit value or expression is provided.
	/// </summary>
	string DefaultExpression { get; }

	/// <summary>
	/// Gets the parsed tokens representing the node's default expression.
	/// </summary>
	List<ExpressionToken> DefaultExpressionTokens { get; }

	/// <summary>
	/// Sets the parsed tokens representing the node's expression in evaluation order.
	/// This also updates dependency tracking and determinism flags.
	/// </summary>
	/// <param name="tokens">The tokens to assign to the node.</param>
	void SetTokens(IEnumerable<ExpressionToken> tokens);

	/// <summary>
	/// Sets the parsed tokens representing the node's default expression in evaluation order.
	/// This also updates determinism flags for the default expression.
	/// </summary>
	/// <param name="defaultTokens">The default expression tokens to assign.</param>
	void SetDefaultExpressionTokens(IEnumerable<ExpressionToken> defaultTokens);

	/// <summary>
	/// Clears the node's value, expression state, and dependency information,
	/// returning it to an uninitialized state.
	/// </summary>
	void Clear();

	/// <summary>
	/// Retrieves the node's internally stored value without triggering evaluation
	/// or dependency propagation.
	/// </summary>
	/// <returns>The internal value, or null if no value is assigned.</returns>
	object? GetInternalValue();

	/// <summary>
	/// Attempts to assign an internal value to the node without triggering
	/// dependency propagation.
	/// Returns <c>true</c> if the value changed; otherwise <c>false</c>.
	/// </summary>
	/// <param name="value">The value to assign.</param>
	bool SetInternalValue(object? value);

	/// <summary>
	/// Gets the list of node references that this node depends on as inputs.
	/// </summary>
	/// <returns>A list of input node reference strings.</returns>
	List<string> GetInputNodeRefs();

	/// <summary>
	/// Gets the list of node references that depend on this node as outputs.
	/// </summary>
	/// <returns>A list of output node reference strings.</returns>
	List<string> GetOutputNodeRefs();

	/// <summary>
	/// Attempts to register another node as depending on this node.
	/// </summary>
	/// <param name="nodeRef">The reference of the dependent node.</param>
	/// <returns><c>true</c> if the reference was added; otherwise <c>false</c>.</returns>
	bool TryAddOutputNodeRef(string nodeRef);

	/// <summary>
	/// Notifies the system that the node's externally visible value has changed,
	/// allowing UI bindings or dependent nodes to react.
	/// </summary>
	void OnValueChanged();
}
