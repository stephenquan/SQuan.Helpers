// ExpressionToken.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a single token produced by <see cref="ExpressionParser"/> and
/// consumed by the expression evaluation engine.
/// </summary>
/// <param name="TokenType">The kind of token (constant, node reference, operator, or function).</param>
/// <param name="Text">The textual representation of the token as it appears in the expression.</param>
/// <param name="Value">The literal value associated with the token, if applicable.</param>
/// <param name="FunctionInfo">The function metadata associated with operator or function tokens.</param>
/// <param name="FunctionArity">The number of arguments consumed by the function or operator.</param>
public record ExpressionToken(
	ExpressionTokenType TokenType,
	string Text,
	object? Value = null,
	ExpressionFunctionInfo? FunctionInfo = null,
	int FunctionArity = 0);
