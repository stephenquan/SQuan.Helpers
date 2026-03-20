// ExpressionAritySpec.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Describes the allowed arity (number of arguments) for a function or operator.
/// </summary>
public readonly record struct AritySpec
{
	/// <summary>
	/// The minimum number of arguments required.
	/// </summary>
	public int Min { get; }

	/// <summary>
	/// The maximum number of arguments allowed, or null if unbounded.
	/// </summary>
	public int? Max { get; }

	/// <summary>
	/// Initializes a new <see cref="AritySpec"/> with the specified bounds.
	/// </summary>
	/// <param name="min">The minimum number of arguments.</param>
	/// <param name="max">The maximum number of arguments, or null to indicate no upper bound.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="min"/> is negative, or if <paramref name="max"/> is less than <paramref name="min"/>.</exception>
	public AritySpec(int min, int? max)
	{
		if (min < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(min));
		}
		if (max is not null && max < min)
		{
			throw new ArgumentOutOfRangeException(nameof(max));
		}

		Min = min;
		Max = max;
	}

	/// <summary>
	/// Creates an arity specification that requires exactly <paramref name="n"/> arguments.
	/// </summary>
	public static AritySpec Exactly(int n) => new(n, n);

	/// <summary>
	/// Creates an arity specification that allows a range of arguments.
	/// </summary>
	/// <param name="min">The minimum number of arguments.</param>
	/// <param name="max">The maximum number of arguments.</param>
	public static AritySpec Range(int min, int max) => new(min, max);

	/// <summary>
	/// Creates an arity specification that allows at least <paramref name="min"/> arguments.
	/// </summary>
	public static AritySpec AtLeast(int min) => new(min, null);

	/// <summary>Requires zero arguments.</summary>
	public static AritySpec Zero => Exactly(0);

	/// <summary>Allows zero or more arguments.</summary>
	public static AritySpec ZeroOrMore => AtLeast(0);

	/// <summary>Requires exactly one argument.</summary>
	public static AritySpec One => Exactly(1);

	/// <summary>Requires one or two arguments.</summary>
	public static AritySpec OneOrTwo => Range(1, 2);

	/// <summary>Allows one or more arguments.</summary>
	public static AritySpec OneOrMore => AtLeast(1);

	/// <summary>Requires exactly two arguments.</summary>
	public static AritySpec Two => Exactly(2);

	/// <summary>Allows two or three arguments.</summary>
	public static AritySpec TwoOrThree => Range(2, 3);

	/// <summary>Requires exactly three arguments.</summary>
	public static AritySpec Three => Exactly(3);

	/// <summary>
	/// Determines whether this specification requires exactly <paramref name="n"/> arguments.
	/// </summary>
	public bool IsExactly(int n) => Min == n && Max == n;

	/// <summary>
	/// Indicates whether the arity is exactly zero.
	/// </summary>
	public bool IsZero => IsExactly(0);

	/// <summary>
	/// Indicates whether the arity is exactly one.
	/// </summary>
	public bool IsOne => IsExactly(1);

	/// <summary>
	/// Indicates whether the arity is exactly two.
	/// </summary>
	public bool IsTwo => IsExactly(2);

	/// <summary>
	/// Indicates whether the arity is fixed (minimum equals maximum).
	/// </summary>
	public bool IsFixed => Max is int max && max == Min;

	/// <summary>
	/// Indicates whether the arity is a range (minimum does not equal maximum).
	/// </summary>
	public bool IsRange => Max is int max && max != Min;

	/// <summary>
	/// Indicates whether the arity is unbounded (no maximum).
	/// </summary>
	public bool IsInfinite => Max is null;

	/// <summary>
	/// Determines whether the specified argument count is accepted by this arity.
	/// </summary>
	/// <param name="arity">The argument count to test.</param>
	/// <returns>True if the argument count falls within the allowed range; otherwise false.</returns>
	public bool Accepts(int arity) =>
		arity >= Min && (Max is null || arity <= Max.Value);

	/// <summary>
	/// Returns a string representation of the arity specification.
	/// </summary>
	/// <returns>A human-readable representation of the allowed argument range.</returns>
	public override string ToString() =>
		Max is null ? $"{Min}..Infinity" : (Min == Max ? Min.ToString() : $"{Min}..{Max}");
}
