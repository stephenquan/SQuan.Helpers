// ExpressionManager.cs

using System.Collections.Concurrent;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Manages a graph of <see cref="ExpressionNode{T}"/> instances and evaluates expressions
/// asynchronously using a background calculation loop.
/// The manager maintains dependency relationships between nodes, propagates value changes,
/// and ensures calculations are performed in the correct order.
/// Results and notifications are marshalled back to the UI thread when required.
/// </summary>
public partial class ExpressionManager : INotifyPropertyChanged, IDisposable
{
	static ILogger? logger;

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
	/// Parser plugin defining supported operators, functions, and syntax rules.
	/// </summary>
	public readonly ExpressionParserPlugin ParserPlugin = new();

	readonly ConcurrentDictionary<string, IExpressionNode> expressions = new();
	readonly BlockingCollection<IExpressionWorkItem> queuedWork = new();
	readonly ConcurrentDictionary<IExpressionNode, byte> queuedNodes = new();
	readonly WeakEventManager propertyChangedEventManager = new();

	volatile bool isRunning = false;
	long runId = 0;
	long currentRunId = 0;
	Task? runningTask;

	/// <summary>
	/// Raised when a value exposed by this manager changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged
	{
		add => propertyChangedEventManager.AddEventHandler(value);
		remove => propertyChangedEventManager.RemoveEventHandler(value);
	}

	Action<Action>? invokeOnUIThread;

	/// <summary>
	/// Configures a callback used to marshal work onto the UI thread.
	/// </summary>
	/// <param name="invokeOnUIThread">A delegate that executes the supplied action on the UI thread.</param>
	public void SetInvokeOnUIThread(Action<Action> invokeOnUIThread)
	{
		this.invokeOnUIThread = invokeOnUIThread;
	}

	/// <summary>
	/// Configures UI-thread marshalling using a MAUI <see cref="IDispatcher"/>.
	/// </summary>
	/// <param name="dispatcher">The dispatcher used to invoke actions.</param>
	public void SetInvokeOnUIThread(IDispatcher dispatcher)
	{
		this.invokeOnUIThread = action =>
		{
			if (!dispatcher.Dispatch(action))
			{
				action();
			}
		};
	}

	/// <summary>
	/// Executes an action on the UI thread if a dispatcher has been configured;
	/// otherwise executes it immediately on the current thread.
	/// </summary>
	/// <param name="action">The action to execute.</param>
	public void InvokeOnUIThread(Action action)
	{
		if (invokeOnUIThread is not null)
		{
			invokeOnUIThread(action);
		}
		else
		{
			action();
		}
	}

	/// <summary>
	/// Gets or sets the value of a node by reference.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	public object? this[string nodeRef]
	{
		get
		{
			if (expressions.TryGetValue(nodeRef, out var node) && node is IExpressionNode _node)
			{
				return _node.GetInternalValue();
			}
			return null;
		}
		set
		{
			if (expressions.TryGetValue(nodeRef, out var node) && node is IExpressionNode _node)
			{
				IExpressionNode? _ = _node switch
				{
					ExpressionNode<int> nodeI => SetValue(nodeI, value, ExpressionValueKind.UserInput),
					ExpressionNode<long> nodeL => SetValue(nodeL, value, ExpressionValueKind.UserInput),
					ExpressionNode<double> nodeD => SetValue(nodeD, value, ExpressionValueKind.UserInput),
					ExpressionNode<string> nodeS => SetValue(nodeS, value, ExpressionValueKind.UserInput),
					_ => null
				};
			}
		}
	}

	/// <summary>
	/// Gets the value of a node, cast to the specified type.
	/// </summary>
	/// <typeparam name="T">The expected value type.</typeparam>
	/// <param name="nodeRef">The node reference.</param>
	/// <returns>
	/// The value if present and compatible; otherwise the default value of <typeparamref name="T"/>.
	/// </returns>
	public T? GetValue<T>(string nodeRef)
		=> (GetValue(nodeRef) is T value) ? value : default;

	/// <summary>
	/// Gets the raw value of a node.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	/// <returns>The current value, or null if the node does not exist.</returns>
	public object? GetValue(string nodeRef)
	{
		if (expressions.TryGetValue(nodeRef, out var node) && node is IExpressionNode _node)
		{
			return _node.GetInternalValue();
		}
		return null;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="ExpressionNode{T}"/> class associated with the specified node reference.
	/// </summary>
	/// <param name="nodeRef">The unique identifier for the node to create. Cannot be null.</param>
	/// <returns>A new <see cref="ExpressionNode{T}"/> instance associated with the specified node reference.</returns>
	public ExpressionNode<T> CreateNode<T>(string nodeRef)
	{
		if (expressions.TryGetValue(nodeRef, out var _node) && _node is ExpressionNode<T> __node)
		{
			__node.Clear();
		}

		var node = new ExpressionNode<T>
		{
			Owner = this,
			NodeRef = nodeRef,
			ValueType = typeof(T),
		};
		expressions[nodeRef] = node;
		return node;
	}

	ExpressionNode<T> GetOrCreateNode<T>(string nodeRef)
	{
		if (expressions.TryGetValue(nodeRef, out var _node) && _node is ExpressionNode<T> __node)
		{
			return __node;
		}

		return CreateNode<T>(nodeRef);
	}

	/// <summary>
	/// Sets the value of a node with an explicit value type.
	/// </summary>
	/// <typeparam name="T">The expected value type.</typeparam>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="value">The value to assign.</param>
	/// <param name="kind">The reason for the value change.</param>
	/// <returns>The affected node.</returns>
	public ExpressionNode<T> SetValue<T>(string nodeRef, object? value, ExpressionValueKind kind = ExpressionValueKind.Default)
		=> SetValue<T>(GetOrCreateNode<T>(nodeRef), value, kind);

	/// <summary>
	/// Sets the value of a node with an explicit value type.
	/// </summary>
	/// <typeparam name="T">The expected value type.</typeparam>
	/// <param name="node">The node to update.</param>
	/// <param name="value">The value to assign.</param>
	/// <param name="kind">The reason for the value change.</param>
	/// <returns>The updated node.</returns>
	public ExpressionNode<T> SetValue<T>(ExpressionNode<T> node, object? value, ExpressionValueKind kind = ExpressionValueKind.Default)
	{
		if (node.ValueKind == ExpressionValueKind.PendingParsing)
		{
			return node;
		}

		if (node.SetInternalValue(value))
		{
			node.ValueKind = kind;
			Logger?.LogTrace("SetValue {NodeRef} to {Value} (type={ValueType}, valueKind={ValueKind})", node.NodeRef, value, node.ValueType, kind);
			InvokeOnUIThread(() =>
			{
				node.OnValueChanged();
				propertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs($"Item[{node.NodeRef}]"), nameof(PropertyChanged));
			});

			foreach (var outputKey in node.OutputNodeRefs.Keys)
			{
				if (expressions.TryGetValue(outputKey, out var outputNode) && outputNode is IExpressionNode _outputNode)
				{
					//Logger?.LogTrace("Notifying output node {OutputNodeRef} of change in {NodeRef}", outputKey, node.NodeRef);
					Recalculate(_outputNode);
				}
			}
		}
		return node;
	}

	/// <summary>
	/// Assigns an expression to a node with an explicit result type.
	/// </summary>
	/// <typeparam name="T">The expected result type.</typeparam>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="expression">The expression text.</param>
	/// <returns>The affected node.</returns>
	public ExpressionNode<T> SetExpression<T>(string nodeRef, string expression)
		=> SetExpression<T>(GetOrCreateNode<T>(nodeRef), expression);

	ExpressionNode<T> SetExpression<T>(ExpressionNode<T> node, string expression)
	{
		Logger?.LogTrace("SetExpression {NodeRef} to {Expression} (type={ValueType})", node.NodeRef, expression, typeof(T));
		node.Expression = expression;
		node.ValueKind = ExpressionValueKind.PendingParsing;
		if (queuedNodes.TryAdd(node, 0))
		{
			queuedWork.Add(new ExpressionWorkItemInitialize { Node = node });
		}
		return node;
	}

	/// <summary>
	/// Assigns a default expression to a node.
	/// </summary>
	/// <typeparam name="T">The node's value type.</typeparam>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="expression">The default expression text.</param>
	/// <returns>The affected node.</returns>
	public ExpressionNode<T> SetDefault<T>(string nodeRef, string expression)
		=> SetDefault<T>(GetOrCreateNode<T>(nodeRef), expression);

	ExpressionNode<T> SetDefault<T>(ExpressionNode<T> node, string expression)
	{
		Logger?.LogTrace("SetDefault {NodeRef} to {Expression} (type={ValueType})", node.NodeRef, expression, typeof(T));
		node.DefaultExpression = expression;
		node.ValueKind = ExpressionValueKind.PendingParsing;
		if (queuedNodes.TryAdd(node, 0))
		{
			queuedWork.Add(new ExpressionWorkItemInitialize { Node = node });
		}
		return node;
	}

	/// <summary>
	/// Enqueues a node for recalculation by reference.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	public void Recalculate(string nodeRef)
	{
		if (expressions.TryGetValue(nodeRef, out var node) && node is IExpressionNode _node)
		{
			Recalculate(_node);
		}
	}

	/// <summary>
	/// Enqueues a specific node for recalculation.
	/// </summary>
	/// <param name="node">The node to recalculate.</param>
	public void Recalculate(IExpressionNode node)
	{
		switch (node.ValueKind)
		{
			case ExpressionValueKind.Uninitialized:
			case ExpressionValueKind.PendingParsing:
				node.ValueKind = ExpressionValueKind.PendingParsing;
				break;
			case ExpressionValueKind.UserInput:
			case ExpressionValueKind.PendingReset:
			case ExpressionValueKind.Default:
			case ExpressionValueKind.Calculated:
			case ExpressionValueKind.PendingCalculation:
				node.ValueKind = ExpressionValueKind.PendingCalculation;
				break;
			case ExpressionValueKind.ParseError:
			case ExpressionValueKind.CalculateError:
			default:
				// Do not enqueue nodes in error state
				Logger?.LogWarning("Node {NodeRef} is in error state ({ValueKind}), skipping recalculation", node.NodeRef, node.ValueKind);
				return;
		}

		if (queuedNodes.TryAdd(node, 0))
		{
			queuedWork.Add(new ExpressionWorkItemCalculate { Node = node });
		}
	}

	/// <summary>
	/// Resets the specified expression node to a pending reset state if it is in a valid state.
	/// </summary>
	/// <param name="nodeRef">The expression node to reset. This node must not be in an error state to be reset successfully.</param>
	public void ResetToDefault(string nodeRef)
	{
		if (expressions.TryGetValue(nodeRef, out var node) && node is IExpressionNode _node)
		{
			ResetToDefault(_node);
		}
	}

	/// <summary>
	/// Resets the specified expression node to a pending reset state if it is in a valid state.
	/// </summary>
	/// <param name="node">The expression node to reset. This node must not be in an error state to be reset successfully.</param>
	public void ResetToDefault(IExpressionNode node)
	{
		switch (node.ValueKind)
		{
			case ExpressionValueKind.Uninitialized:
			case ExpressionValueKind.PendingParsing:
				break;
			case ExpressionValueKind.UserInput:
			case ExpressionValueKind.PendingReset:
			case ExpressionValueKind.Default:
			case ExpressionValueKind.Calculated:
			case ExpressionValueKind.PendingCalculation:
				node.ValueKind = ExpressionValueKind.PendingReset;
				break;
			case ExpressionValueKind.ParseError:
			case ExpressionValueKind.CalculateError:
			default:
				// Do not enqueue nodes in error state
				Logger?.LogWarning("Node {NodeRef} is in error state ({ValueKind}), skipping reset", node.NodeRef, node.ValueKind);
				return;
		}

		if (queuedNodes.TryAdd(node, 0))
		{
			queuedWork.Add(new ExpressionWorkItemReset { Node = node });
		}
	}

	/// <summary>
	/// Starts the background calculation loop.
	/// </summary>
	/// <param name="ct">A cancellation token used to stop the loop.</param>
	public void StartWorkLoop(CancellationToken ct)
	{
		if (isRunning || runningTask is not null)
		{
			return;
		}

		isRunning = true;
		currentRunId = Interlocked.Increment(ref runId);

		runningTask = Task.Run(() =>
		{
			Logger?.LogTrace("Calculation loop started (worker entered, currentRunId={CurrentRunId})", currentRunId);
			try
			{
				while (isRunning && !ct.IsCancellationRequested)
				{
					try
					{
						var workItem = queuedWork.Take(ct);
						if (workItem is ExpressionWorkItemNode workItemNode)
						{
							queuedNodes.TryRemove(workItemNode.Node, out _);
						}

						if (workItem is ExpressionWorkItemInitialize init)
						{
							_ = InitializeNow(init.Node, ct);
							continue;
						}

						if (workItem is ExpressionWorkItemCalculate calculate)
						{
							_ = CalculateNow(calculate.Node, ct);
							continue;
						}

						if (workItem is ExpressionWorkItemReset reset)
						{
							_ = ResetNow(reset.Node, ct);
							continue;
						}

						if (workItem is ExpressionWorkItemQuit quit)
						{
							if (quit.RunId != currentRunId)
							{
								Logger?.LogDebug(
									"Ignoring stale ExpressionWorkItemQuit (runId={QuitRunId}, currentRunId={CurrentRunId})",
									quit.RunId,
									currentRunId);
								continue;
							}

							isRunning = false;
							break;
						}
					}
					catch (OperationCanceledException)
					{
						break;
					}
				}
			}
			finally
			{
				isRunning = false;
				runningTask = null;
				Logger?.LogTrace("Calculation loop exited (worker leaving, currentRunId={CurrentRunId})", currentRunId);
			}
		}, ct);
	}

	bool InitializeNow(IExpressionNode node, CancellationToken ct)
	{
		var parser = new ExpressionParser(ParserPlugin);
		if (!string.IsNullOrEmpty(node.DefaultExpression))
		{
			if (!parser.TryParse(node.NodeRef, node.DefaultExpression))
			{
				Logger?.LogError("Failed to initialize {NodeRef}'s default expression {DefaultExpression}", node.NodeRef, node.DefaultExpression);
				node.ValueKind = ExpressionValueKind.ParseError;
				return false;
			}
			node.SetDefaultExpressionTokens(parser.Tokens);
		}
		if (!string.IsNullOrEmpty(node.Expression))
		{
			if (!parser.TryParse(node.NodeRef, node.Expression))
			{
				Logger?.LogError("Failed to initialize {NodeRef}'s expression {Expression}", node.NodeRef, node.Expression);
				node.ValueKind = ExpressionValueKind.ParseError;
				return false;
			}
			node.SetTokens(parser.Tokens);
			UpdateDependencyGraph(node);
		}

		return CalculateNow(node, ct);
	}

	bool CalculateNow(IExpressionNode node, CancellationToken ct)
	{
		switch (node.ValueKind)
		{
			case ExpressionValueKind.ParseError:
			case ExpressionValueKind.CalculateError:
				return false;
		}

		if (node.Tokens.Count > 0)
		{
			if (node.Tokens.TryEvaluate(GetValue, ct, out var isDeterministic, out var value))
			{
				node.IsDeterministic = isDeterministic;
				//Logger?.LogTrace($"[X] Recalculate {node.NodeRef} as {value}");
				if (node.SetInternalValue(value))
				{
					node.ValueKind = ExpressionValueKind.Calculated;
					Logger?.LogTrace("Recalculated {NodeRef} to {Value} (type={ValueType}, valueKind={ValueKind})", node.NodeRef, value, node.ValueType, node.ValueKind);
					InvokeOnUIThread(() =>
					{
						node.OnValueChanged();
						propertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs($"Item[{node.NodeRef}]"), nameof(PropertyChanged));
					});
				}
				else
				{
					node.ValueKind = ExpressionValueKind.Calculated;
				}
			}
		}
		else
		{
			node.ValueKind = ExpressionValueKind.Calculated;
		}
		return true;
	}

	bool ResetNow(IExpressionNode node, CancellationToken ct)
	{
		switch (node.ValueKind)
		{
			case ExpressionValueKind.ParseError:
			case ExpressionValueKind.CalculateError:
				return false;
		}

		if (node.DefaultExpressionTokens.Count > 0)
		{
			if (node.DefaultExpressionTokens.TryEvaluate(GetValue, ct, out var isDeterministic, out var value))
			{
				//Logger?.LogTrace($"Reset {node.NodeRef} to {value}");
				if (node.SetInternalValue(value))
				{
					node.ValueKind = ExpressionValueKind.Default;
					node.IsDeterministic = isDeterministic;
					Logger?.LogTrace("Reseted {NodeRef} to {Value} (type={ValueType}, valueKind={ValueKind})", node.NodeRef, value, node.ValueType, node.ValueKind);
					InvokeOnUIThread(() =>
					{
						node.OnValueChanged();
						propertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs($"Item[{node.NodeRef}]"), nameof(PropertyChanged));
					});
				}
				else
				{
					node.ValueKind = ExpressionValueKind.Default;
				}
			}
		}
		else
		{
			node.ValueKind = ExpressionValueKind.Default;
		}

		return true;
	}

	/// <summary>
	/// Stops the background calculation loop and waits for it to exit.
	/// </summary>
	public async Task StopCalculationLoopAsync()
	{
		var task = runningTask;
		if (task is null || task.IsCompleted)
		{
			return;
		}

		isRunning = false;
		queuedWork.Add(new ExpressionWorkItemQuit { RunId = currentRunId });
		await task;
		runningTask = null;
	}

	void UpdateDependencyGraph(IExpressionNode node)
	{
		foreach (var inputNodeRef in node.GetInputNodeRefs())
		{
			if (expressions.TryGetValue(inputNodeRef, out var inputNode) && inputNode is not null)
			{
				inputNode.TryAddOutputNodeRef(node.NodeRef);
				//Logger?.LogTrace($"Setting ${inputNodeRef} as a dependency to {node.NodeRef}");
			}
		}
	}

	/// <summary>
	/// Releases managed resources used by this instance.
	/// </summary>
	/// <param name="disposing">True when called from Dispose.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (isRunning)
			{
				// Ensure the background calculation loop is stopped before disposing the collection
				StopCalculationLoopAsync().GetAwaiter().GetResult();
			}
			queuedWork.Dispose();
		}
	}

	/// <summary>
	/// Disposes the manager and suppresses finalization.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Clears all nodes, pending calculations, and dependency state.
	/// </summary>
	public async Task ClearAsync()
	{
		if (isRunning)
		{
			await StopCalculationLoopAsync().ConfigureAwait(false);
		}
		queuedNodes.Clear();
		while (queuedWork.TryTake(out _))
		{
		}

		foreach (var node in expressions.Values)
		{
			node.Clear();
		}
		expressions.Clear();
	}
}
