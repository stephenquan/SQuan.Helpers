// ExpressionWorkItem.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a piece of work to be done by the expression engine.
/// </summary>
class ExpressionWorkItem
{
	public required ExpressionNode Node { get; set; } = ExpressionNode.Empty;
}
