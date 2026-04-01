// ExpressionExtensionMethods.cs

using System.Collections.Concurrent;
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
	public static bool TryConvert<T>(this object? value, out T? coerceValue)
	{
		if (value is null)
		{
			coerceValue = default;
			return false;
		}
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
	public static object? WrapFunc<T1, TReturn>(this object?[] args, Func<T1, TReturn> func)
		=> (args.Length >= 1
			&& args[0].TryConvert<T1>(out var p1)) && p1 is not null
		? func(p1)
		: null;

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
	public static object? WrapFunc<T1, T2, TReturn>(this object?[] args, Func<T1, T2, TReturn> func)
		=> (args.Length >= 2
			&& args[0].TryConvert<T1>(out var p1) && p1 is not null
			&& args[1].TryConvert<T2>(out var p2) && p2 is not null)
		? func(p1, p2)
		: null;

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
	public static object? WrapFunc<T1, T2, T3, TReturn>(this object?[] args, Func<T1, T2, T3, TReturn> func)
		=> (args.Length >= 3
			&& args[0].TryConvert<T1>(out var p1) && p1 is not null
			&& args[1].TryConvert<T2>(out var p2) && p2 is not null
			&& args[2].TryConvert<T3>(out var p3) && p3 is not null)
		? func(p1, p2, p3)
		: null;

	/// <summary>
	/// Attempts to parse the specified expression and populates the provided token list with the resulting tokens.
	/// </summary>
	/// <param name="expression">The string expression to parse. If null or empty, the method clears the token list and returns <see langword="true"/>.</param>
	/// <param name="nodeRef">A reference node that provides context for parsing the expression.</param>
	/// <param name="plugin">An instance of <see cref="ExpressionParserPlugin"/> used to customize the parsing behavior.</param>
	/// <param name="tokens">A list that will be cleared and then populated with the tokens generated from the parsed expression.</param>
	/// <param name="isDeterministic">When this method returns, indicates whether the expression is deterministic (i.e., does not contain any non-deterministic functions or operators).</param>
	/// <returns><see langword="true"/> if the expression is successfully parsed or is null or empty; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse(this string expression, string nodeRef, ExpressionParserPlugin plugin, List<ExpressionToken> tokens, out bool isDeterministic)
	{
		isDeterministic = true;

		if (string.IsNullOrEmpty(expression))
		{
			tokens.Clear();
			return true;
		}

		ExpressionParser parser = new(plugin);

		if (!parser.TryParse(nodeRef, expression))
		{
			isDeterministic = false;
			return false;
		}

		tokens.Clear();
		foreach (var token in parser.Tokens)
		{
			tokens.Add(token);
		}
		isDeterministic = parser.IsDeterministic;
		return true;
	}

	/// <summary>
	/// Attempts to evaluate a list of expression tokens and returns a value if the expression is valid.
	/// </summary>
	/// <param name="tokens">The list of expression tokens to evaluate. Tokens may represent constants, nodes, operators, or functions.</param>
	/// <param name="getNodeValue">A function that retrieves the value associated with a given token text. Used to resolve node tokens during evaluation.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the evaluation operation.</param>
	/// <param name="isDeterministic">When this method returns, indicates whether the expression is deterministic (i.e., does not contain any non-deterministic functions or operators).</param>
	/// <param name="value">When this method returns <see langword="true"/>, contains the evaluated result of the expression; otherwise, <see langword="null"/>.</param>
	/// <returns><see langword="true"/> if the expression was successfully parsed and evaluated; otherwise, <see langword="false"/>.</returns>
	public static bool TryEvaluate(this List<ExpressionToken> tokens, Func<string, object?> getNodeValue, CancellationToken ct, out bool isDeterministic, out object? value)
	{
		value = null;
		isDeterministic = true;
		Stack<object?> rpn = [];
		for (int i = 0; !ct.IsCancellationRequested && i < tokens.Count; i++)
		{
			var token = tokens[i];
			switch (token.TokenType)
			{
				case ExpressionTokenType.Constant:
					rpn.Push(token.Value);
					break;
				case ExpressionTokenType.Node:
					try
					{
						rpn.Push(getNodeValue(token.Text));
					}
					catch (Exception ex)
					{
						ExpressionManager.Logger?.LogError(ex, "Error retrieving value for node reference '{NodeRef}' during expression evaluation", token.Text);
						return false;
					}
					break;
				case ExpressionTokenType.Operator:
				case ExpressionTokenType.Function:
					ArgumentNullException.ThrowIfNull(token.FunctionInfo);
					if (token.FunctionInfo.IsDeterministic == false)
					{
						isDeterministic = false;
					}
					if (rpn.Count < token.FunctionArity)
					{
						return false;
					}
					var args = new object?[token.FunctionArity];
					for (int j = token.FunctionArity - 1; j >= 0; j--)
					{
						args[j] = rpn.Pop();
					}
					try
					{
						var result = token.FunctionInfo.Function(args);
						rpn.Push(result);
					}
					catch (Exception ex)
					{
						ExpressionManager.Logger?.LogError(
							ex,
							"Error calculating function '{FunctionName}' with arguments {Arguments}",
							token.Text,
							args);
						return false;
					}
					break;
				default:
					ExpressionManager.Logger?.LogError("Unknown token type {TokenType} for token '{TokenText}'", token.TokenType, token.Text);
					return false;
			}
		}

		if (rpn.Count != 1)
		{
			ExpressionManager.Logger?.LogError("RPN evaluation resulted in {StackCount} items on the stack instead of 1", rpn.Count);
			return false;
		}

		value = rpn.Pop();
		return true;
	}

	/// <summary>
	/// Clears all existing entries in the input node references dictionary and adds a new entry for each
	/// token of type <see cref="ExpressionTokenType.Node"/> found in the specified list of expression tokens.
	/// </summary>
	/// <param name="inputNodeRefs">A concurrent dictionary that stores references to input nodes.
	/// This dictionary will be cleared and repopulated based on the provided tokens.</param>
	/// <param name="tokens">A list of expression tokens to process.
	/// Only tokens with a TokenType of Node are added to the input node references dictionary.</param>
	public static void UpdateInputNodeRefs(this ConcurrentDictionary<string, byte> inputNodeRefs, List<ExpressionToken> tokens)
	{
		inputNodeRefs.Clear();
		foreach (var token in tokens)
		{
			if (token.TokenType == ExpressionTokenType.Node)
			{
				inputNodeRefs.TryAdd(token.Text, 0);
			}
		}
	}
}
