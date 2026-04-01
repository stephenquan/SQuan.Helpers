// ExpressionParser.cs

using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Parses expression text into a sequence of <see cref="ExpressionToken"/> instances
/// suitable for evaluation by the expression engine.
/// </summary>
public partial class ExpressionParser
{
	/// <summary>
	/// The tokens produced by the most recent successful parse.
	/// </summary>
	public readonly List<ExpressionToken> Tokens = new();

	/// <summary>
	/// Indicates whether the most recent parse completed successfully.
	/// </summary>
	public bool IsValid { get; private set; } = false;

	/// <summary>
	/// Indicates whether the parsed expression is deterministic.
	/// </summary>
	public bool IsDeterministic { get; private set; } = true;

	ExpressionParserPlugin plugin;
	string key = string.Empty;
	string expression = string.Empty;
	int index = 0;
	Match? match;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExpressionParser"/> class.
	/// </summary>
	/// <param name="plugin">The parser plugin providing syntax rules, operators, and function definitions.</param>
	public ExpressionParser(ExpressionParserPlugin plugin)
	{
		this.plugin = plugin;
	}
	#region TryParse(string key, string expression)

	/// <summary>
	/// Attempts to parse an expression into evaluation tokens.
	/// </summary>
	/// <param name="key">A logical identifier for the expression. Reserved for future context.</param>
	/// <param name="expression">The expression text to parse.</param>
	/// <returns>True if parsing succeeded and the entire expression was consumed; otherwise false.</returns>
	public bool TryParse(string key, string expression)
	{
		this.key = key;
		this.expression = expression;
		this.index = 0;
		this.IsDeterministic = true;
		this.Tokens.Clear();

		IsValid = TryParseExpr();
		SkipWhitespace();
		IsValid &= (index == expression.Length);
		return IsValid;
	}
	#endregion

	#region SkipWhitespace()
	/// <summary>
	/// Advances the parser past any contiguous whitespace characters.
	/// </summary>
	void SkipWhitespace()
	{
		while (index < expression.Length && char.IsWhiteSpace(expression[index]))
		{
			index++;
		}
	}
	#endregion

	#region TryParseNegatablePrimary()
	/// <summary>
	/// Parses a primary expression with optional unary negation.
	/// </summary>
	/// <returns>True if a valid primary expression was parsed; otherwise false.</returns>
	bool TryParseNegatablePrimary()
	{
		int _index = index;
		bool negated = false;
		while (TryParseMatch(plugin.NegateRegex))
		{
			negated = !negated;
		}
		if (!TryParsePrimary())
		{
			index = _index;
			return false;
		}
		if (negated)
		{
			string negateOp = "negate";
			if (!plugin.Functions.TryGetValue(negateOp, out var functionInfo)
				|| functionInfo is null
				|| !functionInfo.AritySpec.IsOne)
			{
				index = _index;
				return false;
			}
			if (functionInfo.IsDeterministic
				&& Tokens.Count >= 1
				&& Tokens.Last() is ExpressionToken lastToken
				&& lastToken.TokenType == ExpressionTokenType.Constant)
			{
				try
				{
					var value = functionInfo.Function(new object?[] { lastToken.Value });
					Tokens.RemoveAt(Tokens.Count - 1);
					Tokens.Add(new ExpressionToken(ExpressionTokenType.Constant, string.Empty, value));
					return true;
				}
				catch (Exception ex)
				{
					ExpressionManager.Logger?.LogError(
						ex,
						"Error evaluating deterministic function {Function} with argument {Argument}",
						functionInfo,
						lastToken.Value);
					index = _index;
					return false;
				}
			}
			Tokens.Add(new ExpressionToken(ExpressionTokenType.Operator, negateOp, null, functionInfo, 1));
			return true;
		}
		return true;
	}
	#endregion

	#region TryParsePrimary()

	/// <summary>
	/// Parses a primary expression.
	/// </summary>
	/// <returns>True if a primary expression was parsed; otherwise false.</returns>
	bool TryParsePrimary()
	{
		SkipWhitespace();
		if (TryParseConstant())
		{
			return true;
		}
		if (TryParseMatch(plugin.NodeAbsoluteRegex) && match is not null)
		{
			Tokens.Add(new ExpressionToken(ExpressionTokenType.Node, match.Groups[1].Value));
			IsDeterministic = false;
			return true;
		}
		if (TryParseIdentifierOrFunction())
		{
			return true;
		}
		if (TryParseParenExpr())
		{
			return true;
		}
		return false;
	}
	#endregion

	/// <summary>
	/// Attempts to parse a numeric constant.
	/// </summary>
	/// <returns>
	/// True if a constant was parsed; otherwise false.
	/// </returns>
	bool TryParseConstant()
	{
		int _index = index;
		if (TryParseMatch(plugin.NumberRegex) && match is not null)
		{
			string text = match.Groups[1].Value;
			if (double.TryParse(text, CultureInfo.InvariantCulture, out var number))
			{
				Tokens.Add(new ExpressionToken(ExpressionTokenType.Constant, text, number));
				return true;
			}
			index = _index;
			return false;
		}
		if (TryParseString())
		{
			return true;
		}
		index = _index;
		return false;
	}

	#region TryParseString()
	/// <summary>
	/// Attempts to parse a quoted string literal.
	/// </summary>
	/// <returns>
	/// True if a valid quoted string was parsed; otherwise false.
	/// </returns>
	bool TryParseString()
	{
		int _index = index;
		if (TryParseQuotedString(plugin.SingleQuotedRegex, plugin.SingleQuotedBodyRegex))
		{
			return true;
		}
		if (TryParseQuotedString(plugin.DoubleQuotedRegex, plugin.DoubleQuotedBodyRegex))
		{
			return true;
		}
		index = _index;
		return false;
	}
	#endregion

	#region TryParseQuotedString(Regex QuoteRegex, Regex BodyRegex)

	/// <summary>
	/// Attempts to parse a quoted string using the specified quote and body patterns.
	/// </summary>
	/// <param name="QuoteRegex">The regular expression matching the opening and closing quote character.</param>
	/// <param name="BodyRegex">The regular expression matching the string body.</param>
	/// <returns>True if a valid quoted string was parsed; otherwise false.</returns>
	bool TryParseQuotedString(Regex QuoteRegex, Regex BodyRegex)
	{
		SkipWhitespace();

		int _index = index;
		if (!TryParseMatch(QuoteRegex))
		{
			return false;
		}

		int __index = index;
		if (TryParseMatch(plugin.NodeAbsoluteRegex) && match is not null)
		{
			string nodeName = match.Groups[1].Value;
			if (TryParseMatch(QuoteRegex))
			{
				SkipWhitespace();
				Tokens.Add(new(ExpressionTokenType.Node, nodeName));
				IsDeterministic = false;
				return true;
			}
			index = __index;
		}

		if (TryParseMatch(BodyRegex) && match is not null)
		{
			string str = match.Groups[1].Value;
			if (TryParseMatch(QuoteRegex))
			{
				SkipWhitespace();
				Tokens.Add(new(ExpressionTokenType.Constant, str, str));
				return true;
			}
		}

		index = _index;
		return false;
	}
	#endregion

	#region TryParseIdentifierOrFunction()
	/// <summary>
	/// Attempts to parse an identifier or function invocation.
	/// </summary>
	/// <returns>True if a valid identifier or function call was parsed; otherwise false.</returns>
	bool TryParseIdentifierOrFunction()
	{
		int _index = index;
		if (TryParseMatch(plugin.IdentifierRegex))
		{
			ArgumentNullException.ThrowIfNull(match);
			string identifier = match.Groups[1].Value;
			if (!TryParseMatch(plugin.FunctionStartRegex))
			{
				index = _index;
				return false;
			}
			if (TryParseMatch(plugin.FunctionEndRegex))
			{
				if (plugin.Functions.TryGetValue(identifier, out var _functionInfo) && _functionInfo is not null && _functionInfo.AritySpec.IsZero)
				{
					if (!_functionInfo.IsDeterministic)
					{
						IsDeterministic = false;
					}
					if (_functionInfo.IsDeterministic)
					{
						try
						{
							// For deterministic functions with zero arguments, we can evaluate them at parse time and treat them as constants.
							object? value = _functionInfo.Function(Array.Empty<object?>());
							Tokens.Add(new ExpressionToken(ExpressionTokenType.Constant, identifier, value, _functionInfo, 0));
							return true;
						}
						catch (Exception ex)
						{
							ExpressionManager.Logger?.LogError(
								ex,
								"Error evaluating deterministic function {Function} with zero arguments",
								_functionInfo);
							index = _index;
							return false;
						}
					}
					Tokens.Add(new ExpressionToken(ExpressionTokenType.Function, identifier, null, _functionInfo, 0));
					return true;
				}
				index = _index;
				return false;
			}
			if (!TryParseExpr())
			{
				index = _index;
				return false;
			}
			int arity = 1;
			while (TryParseMatch(plugin.FunctionCommaRegex))
			{
				if (!TryParseExpr())
				{
					index = _index;
					return false;
				}
				arity++;
			}
			if (!TryParseMatch(plugin.FunctionEndRegex))
			{
				index = _index;
				return false;
			}
			if (plugin.Functions.TryGetValue(identifier, out var functionInfo) && functionInfo is not null && (functionInfo.AritySpec.Accepts(arity)))
			{
				if (!functionInfo.IsDeterministic)
				{
					IsDeterministic = false;
				}
				if (functionInfo.IsDeterministic
					&& Tokens.Count >= arity
					&& Tokens.Skip(Tokens.Count - arity).Take(arity).ToList() is List<ExpressionToken> lastArgs
					&& lastArgs.All(t => t.TokenType == ExpressionTokenType.Constant))
				{
					try
					{
						object? value = functionInfo.Function(lastArgs.Select(t => t.Value).ToArray());
						Tokens.RemoveRange(Tokens.Count - arity, arity);
						Tokens.Add(new ExpressionToken(ExpressionTokenType.Constant, string.Empty, value));
						return true;
					}
					catch (Exception ex)
					{
						ExpressionManager.Logger?.LogError(
							ex,
							"Error evaluating deterministic function {Function} with arguments {Arguments}",
							functionInfo,
							lastArgs.Select(t => t.Value).ToArray());
						index = _index;
						return false;
					}
				}
				Tokens.Add(new ExpressionToken(ExpressionTokenType.Function, identifier, null, functionInfo, arity));
				return true;
			}
			index = _index;
			return false;
		}
		index = _index;
		return false;
	}
	#endregion

	#region TryParseParenExpr()
	/// <summary>
	/// Attempts to parse a parenthesized expression.
	/// </summary>
	/// <returns>True if a valid parenthesized expression was parsed; otherwise false.</returns>
	bool TryParseParenExpr()
	{
		int _index = index;
		if (TryParseMatch(plugin.ParenStartRegex))
		{
			if (!TryParseExpr())
			{
				index = _index;
				return false;
			}
			if (!TryParseMatch(plugin.ParenEndRegex))
			{
				index = _index;
				return false;
			}
			return true;
		}
		index = _index;
		return false;
	}
	#endregion

	#region TryParseExpr()
	/// <summary>
	/// Parses a full expression.
	/// </summary>
	/// <returns>True if a parsing was successful; otherwise false.</returns>
	bool TryParseExpr() => TryParseLogicalOr();
	#endregion

	#region TryParseLogicalOr()

	/// <summary>
	/// Parses logical OR expressions.
	/// </summary>
	/// <returns>True if a parsing was successful; otherwise false.</returns>
	bool TryParseLogicalOr() => TryParseBinaryOperation(plugin.LogicalOrRegex, TryParseLogicalAnd);
	#endregion

	#region TryParseLogicalAnd()
	/// <summary>
	/// Parses logical AND expressions.
	/// </summary>
	/// <returns>True if a parsing was successful; otherwise false.</returns>
	bool TryParseLogicalAnd() => TryParseBinaryOperation(plugin.LogicalAndRegex, TryParseEquality);
	#endregion

	#region TryParseEquality()
	/// <summary>
	/// Parses equality expressions.
	/// </summary>
	/// <returns>True if a parsing was successful; otherwise false.</returns>
	bool TryParseEquality() => TryParseBinaryOperation(plugin.EqualityOperatorsRegex, TryParseComparison);
	#endregion

	#region TryParseComparison()
	/// <summary>
	/// Parses comparison expressions.
	/// </summary>
	/// <returns>True if a parsing was successful; otherwise false.</returns>
	bool TryParseComparison() => TryParseBinaryOperation(plugin.ComparisonOperatorsRegex, TryParseSum);
	#endregion

	#region TryParseSum()
	/// <summary>
	/// Parses additive expressions.
	/// </summary>
	/// <returns>True if a parsing was successful; otherwise false.</returns>
	bool TryParseSum() => TryParseBinaryOperation(plugin.SumRegex, TryParseProduct);
	#endregion

	#region TryParseProduct()
	/// <summary>
	/// Parses multiplicative expressions.
	/// </summary>
	/// <returns>True if a parsing was successful; otherwise false.</returns>
	bool TryParseProduct() => TryParseBinaryOperation(plugin.ProductRegex, TryParseNegatablePrimary);
	#endregion

	#region TryParseMatch(Regex regex)
	/// <summary>
	/// Attempts to match a regular expression at the current parse position.
	/// </summary>
	/// <param name="regex">The regular expression to match.</param>
	/// <returns>True if the regex matches at the current position; otherwise false.</returns>
	bool TryParseMatch(Regex regex)
	{
		match = regex.Match(expression, index);
		if (!match.Success || match.Index != index)
		{
			return false;
		}
		index += match.Length;
		return true;
	}
	#endregion

	#region TryParseBinaryOperation(Regex operatorRegex, Func<bool> parseNext)

	/// <summary>
	/// Parses a left-associative binary operation at a given precedence level.
	/// </summary>
	/// <param name="operatorRegex">The regex identifying valid operators at this precedence.</param>
	/// <param name="parseNext">A delegate that parses the next higher-precedence expression.</param>
	/// <returns>True if parsing succeeded; otherwise false.</returns>
	bool TryParseBinaryOperation(Regex operatorRegex, Func<bool> parseNext)
	{
		if (!parseNext())
		{
			return false;
		}
		while (true)
		{
			int _index = index;
			if (TryParseMatch(operatorRegex) && match is not null)
			{
				string op = match.Groups[1].Value;
				if (!parseNext())
				{
					index = _index;
					return false;
				}
				if (!plugin.Functions.TryGetValue(op, out var functionInfo) || functionInfo is null)
				{
					return false;
				}
				if (functionInfo.IsDeterministic
					&& Tokens.Count >= 2
					&& Tokens[Tokens.Count - 1].TokenType == ExpressionTokenType.Constant
					&& Tokens[Tokens.Count - 2].TokenType == ExpressionTokenType.Constant)
				{
					object?[] args = new object?[2];
					args[0] = Tokens[Tokens.Count - 2].Value;
					args[1] = Tokens[Tokens.Count - 1].Value;
					try
					{
						object? value = functionInfo.Function(args);
						Tokens.RemoveRange(Tokens.Count - 2, 2);
						Tokens.Add(new ExpressionToken(ExpressionTokenType.Constant, string.Empty, value));
						continue;
					}
					catch (Exception ex)
					{
						ExpressionManager.Logger?.LogError(
							ex,
							"Error evaluating deterministic function {Function} with arguments {Arguments}",
							functionInfo,
							args);
						index = _index;
						return false;
					}
				}
				Tokens.Add(new ExpressionToken(ExpressionTokenType.Operator, op, null, functionInfo, 2));
				continue;
			}
			break;
		}
		return true;
	}
	#endregion
}
