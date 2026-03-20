// ExpressionNode.cs

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a single value or expression in the expression graph.
/// The value of an <see cref="ExpressionNode"/> can be set by user input or by evaluating the expression defined by the node.
/// The <see cref="ExpressionManager"/> manages a collection of <see cref="ExpressionNode"/>s and their dependencies,
/// and is responsible for evaluating the expressions and updating the values of the nodes accordingly.
/// </summary>
public partial class ExpressionNode : INotifyPropertyChanged
{
	/// <summary>
	/// The owning <see cref="ExpressionManager"/> responsible for evaluating
	/// and updating this node.
	/// </summary>
	public ExpressionManager? Owner { get; internal set; }

	/// <summary>
	/// The unique reference string identifying this node within the expression graph.
	/// </summary>
	public string NodeRef { get; internal set; } = string.Empty;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExpressionNode"/> class.
	/// </summary>
	public ExpressionNode()
	{
	}

	/// <summary>
	/// Gets or sets the externally visible value of this node.
	/// </summary>
	public object? Value
	{
		get => InternalValue;
		set => Owner?.SetValue(NodeRef, value, ExpressionValueKind.UserInput);
	}

	/// <summary>
	/// Raises a property-changed notification for <see cref="Value"/>.
	/// </summary>
	public void OnValueChanged()
		=> OnPropertyChanged(nameof(Value));

	/// <summary>
	/// Sets the internal value of the node without triggering dependency propagation.
	/// </summary>
	/// <param name="value">The value to assign.</param>
	internal void SetInternalValue(object? value)
	{
		InternalValue = value;
	}

	/// <summary>
	/// Gets the internally stored value for this node.
	/// </summary>
	internal object? InternalValue { get; private set; }

	/// <summary>
	/// Describes the current lifecycle state of the node.
	/// </summary>
	public partial ExpressionValueKind ValueKind { get; internal set; } = ExpressionValueKind.Default;
	public partial ExpressionValueKind ValueKind
	{
		get => field;
		internal set => SetProperty(ref field, value);
	}

	/// <summary>
	/// The expected type of the node's value, if constrained.
	/// </summary>
	public partial Type? ValueType { get; internal set; } = null;
	public partial Type? ValueType
	{
		get => field;
		internal set => SetProperty(ref field, value);
	}

	/// <summary>
	/// The expression text associated with this node.
	/// </summary>
	public partial string Expression { get; internal set; } = string.Empty;
	public partial string Expression
	{
		get => field;
		internal set => SetProperty(ref field, value);
	}

	/// <summary>
	/// Indicates whether the node's value is deterministic.
	/// </summary>
	public partial bool IsDeterministic { get; internal set; } = true;
	public partial bool IsDeterministic
	{
		get => field;
		internal set => SetProperty(ref field, value);
	}

	/// <summary>
	/// The parsed tokens representing the node's expression in evaluation order.
	/// </summary>
	public readonly List<ExpressionToken> Tokens = new();

	/// <summary>
	/// References to nodes that this node depends on as inputs.
	/// </summary>
	public readonly ConcurrentDictionary<string, byte> InputNodeRefs = new();

	/// <summary>
	/// References to nodes that depend on this node as an input.
	/// </summary>
	public readonly ConcurrentDictionary<string, byte> OutputNodeRefs = new();

	/// <summary>
	/// Returns a string representation of the node's current value.
	/// </summary>
	public override string? ToString() => InternalValue?.ToString();

	/// <summary>
	/// Resets the node to an uninitialized state and clears all internal data.
	/// </summary>
	internal void Clear()
	{
		Owner = null;
		InternalValue = null;
		ValueKind = ExpressionValueKind.Uninitialized;
		ValueType = null;
		Expression = string.Empty;
		InputNodeRefs.Clear();
		OutputNodeRefs.Clear();
		Tokens.Clear();
	}

	/// <summary>
	/// Sets the specified property's backing field to a new value and raises the property changed notification if the
	/// value has changed.
	/// </summary>
	/// <typeparam name="T">The type of the property being set.</typeparam>
	/// <param name="storage">A reference to the field that stores the current value of the property.</param>
	/// <param name="value">The new value to assign to the property.</param>
	/// <param name="propertyName">The name of the property that changed. This value is automatically provided by the compiler if not specified.</param>
	/// <returns>true if the value was changed and the property changed notification was raised; otherwise, false.</returns>
	protected bool SetProperty<T>(
		ref T storage,
		T value,
		[CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(storage, value))
		{
			return false;
		}

		storage = value;
		OnPropertyChanged(propertyName);
		return true;
	}

	/// <summary>
	/// Raises the PropertyChanged event for a specified property to notify listeners that the property's value has changed.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. If not specified, the name of the calling member is used.</param>
	public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;
}
