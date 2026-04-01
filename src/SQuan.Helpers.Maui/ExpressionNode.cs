// ExpressionNode.cs

using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a single value or expression in the expression graph.
/// The value of an <see cref="ExpressionNode"/> can be set by user input or by evaluating the expression defined by the node.
/// The <see cref="ExpressionManager"/> manages a collection of <see cref="ExpressionNode"/>s and their dependencies,
/// and is responsible for evaluating the expressions and updating the values of the nodes accordingly.
/// </summary>
public partial class ExpressionNode : ObservableObject
{
	static ILogger? logger;

	/// <summary>
	/// An empty expression node.
	/// </summary>
	public static ExpressionNode Empty { get; } = new();

	/// <summary>
	/// Logger used for diagnostics, tracing, and error reporting.
	/// </summary>
	public static ILogger? Logger
	{
		get
		{
			if (logger is not null)
			{
				return logger;
			}

			var app = IPlatformApplication.Current;
			if (app is null)
			{
				return null;
			}

			logger = app.Services.GetService<ILogger<ExpressionManager>>();
			return logger;
		}
	}

	/// <summary>
	/// The owning <see cref="ExpressionManager"/> responsible for evaluating
	/// and updating this node.
	/// </summary>
	public ExpressionManager? Owner { get; set; }

	/// <summary>
	/// The unique reference string identifying this node within the expression graph.
	/// </summary>
	public string NodeRef { get; set; } = string.Empty;

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
		set => Owner?.SetValue(this, value, ExpressionValueKind.UserInput);
	}

	/// <summary>
	/// Gets or sets the text representation of the node's value.
	/// </summary>
	public string? TextValue
	{
		get => InternalValue?.ToString();
		set => Owner?.SetValue(this, value, ExpressionValueKind.UserInput);
	}

	/// <summary>
	/// Raises a property-changed notification for <see cref="Value"/>.
	/// </summary>
	public void OnValueChanged()
	{
		OnPropertyChanged(nameof(Value));
		OnPropertyChanged(nameof(TextValue));
	}

	/// <inheritdoc/>
	public object? GetInternalValue()
	{
		return InternalValue;
	}

	/// <summary>
	/// Sets the internal value of the node without triggering dependency propagation.
	/// </summary>
	/// <param name="value">The value to assign.</param>
	public bool SetInternalValue(object? value)
	{
		if (value is null)
		{
			return SetNull();
		}
		if (value is string strValue && string.IsNullOrEmpty(strValue) && ValueType != typeof(string))
		{
			return SetNull();
		}
		if (!value.TryConvert(ValueType, out var convertedValue))
		{
			return false;
		}
		if (convertedValue is null)
		{
			return SetNull();
		}
		if (convertedValue.Equals(InternalValue))
		{
			return false;
		}
		InternalValue = convertedValue;
		return true;
	}

	bool SetNull()
	{
		if (InternalValue is null)
		{
			return false;
		}
		InternalValue = null;
		return true;
	}

	/// <summary>
	/// Gets the internally stored value for this node.
	/// </summary>
	internal object? InternalValue { get; set; }

	/// <summary>
	/// Describes the current lifecycle state of the node.
	/// </summary>
	[ObservableProperty]
	public partial ExpressionValueKind ValueKind { get; set; } = ExpressionValueKind.Uninitialized;

	/// <summary>
	/// The expected type of the node's value, if constrained.
	/// </summary>
	[ObservableProperty]
	public partial Type? ValueType { get; set; } = null;

	/// <summary>
	/// The expression text associated with this node.
	/// </summary>
	[ObservableProperty]
	public partial string Expression { get; set; } = string.Empty;

	/// <summary>
	/// Indicates whether the node's value is deterministic.
	/// </summary>
	[ObservableProperty]
	public partial bool IsDeterministic { get; set; } = true;

	/// <summary>
	/// The parsed tokens representing the node's expression in evaluation order.
	/// </summary>
	public List<ExpressionToken> Tokens { get; } = new();

	/// <summary>
	/// References to nodes that this node depends on as inputs.
	/// </summary>
	public readonly ConcurrentDictionary<string, byte> InputNodeRefs = new();

	/// <summary>
	/// References to nodes that depend on this node as outputs.
	/// </summary>
	public readonly ConcurrentDictionary<string, byte> OutputNodeRefs = new();

	/// <summary>
	/// Gets or sets the default expression that is used for initialization when no other value is provided.
	/// </summary>
	[ObservableProperty]
	public partial string DefaultExpression { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets a value indicating whether the default expression is deterministic.
	/// </summary>
	[ObservableProperty]
	public partial bool IsDefaultExpressionDeterministic { get; set; } = true;

	/// <summary>
	/// Gets the list of tokens used for evaluating default expressions.
	/// </summary>
	public List<ExpressionToken> DefaultExpressionTokens { get; } = new();

	/// <summary>
	/// Returns a string representation of the node's current value.
	/// </summary>
	public override string? ToString() => InternalValue?.ToString();

	/// <inheritdoc/>
	public void SetTokens(IEnumerable<ExpressionToken> tokens)
	{
		Tokens.Clear();
		IsDeterministic = true;
		InputNodeRefs.Clear();
		foreach (var token in tokens)
		{
			Tokens.Add(token);
			switch (token.TokenType)
			{
				case ExpressionTokenType.Function:
					if (token.FunctionInfo is not null && !token.FunctionInfo.IsDeterministic)
					{
						IsDeterministic = false;
					}
					break;
				case ExpressionTokenType.Node:
					IsDeterministic = false;
					InputNodeRefs.TryAdd(token.Text, 0);
					break;
			}
		}
	}

	/// <inheritdoc/>
	public void SetDefaultExpressionTokens(IEnumerable<ExpressionToken> tokens)
	{
		DefaultExpressionTokens.Clear();
		IsDefaultExpressionDeterministic = true;
		foreach (var token in tokens)
		{
			DefaultExpressionTokens.Add(token);
			switch (token.TokenType)
			{
				case ExpressionTokenType.Function:
					if (token.FunctionInfo is not null && !token.FunctionInfo.IsDeterministic)
					{
						IsDefaultExpressionDeterministic = false;
					}
					break;
				case ExpressionTokenType.Node:
					IsDefaultExpressionDeterministic = false;
					break;
			}
		}
	}

	/// <summary>
	/// Resets the node to an uninitialized state and clears all internal data.
	/// </summary>
	public void Clear()
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

	/// <inheritdoc/>
	public List<string> GetInputNodeRefs() => InputNodeRefs.Keys.ToList();

	/// <inheritdoc/>
	public List<string> GetOutputNodeRefs() => OutputNodeRefs.Keys.ToList();

	/// <inheritdoc/>
	public bool TryAddOutputNodeRef(string nodeRef) => OutputNodeRefs.TryAdd(nodeRef, 0);

	/// <summary>
	/// Creates a data binding for the TextValue property with the specified binding mode, value converter, converter
	/// parameter, and string format.
	/// </summary>
	/// <param name="mode">The binding mode that determines how changes are propagated between the source and the target. The default is
	/// BindingMode.OneWay.</param>
	/// <param name="converter">An optional value converter that transforms values between the source and target properties during binding. May be
	/// null if no conversion is required.</param>
	/// <param name="converterParameter">An optional parameter to pass to the value converter to influence its logic. May be null if not needed.</param>
	/// <param name="stringFormat">An optional string format to apply to the bound value when displaying it in the target property. May be null if no
	/// formatting is required.</param>
	/// <returns>A BindingBase instance representing the configured binding for the TextValue property.</returns>
	public BindingBase BindTextValue(BindingMode mode = BindingMode.OneWay, IValueConverter? converter = null, object? converterParameter = null, string? stringFormat = null)
		=> new Binding(nameof(TextValue), mode, converter, converterParameter, stringFormat, source: this);

	/// <summary>
	/// Creates a data binding for the Value property with the specified binding options.
	/// </summary>
	/// <param name="mode">The binding mode that determines how changes are propagated between the source and target properties. The default
	/// is BindingMode.Default.</param>
	/// <param name="converter">An optional value converter that transforms the value between the source and target properties during binding.</param>
	/// <param name="converterParameter">An optional parameter to pass to the value converter to influence its conversion logic.</param>
	/// <param name="stringFormat">An optional string format to apply to the bound value when it is displayed in the target property.</param>
	/// <returns>A BindingBase instance that represents the configured binding for the Value property.</returns>
	public BindingBase BindValue(BindingMode mode = BindingMode.OneWay, IValueConverter? converter = null, object? converterParameter = null, string? stringFormat = null)
		=> new Binding(nameof(Value), mode, converter, converterParameter, stringFormat, source: this);

	/// <summary>
	/// Creates a data binding for a value type property with the specified binding mode and optional converter settings.
	/// </summary>
	/// <param name="mode">The binding mode that determines how changes are propagated between the source and target. The default is <see
	/// cref="BindingMode.OneWay"/>.</param>
	/// <param name="converter">An optional value converter used to transform the value between the source and target types during binding.</param>
	/// <param name="converterParameter">An optional parameter passed to the converter to influence its conversion logic.</param>
	/// <param name="stringFormat">An optional string format applied to the value when converting it to a string representation for display.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the configured binding for the value type property.</returns>
	public BindingBase BindValueType(BindingMode mode = BindingMode.OneWay, IValueConverter? converter = null, object? converterParameter = null, string? stringFormat = null)
		=> new Binding(nameof(ValueType), mode, converter, converterParameter, stringFormat, source: this);

	/// <summary>
	/// Creates a data binding that retrieves the value kind of the current expression node.
	/// </summary>
	/// <param name="mode">The binding mode that determines how and when the target property is updated. The default is <see
	/// cref="BindingMode.OneWay"/>.</param>
	/// <param name="converter">An optional value converter used to transform the source value before it is applied to the target property.</param>
	/// <param name="converterParameter">An optional parameter to pass to the value converter, allowing customization of the conversion logic.</param>
	/// <param name="stringFormat">An optional string format to apply to the value before it is set on the target property.</param>
	/// <returns>A <see cref="BindingBase"/> instance that binds to the value kind of the expression node.</returns>
	public BindingBase BindValueKind(BindingMode mode = BindingMode.OneWay, IValueConverter? converter = null, object? converterParameter = null, string? stringFormat = null)
		=> new Binding(nameof(ValueKind), mode, converter, converterParameter, stringFormat, source: this);

	/// <summary>
	/// Creates a data binding that reflects whether the bound value is deterministic.
	/// </summary>
	/// <param name="mode">The binding mode that determines how changes are propagated between the source and target. Defaults to <see
	/// cref="BindingMode.OneWay"/>.</param>
	/// <param name="converter">An optional value converter used to transform the source value to the target value and vice versa during binding.</param>
	/// <param name="converterParameter">An optional parameter to pass to the value converter to influence its conversion logic.</param>
	/// <param name="stringFormat">An optional string format to apply to the bound value when it is displayed as a string.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the configured binding for the deterministic state.</returns>
	public BindingBase BindIsDeterministic(BindingMode mode = BindingMode.OneWay, IValueConverter? converter = null, object? converterParameter = null, string? stringFormat = null)
		=> new Binding(nameof(IsDeterministic), mode, converter, converterParameter, stringFormat, source: this);
}
