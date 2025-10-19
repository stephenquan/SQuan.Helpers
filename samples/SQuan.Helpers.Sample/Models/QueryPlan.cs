// QueryPlan.cs

using SQLite;

namespace SQuan.Helpers.Sample;

/// <summary>
/// Query plan model for SQLite EXPLAIN QUERY PLAN results.
/// </summary>
public class QueryPlan
{
	[Column("id")]
	public int Id { get; set; }

	[Column("parent")]
	public int Parent { get; set; }

	[Column("notused")]
	public int NotUsed { get; set; }

	[Column("detail")]
	public string Detail { get; set; } = string.Empty;
}
