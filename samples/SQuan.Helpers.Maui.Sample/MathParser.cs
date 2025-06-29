using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SQuan.Helpers.Maui.Sample;

public class IMathToken
{

}

public class MathNumberToken : IMathToken
{
	public object? Value { get; set; }
	public MathNumberToken(object? value = null)
	{
		this.Value = value;
	}
}

public class MathOperatorToken : IMathToken
{
	public string Operator { get; set; } = string.Empty;
	public Func<object?, object?, object?>? Operation;

	public MathOperatorToken(string op)
	{
		this.Operator = op;
		if (MathParser.Operators.TryGetValue(op, out Func<object?, object?, object?>? operation))
		{
			this.Operation = operation;
		}
		else
		{
			this.Operation = null;
		}
	}
}

public class MathIdentifierToken : IMathToken
{
	public int Index { get; set; } = 0;
	public MathIdentifierToken(int index)
	{
		this.Index = index;
	}
}

public partial class MathParser
{
	public static Dictionary<string, Func<object?, object?, object?>> Operators { get; } = new Dictionary<string, Func<object?, object?, object?>>
	{
		{ "*", Mul },
		{ "/", Div },
		{ "+", Add },
		{ "-", Sub },
	};

	public static bool ConvertToDouble(object? value, out double result)
	{
		if (value is null)
		{
			result = double.NaN;
			return false;
		}

		if (value is string s)
		{
			result = double.Parse(s, CultureInfo.InvariantCulture);
			return true;
		}

		if (value is double d)
		{
			result = d;
			return true;
		}

		result = Convert.ToDouble(value);
		return true;
	}

	public static object? Mul(object? a, object? b) => ConvertToDouble(a, out double da) && ConvertToDouble(b, out double db) ? da * db : null;
	public static object? Div(object? a, object? b) => ConvertToDouble(a, out double da) && ConvertToDouble(b, out double db) ? da / db : null;
	public static object? Add(object? a, object? b) => ConvertToDouble(a, out double da) && ConvertToDouble(b, out double db) ? da + db : null;
	public static object? Sub(object? a, object? b) => ConvertToDouble(a, out double da) && ConvertToDouble(b, out double db) ? da - db : null;
	public string Expression { get; private set; } = string.Empty;
	public int ExpressionIndex { get; private set; } = 0;
	public bool Valid { get; private set; } = false;
	public Match PatternMatch { get; private set; } = Match.Empty;
	public ObservableCollection<IMathToken> Tokens { get; } = [];

	[GeneratedRegex("""^\s*""")]
	private static partial Regex SpacePattern();

	public bool Prepare(string expression)
	{
		this.Tokens.Clear();
		this.Expression = expression.Trim();
		this.ExpressionIndex = 0;
		this.Valid = false;

		if (!ParseExpr())
		{
			return false;
		}

		ParsePattern(SpacePattern());

		this.Valid = ExpressionIndex >= Expression.Length;
		return this.Valid;
	}

	public object? Evaluate(object?[] args)
	{
		Stack<object?> stack = [];
		foreach (IMathToken token in Tokens)
		{
			if (token is MathNumberToken numberToken)
			{
				stack.Push(numberToken.Value);
			}
			else if (token is MathIdentifierToken identifierToken)
			{
				if (identifierToken.Index < 0 || identifierToken.Index >= args.Length)
				{
					return null; // Invalid index
				}
				stack.Push(args[identifierToken.Index]);
			}
			else if (token is MathOperatorToken operatorToken)
			{
				if (stack.Count < 2 || operatorToken.Operation == null)
				{
					return null; // Not enough operands or invalid operation
				}
				object? b = stack.Pop();
				object? a = stack.Pop();
				object? result = operatorToken.Operation(a, b);
				stack.Push(result);
			}
		}
		if (stack.Count != 1)
		{
			return null; // Invalid expression
		}

		var finalResult = stack.Pop();
		return finalResult;
	}

	bool ParseExpr()
	{
		return ParseSum();
	}

	[GeneratedRegex("""^\s*(\-|\+)\s*""")]
	private static partial Regex SumPattern();

	bool ParseSum()
	{
		if (!ParseProduct())
		{
			return false;
		}

		while (ParsePattern(SumPattern()))
		{
			string op = PatternMatch.Value.Trim();
			if (!ParseProduct())
			{
				return false;
			}

			MathOperatorToken opToken = new MathOperatorToken(op);

			if (Tokens.Count >= 2 && Tokens[^1] is MathNumberToken b && Tokens[^2] is MathNumberToken a)
			{
				if (opToken.Operation != null)
				{
					object? result = opToken.Operation(a.Value, b.Value);
					Tokens[^2] = new MathNumberToken(result);
					Tokens.RemoveAt(Tokens.Count - 1); // Remove b
				}
			}
			else
			{
				Tokens.Add(opToken);
			}
		}

		return true;
	}

	[GeneratedRegex("""^\s*(\*|\/)\s*""")]
	private static partial Regex ProductPattern();

	bool ParseProduct()
	{
		if (!ParsePrimary())
		{
			return false;
		}

		while (ParsePattern(ProductPattern()))
		{
			string op = PatternMatch.Value.Trim();
			if (!ParsePrimary())
			{
				return false;
			}

			MathOperatorToken opToken = new MathOperatorToken(op);

			if (Tokens.Count >= 2 && Tokens[^1] is MathNumberToken b && Tokens[^2] is MathNumberToken a)
			{
				if (opToken.Operation != null)
				{
					object? result = opToken.Operation(a.Value, b.Value);
					Tokens[^2] = new MathNumberToken(result);
					Tokens.RemoveAt(Tokens.Count - 1); // Remove b
				}
			}
			else
			{
				Tokens.Add(opToken);
			}
		}

		return true;
	}

	[GeneratedRegex("""^\s*(\()\s*""")]
	private static partial Regex OpenParenPattern();

	[GeneratedRegex("""^\s*(\()\s*""")]
	private static partial Regex CloseParenPattern();

	bool ParsePrimary()
	{
		if (ParseNumber())
		{
			return true;
		}

		if (ParseIdentifier())
		{
			return true;
		}

		if (ParsePattern(OpenParenPattern()))
		{
			if (!ParseExpr())
			{
				return false;
			}
			if (!ParsePattern(CloseParenPattern()))
			{
				return false;
			}
			return true;
		}

		return false;
	}

	[GeneratedRegex("""^\s*(\-?\d+\.\d+|\-?\d+)\s*""")]
	private static partial Regex NumberPattern();

	bool ParseNumber()
	{
		if (!ParsePattern(NumberPattern()))
		{
			return false;
		}

		if (!double.TryParse(PatternMatch.Value, CultureInfo.InvariantCulture, out double value))
		{
			return false;
		}

		Tokens.Add(new MathNumberToken(value));
		return true;
	}

	[GeneratedRegex("""^\s*x(\d+)\s*""")]
	private static partial Regex IdentifierPattern();

	bool ParseIdentifier()
	{
		if (!ParsePattern(IdentifierPattern()))
		{
			return false;
		}
		if (!int.TryParse(PatternMatch.Groups[1].Value, out int value))
		{
			return false;
		}
		Tokens.Add(new MathIdentifierToken(value));
		return true;
	}

	bool ParsePattern(Regex pattern)
	{
		PatternMatch = pattern.Match(Expression[ExpressionIndex..]);
		if (!PatternMatch.Success)
		{
			return false;
		}

		ExpressionIndex += PatternMatch.Length;
		return true;
	}
}
