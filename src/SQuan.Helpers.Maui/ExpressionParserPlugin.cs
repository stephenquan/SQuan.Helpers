// ExpressionParserPlugin.cs

using System.Text.RegularExpressions;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides the configurable syntax and function registry used by
/// <see cref="ExpressionParser"/> and the expression evaluation engine.
/// </summary>
public class ExpressionParserPlugin
{
	/// <summary>
	/// Regular expression matching the logical OR operator.
	/// </summary>
	public Regex LogicalOrRegex = ExpressionParserDefaults.LogicalOrOperatorRegex();

	/// <summary>
	/// Regular expression matching the logical AND operator.
	/// </summary>
	public Regex LogicalAndRegex = ExpressionParserDefaults.LogicalAndOperatorRegex();

	/// <summary>
	/// Regular expression matching comparison operators.
	/// </summary>
	public Regex ComparisonOperatorsRegex = ExpressionParserDefaults.ComparisonOperatorsRegex();

	/// <summary>
	/// Regular expression matching equality operators.
	/// </summary>
	public Regex EqualityOperatorsRegex = ExpressionParserDefaults.EqualityOperatorsRegex();

	/// <summary>
	/// Regular expression matching additive operators.
	/// </summary>
	public Regex SumRegex = ExpressionParserDefaults.SumRegex();

	/// <summary>
	/// Regular expression matching multiplicative operators.
	/// </summary>
	public Regex ProductRegex = ExpressionParserDefaults.ProductRegex();

	/// <summary>
	/// Regular expression matching absolute node references.
	/// </summary>
	public Regex NodeAbsoluteRegex = ExpressionParserDefaults.NodeAbsoluteRegex();

	/// <summary>
	/// Regular expression matching numeric literals.
	/// </summary>
	public Regex NumberRegex = ExpressionParserDefaults.NumberRegex();

	/// <summary>
	/// Regular expression matching identifiers.
	/// </summary>
	public Regex IdentifierRegex = ExpressionParserDefaults.IdentifierRegex();

	/// <summary>
	/// Regular expression matching the start of a function call.
	/// </summary>
	public Regex FunctionStartRegex = ExpressionParserDefaults.FunctionStartRegex();

	/// <summary>
	/// Regular expression matching the end of a function call.
	/// </summary>
	public Regex FunctionEndRegex = ExpressionParserDefaults.FunctionEndRegex();

	/// <summary>
	/// Regular expression matching the comma separator in function arguments.
	/// </summary>
	public Regex FunctionCommaRegex = ExpressionParserDefaults.FunctionCommaRegex();

	/// <summary>
	/// Regular expression matching the start of a parenthesis.
	/// </summary>
	public Regex ParenStartRegex = ExpressionParserDefaults.ParenStartRegex();

	/// <summary>
	/// Regular expression matching the end of a parenthesis.
	/// </summary>
	public Regex ParenEndRegex = ExpressionParserDefaults.ParenEndRegex();

	/// <summary>
	/// Regular expression matching the negation operator.
	/// </summary>
	public Regex NegateRegex = ExpressionParserDefaults.NegateRegex();

	/// <summary>
	/// Regular expression matching single-quoted strings.
	/// </summary>
	public Regex SingleQuotedRegex = ExpressionParserDefaults.SingleQuotedRegex();

	/// <summary>
	/// Regular expression matching the body of single-quoted strings.
	/// </summary>
	public Regex SingleQuotedBodyRegex = ExpressionParserDefaults.SingleQuotedBodyRegex();

	/// <summary>
	/// Regular expression matching double-quoted strings.
	/// </summary>
	public Regex DoubleQuotedRegex = ExpressionParserDefaults.DoubleQuotedRegex();

	/// <summary>
	/// Regular expression matching the body of double-quoted strings.
	/// </summary>
	public Regex DoubleQuotedBodyRegex = ExpressionParserDefaults.DoubleQuotedBodyRegex();

	/// <summary>
	/// Registry of supported operators and functions.
	/// </summary>
	public Dictionary<string, ExpressionFunctionInfo> Functions = new()
	{
		// https://docs.getodk.org/form-operators-functions/#math-operators
		{ "+", new (args => args.WrapFunc<double,double,double>((a, b) => a + b), AritySpec.Two) },
		{ "-", new (args => args.WrapFunc<double,double,double>((a, b) => a - b), AritySpec.Two) },
		{ "*", new (args => args.WrapFunc<double,double,double>((a, b) => a * b), AritySpec.Two) },
		{ "div", new (args => args.WrapFunc<double,double,double>((a, b) => a / b), AritySpec.Two) },
		{ "mod", new (args => args.WrapFunc<double,double,double>((a, b) => a % b), AritySpec.Two) },
		// https://docs.getodk.org/form-operators-functions/#comparison-operators
		{ "=", new (args => args.WrapFunc<object,object,bool>((a, b) => Eq(a,b)), AritySpec.Two) },
		{ "!=", new (args => args.WrapFunc<object,object,bool>((a, b) => !Eq(a,b)), AritySpec.Two) },
		{ ">", new (args => args.WrapFunc<double,double,bool>((a, b) => a > b), AritySpec.Two) },
		{ ">=", new (args => args.WrapFunc<double,double,bool>((a, b) => a >= b), AritySpec.Two) },
		{ "<", new (args => args.WrapFunc<double,double,bool>((a, b) => a < b), AritySpec.Two) },
		{ "<=", new (args => args.WrapFunc<double,double,bool>((a, b) => a <= b), AritySpec.Two) },

		// https://docs.getodk.org/form-operators-functions/#boolean-operators
		{ "and", new (args => args.WrapFunc<bool,bool,bool>((a, b) => a && b), AritySpec.Two) },
		{ "or", new (args => args.WrapFunc<bool,bool,bool>((a, b) => a || b), AritySpec.Two) },
		// https://docs.getodk.org/form-operators-functions/#control-flow
		{ "if", new (args => ToBoolean(args[0]) ? args[1] : args[2], AritySpec.Three) },

		// https://docs.getodk.org/form-operators-functions/#strings
		{ "regex", new (args => args.WrapFunc<string,string,bool>((value, pattern) => Regex.IsMatch(value, pattern)), AritySpec.Two) },
		{ "starts-with", new (args => args.WrapFunc<string,string,bool>((value, prefix) => value.StartsWith(prefix)), AritySpec.Two) },
		{ "ends-with", new (args => args.WrapFunc<string,string,bool>((value, suffix) => value.EndsWith(suffix)), AritySpec.Two) },
		{ "substr", new (args => args.WrapFunc<string,int,string>((value, start) => value.Substring(start)), AritySpec.Two) },
		{ "string-length", new (args => (args[0]?.ToString() is string s) ? s.Length : 0, AritySpec.One) },
		{ "normalize-space", new (args => args[0]?.ToString() is string s ? Regex.Replace(s, @"\s+", " ").Trim() : "", AritySpec.One) },
		{ "concat", new (args => string.Join("", args), AritySpec.AtLeast(0)) },
		{ "boolean-from-string", new (args => ToBoolean(args[0]?.ToString()), AritySpec.One) },
		{ "string", new (args => args[0]?.ToString(), AritySpec.One) },

		// https://docs.getodk.org/form-operators-functions/#math
		{ "pow", new (args => args.WrapFunc<double,double,double>((x, y) => Math.Pow(x, y)), AritySpec.Two) },
		{ "log", new (args => args.WrapFunc<double,double>((x) => Math.Log(x)), AritySpec.One) },
		{ "log10", new (args => args.WrapFunc<double,double>((x) => Math.Log10(x)), AritySpec.One) },
		{ "abs", new (args => args.WrapFunc<double,double>((x) => Math.Abs(x)), AritySpec.One) },
		{ "sin", new (args => args.WrapFunc<double,double>((x) => Math.Sin(x)), AritySpec.One) },
		{ "cos", new (args => args.WrapFunc<double,double>((x) => Math.Cos(x)), AritySpec.One) },
		{ "tan", new (args => args.WrapFunc<double,double>((x) => Math.Tan(x)), AritySpec.One) },
		{ "asin", new (args => args.WrapFunc<double,double>((x) => Math.Asin(x)), AritySpec.One) },
		{ "acos", new (args => args.WrapFunc<double,double>((x) => Math.Acos(x)), AritySpec.One) },
		{ "atan", new (args => args.WrapFunc<double,double>((x) => Math.Atan(x)), AritySpec.One) },
		{ "atan2", new (args => args.WrapFunc<double,double,double>((y, x) => Math.Atan2(y, x)), AritySpec.Two) },
		{ "sqrt", new (args => args.WrapFunc<double,double>((x) => Math.Sqrt(x)), AritySpec.One) },
		{ "exp", new (args => args.WrapFunc<double,double>((x) => Math.Exp(x)), AritySpec.One) },
		{ "exp10", new (args => args.WrapFunc<double,double>((x) => Math.Pow(10, x)), AritySpec.One) },
		{ "pi", new (args => Math.PI, AritySpec.Zero) },

		// https://docs.getodk.org/form-operators-functions/#number-handling
		{ "round", new (args => args.WrapFunc<double,int,double>((x, digits) => Math.Round(x, digits)), AritySpec.Two) },
		{ "int", new (args => args.WrapFunc<double,int>((x) => (int)Math.Floor(x)), AritySpec.One) },
		{ "number", new (args => args.WrapFunc<object,double>((x) => args[0] is not null && double.TryParse(x.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result) ? result : 0), AritySpec.One) },

		// https://docs.getodk.org/form-operators-functions/#date-and-time
		{ "today", new (args => Today(), AritySpec.Zero, false) },
		{ "now", new (args => Now(), AritySpec.Zero, false) },
		// https://docs.getodk.org/form-operators-functions/#utility
		{ "random", new (args => Random.Shared.NextDouble(), AritySpec.Zero, false) },
		{ "uuid", new (args => Guid.NewGuid().ToString(), AritySpec.Zero, false) },
		{ "boolean", new (args => ToBoolean(args[0]), AritySpec.One) },
		{ "not",new (args => !ToBoolean(args[0]), AritySpec.One) },
		{ "true", new (args => true, AritySpec.Zero) },
		{ "false", new (args => false, AritySpec.Zero) },

		{ "negate", new (args => args.WrapFunc<double,double>(a => -a), AritySpec.One) },
	};

	/// <summary>
	/// Compares two values for equality with strict type matching.
	/// </summary>
	/// <param name="x">The first value.</param>
	/// <param name="y">The second value.</param>
	/// <returns>True if both values are null or equal and of the same type; otherwise false.</returns>
	public static bool Eq(object? x, object? y)
	{
		if (x is null && y is null)
		{
			return true;
		}
		if (x is null || y is null)
		{
			return false;
		}
		if (x.GetType() != y.GetType())
		{
			return false;
		}
		return x.Equals(y);
	}

	/// <summary>
	/// Returns the current date (local) as a Unix timestamp in milliseconds.
	/// </summary>
	public static long Today()
		=> new DateTimeOffset(
			DateTime.Today.Year,
			DateTime.Today.Month,
			DateTime.Today.Day,
			12, 0, 0,
			DateTimeOffset.Now.Offset).ToUnixTimeMilliseconds();

	/// <summary>
	/// Returns the current UTC time as a Unix timestamp in milliseconds.
	/// </summary>
	public static long Now()
		=> DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

	/// <summary>
	/// Converts a value to a boolean using expression semantics.
	/// </summary>
	/// <param name="arg">The value to convert.</param>
	/// <returns>The boolean interpretation of the value.</returns>
	public static bool ToBoolean(object? arg)
		=> arg switch
		{
			null => false,

			// Boolean: return as-is
			bool b => b,

			// Numeric types: true if non-zero
			sbyte sb => sb != 0,
			byte by => by != 0,
			short sh => sh != 0,
			ushort ush => ush != 0,
			int i => i != 0,
			uint ui => ui != 0,
			long l => l != 0,
			ulong ul => ul != 0,
			float f => f != 0f,
			double d => d != 0d,
			decimal m => m != 0m,

			// String: true if non-empty
			string s => s.Length > 0,

			// ICollection: true if non-empty
			System.Collections.ICollection col => col.Count > 0,

			// Any other non-null object: true
			_ => true
		};
}
