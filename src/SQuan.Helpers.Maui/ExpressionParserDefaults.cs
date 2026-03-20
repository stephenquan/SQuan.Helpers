// ExpressionParserDefaults.cs

using System.Text.RegularExpressions;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides the default regular expressions used by <see cref="ExpressionParser"/> to tokenize and parse expressions.
/// </summary>
public static partial class ExpressionParserDefaults
{
	/// <summary>
	/// Matches the logical OR operator.
	/// </summary>
	[GeneratedRegex(@"\s*(or)\s*")]
	public static partial Regex LogicalOrOperatorRegex();

	/// <summary>
	/// Matches the logical AND operator.
	/// </summary>
	[GeneratedRegex(@"\s*(and)\s*")]
	public static partial Regex LogicalAndOperatorRegex();

	/// <summary>
	/// Matches equality comparison operators.
	/// </summary>
	[GeneratedRegex(@"\s*(=|!=)\s*")]
	public static partial Regex EqualityOperatorsRegex();

	/// <summary>
	/// Matches relational comparison operators.
	/// </summary>
	[GeneratedRegex(@"\s*(<=|<|>=|>)\s*")]
	public static partial Regex ComparisonOperatorsRegex();

	/// <summary>
	/// Matches additive operators.
	/// </summary>
	[GeneratedRegex(@"\s*(\+|\-)\s*")]
	public static partial Regex SumRegex();

	/// <summary>
	/// Matches multiplicative operators.
	/// </summary>
	[GeneratedRegex(@"\s*(\*|div|mod)\s*")]
	public static partial Regex ProductRegex();

	/// <summary>
	/// Matches an absolute node reference.
	/// </summary>
	[GeneratedRegex(@"((?:\/[A-Za-z_][A-Za-z0-9_-]*)+)")]
	public static partial Regex NodeAbsoluteRegex();

	/// <summary>
	/// Matches a numeric literal.
	/// </summary>
	[GeneratedRegex(@"\s*(-?(?:\d+\.\d+|\d+))\s*")]
	public static partial Regex NumberRegex();

	/// <summary>
	/// Matches an identifier.
	/// </summary>
	[GeneratedRegex(@"\s*([A-Za-z_][A-Za-z0-9_:-]*)\s*")]
	public static partial Regex IdentifierRegex();

	/// <summary>
	/// Matches the start of a function call.
	/// </summary>
	[GeneratedRegex(@"\s*(\()\s*")]
	public static partial Regex FunctionStartRegex();

	/// <summary>
	/// Matches the end of a function call.
	/// </summary>
	[GeneratedRegex(@"\s*(\))\s*")]
	public static partial Regex FunctionEndRegex();

	/// <summary>
	/// Matches a comma within a function call.
	/// </summary>
	[GeneratedRegex(@"\s*(,)\s*")]
	public static partial Regex FunctionCommaRegex();

	/// <summary>
	/// Matches the start of a parenthesis.
	/// </summary>
	[GeneratedRegex(@"\s*(\()\s*")]
	public static partial Regex ParenStartRegex();

	/// <summary>
	/// Matches the end of a parenthesis.
	/// </summary>
	[GeneratedRegex(@"\s*(\))\s*")]
	public static partial Regex ParenEndRegex();

	/// <summary>
	/// Matches the negation operator.
	/// </summary>
	[GeneratedRegex(@"\s*(-)\s*")]
	public static partial Regex NegateRegex();

	/// <summary>
	/// Matches a single quote.
	/// </summary>
	[GeneratedRegex("""(')""")]
	public static partial Regex SingleQuotedRegex();

	/// <summary>
	/// Matches the body of a single-quoted string.
	/// </summary>
	[GeneratedRegex("""([^\']*)""")]
	public static partial Regex SingleQuotedBodyRegex();

	/// <summary>
	/// Matches a double quote.
	/// </summary>
	[GeneratedRegex("""(")""")]
	public static partial Regex DoubleQuotedRegex();

	/// <summary>
	/// Matches the body of a double-quoted string.
	/// </summary>
	[GeneratedRegex("""([^\"]*)""")]
	public static partial Regex DoubleQuotedBodyRegex();
}
