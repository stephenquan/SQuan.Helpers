// ExpressionWorkItemNode.cs

namespace SQuan.Helpers.Maui;

class ExpressionWorkItemNode : IExpressionWorkItem
{
	public required IExpressionNode Node { get; init; }
}
