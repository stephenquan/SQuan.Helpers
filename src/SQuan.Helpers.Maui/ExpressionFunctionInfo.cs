// ExpressionFunctionInfo.cs

namespace SQuan.Helpers.Maui;


/// <summary>
/// Describes an expression function, including its implementation, arity, and determinism.
/// </summary>
/// <param name="Function">The function implementation, invoked with the evaluated argument values.</param>
/// <param name="AritySpec">Defines the supported argument arity (fixed, ranged, or unbounded).</param>
/// <param name="IsDeterministic">Indicates whether the function is deterministic,
/// meaning it always produces the same result for the same input arguments.</param>
public record ExpressionFunctionInfo(
	Func<object?[], object?> Function,
	AritySpec AritySpec,
	bool IsDeterministic = true);
