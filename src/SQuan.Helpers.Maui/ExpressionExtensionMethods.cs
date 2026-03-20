// ExpressionExtensionMethods.cs

using System.Globalization;
using Microsoft.Extensions.Logging;

namespace SQuan.Helpers.Maui;


/// <summary>
/// Helper extension methods used by the expression engine for:
/// type coercion of evaluated values, adapting strongly-typed delegates to an object[] call-site,
/// and initializing/evaluating <see cref="ExpressionNode"/> instances.
/// </summary>
public static class ExpressionExtensionMethods
{

	/// <summary>
	/// Attempts to convert <paramref name="value"/> to <typeparamref name="T"/> using the expression engine's supported coercions.
	/// </summary>
	/// <typeparam name="T">The target type to coerce to.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="coerceValue">When this method returns, contains the converted value if the conversion succeeded;
	/// otherwise, the default value of <typeparamref name="T"/>.</param>
	/// <returns><see langword="true"/> if the value could be converted to <typeparamref name="T"/>; otherwise <see langword="false"/>.</returns>
	public static bool TryConvert<T>(this object? value, out T coerceValue)
	{
		if (TryConvert(value, typeof(T), out var _coerceValue) && _coerceValue is T coerce)
		{
			coerceValue = coerce;
			return true;
		}
		coerceValue = default!;
		return false;
	}

	/// <summary>
	/// Attempts to convert <paramref name="value"/> to <paramref name="valueType"/> using a small set of supported primitive conversions.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="valueType">The requested target type. If <see langword="null"/>, no conversion is performed.</param>
	/// <param name="coerceValue">When this method returns, contains the converted value if the conversion succeeded;
	/// otherwise, contains the original <paramref name="value"/>.</param>
	public static bool TryConvert(this object? value, Type? valueType, out object? coerceValue)
	{
		coerceValue = value;
		if (value is null || valueType is null)
		{
			return true;
		}
		if (value is string str
			&& string.IsNullOrEmpty(str)
			&& (valueType is null || valueType != typeof(string)))
		{
			coerceValue = null;
			return true;
		}
		try
		{
			if (valueType == typeof(bool))
			{
				coerceValue = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
				return true;
			}
			if (valueType == typeof(double))
			{
				coerceValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
				return true;
			}
			if (valueType == typeof(long))
			{
				coerceValue = Convert.ToInt64(value, CultureInfo.InvariantCulture);
				return true;
			}
			if (valueType == typeof(int))
			{
				coerceValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
				return true;
			}
			if (valueType == typeof(string))
			{
				coerceValue = Convert.ToString(value, CultureInfo.InvariantCulture);
				return true;
			}
			if (valueType == typeof(object))
			{
				return true;
			}
			return false;
		}
		catch
		{
			ExpressionManager.Logger?.LogWarning("Failed to convert value '{Value}' of type {ValueType} to {TargetType}", value, value.GetType(), valueType);
			return false;
		}
	}

	/// <summary>
	/// Invokes a strongly-typed single-parameter function using values supplied from an <see cref="object"/> array.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter.</typeparam>
	/// <typeparam name="TReturn">The return type.</typeparam>
	/// <param name="args">The argument array.</param>
	/// <param name="func">The function to invoke.</param>
	/// <returns>The function result if <paramref name="args"/> contains at least one value
	/// and it can be converted to <typeparamref name="T1"/>; otherwise, <see langword="default"/>.
	/// </returns>
	public static TReturn? WrapFunc<T1, TReturn>(this object?[] args, Func<T1, TReturn> func)
		=> (args.Length >= 1
			&& args[0].TryConvert<T1>(out var p1))
		? func(p1)
		: default;

	/// <summary>
	/// Invokes a strongly-typed two-parameter function using values supplied from an <see cref="object"/> array.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter.</typeparam>
	/// <typeparam name="T2">The type of the second parameter.</typeparam>
	/// <typeparam name="TReturn">The return type.</typeparam>
	/// <param name="args">The argument array.</param>
	/// <param name="func">The function to invoke.</param>
	/// <returns>
	/// The function result if <paramref name="args"/> contains at least two values
	/// and both can be converted to <typeparamref name="T1"/> and <typeparamref name="T2"/>;
	/// otherwise, <see langword="default"/>.
	/// </returns>
	public static TReturn? WrapFunc<T1, T2, TReturn>(this object?[] args, Func<T1, T2, TReturn> func)
		=> (args.Length >= 2
			&& args[0].TryConvert<T1>(out var p1)
			&& args[1].TryConvert<T2>(out var p2))
		? func(p1, p2)
		: default;

	/// <summary>
	/// Invokes a strongly-typed three-parameter function using values supplied from an <see cref="object"/> array.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter.</typeparam>
	/// <typeparam name="T2">The type of the second parameter.</typeparam>
	/// <typeparam name="T3">The type of the third parameter.</typeparam>
	/// <typeparam name="TReturn">The return type.</typeparam>
	/// <param name="args">The argument array.</param>
	/// <param name="func">The function to invoke.</param>
	/// <returns>
	/// The function result if <paramref name="args"/> contains at least three values
	/// and all can be converted to the corresponding parameter types;
	/// otherwise, <see langword="default"/>.
	/// </returns>
	public static TReturn? WrapFunc<T1, T2, T3, TReturn>(this object?[] args, Func<T1, T2, T3, TReturn> func)
		=> (args.Length >= 3
			&& args[0].TryConvert<T1>(out var p1)
			&& args[1].TryConvert<T2>(out var p2)
			&& args[2].TryConvert<T3>(out var p3))
		? func(p1, p2, p3)
		: default;

	/// <summary>
	/// Parses the node's expression and initializes its token list and input node references.
	/// </summary>
	/// <param name="node">The node to initialize.</param>
	/// <param name="plugin">The parser plugin providing operators and functions.</param>
	/// <returns>
	/// <see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.
	/// </returns>
	public static bool Initialize(this ExpressionNode node, ExpressionParserPlugin plugin)
	{
		ExpressionParser parser = new(plugin);

		if (!parser.TryParse(node.NodeRef, node.Expression))
		{
			node.ValueKind = ExpressionValueKind.ParseError;
			return false;
		}

		node.Tokens.Clear();

		foreach (var token in parser.Tokens)
		{
			node.Tokens.Add(token);
			if (token.TokenType == ExpressionTokenType.Node)
			{
				node.InputNodeRefs.TryAdd(token.Text, 0);
			}
		}

		node.ValueKind = ExpressionValueKind.PendingCalculation;
		return true;
	}

	/// <summary>
	/// Evaluates the node's tokenized expression using reverse-polish notation (RPN) and updates the node's value.
	/// </summary>
	/// <param name="node">The node to calculate.</param>
	/// <param name="getValue">Callback used to resolve referenced node values by node reference.</param>
	/// <param name="ct">A token used to cancel the evaluation.</param>
	/// <returns>
	/// <see langword="true"/> if the node's internal value changed; otherwise <see langword="false"/>.
	/// </returns>
	public static bool Calculate(this ExpressionNode node, Func<string, object?> getValue, CancellationToken ct)
	{
		switch (node.ValueKind)
		{
			case ExpressionValueKind.Default:
			case ExpressionValueKind.Folder:
			case ExpressionValueKind.UserInput:
			case ExpressionValueKind.Calculated:
			case ExpressionValueKind.PendingCalculation:
				node.ValueKind = ExpressionValueKind.PendingCalculation;
				break;
			case ExpressionValueKind.Uninitialized:
			case ExpressionValueKind.ParseError:
			case ExpressionValueKind.CalculateError:
			default:
				return false;
		}

		bool isDeterministic = true;
		Stack<object?> rpn = [];
		for (int i = 0; !ct.IsCancellationRequested && node.ValueKind == ExpressionValueKind.PendingCalculation && i < node.Tokens.Count; i++)
		{
			var token = node.Tokens[i];
			switch (token.TokenType)
			{
				case ExpressionTokenType.Constant:
					rpn.Push(token.Value);
					break;
				case ExpressionTokenType.Node:
					rpn.Push(getValue(token.Text));
					break;
				case ExpressionTokenType.Operator:
				case ExpressionTokenType.Function:
					ArgumentNullException.ThrowIfNull(token.FunctionInfo);
					if (rpn.Count < token.FunctionArity)
					{
						node.ValueKind = ExpressionValueKind.CalculateError;
						return false;
					}
					var args = new object?[token.FunctionArity];
					for (int j = token.FunctionArity - 1; j >= 0; j--)
					{
						args[j] = rpn.Pop();
					}
					try
					{
						rpn.Push(token.FunctionInfo.Function(args));
						if (token.FunctionInfo.IsDeterministic == false)
						{
							isDeterministic = false;
						}
					}
					catch (Exception ex)
					{
						node.ValueKind = ExpressionValueKind.CalculateError;
						ExpressionManager.Logger?.LogError(
							ex,
							"Error calculating function '{FunctionName}' with arguments {Arguments} for node {NodeRef}",
							token.Text,
							args,
							node.NodeRef);
						return false;
					}
					break;
				default:
					ExpressionManager.Logger?.LogError(
						"Unknown token type {TokenType} for token '{TokenText}'", token.TokenType, token.Text);
					node.ValueKind = ExpressionValueKind.CalculateError;
					return false;
			}
		}

		if (rpn.Count != 1)
		{
			ExpressionManager.Logger?.LogError(
				"RPN evaluation for node {NodeRef} resulted in {StackCount} items on the stack instead of 1", node.NodeRef, rpn.Count);
			node.ValueKind = ExpressionValueKind.CalculateError;
			return false;
		}

		object? value = rpn.Pop();

		if (value is null && node.InternalValue is null)
		{
			node.ValueKind = ExpressionValueKind.Calculated;
			node.IsDeterministic = isDeterministic;
			return false;
		}

		if (value is not null && node.InternalValue is not null && value.Equals(node.InternalValue))
		{
			node.ValueKind = ExpressionValueKind.Calculated;
			node.IsDeterministic = isDeterministic;
			return false;
		}

		node.SetInternalValue(value);
		node.ValueKind = ExpressionValueKind.Calculated;
		node.IsDeterministic = isDeterministic;
		ExpressionManager.Logger?.LogTrace("Calculated {NodeRef} to {Value} (type={ValueType}, valueKind={ValueKind}, isDeterministic={IsDeterministic})", node.NodeRef, value, node.ValueType, node.ValueKind, node.IsDeterministic);
		return true;
	}
}
