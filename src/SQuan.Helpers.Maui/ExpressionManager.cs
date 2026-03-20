// ExpressionManager.cs

using System.Collections.Concurrent;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Manages a graph of <see cref="ExpressionNode"/> instances and evaluates expressions
/// asynchronously using a background calculation loop.
/// The manager maintains dependency relationships between nodes, propagates value changes,
/// and ensures calculations are performed in the correct order.
/// Results and notifications are marshalled back to the UI thread when required.
/// </summary>
public partial class ExpressionManager : INotifyPropertyChanged, IDisposable
{
	/// <summary>
	/// Optional logger used for diagnostics, tracing, and error reporting.
	/// </summary>
	public static ILogger? Logger { get; } = IPlatformApplication.Current?.Services.GetService<ILogger<ExpressionManager>>();

	/// <summary>
	/// Parser plugin defining supported operators, functions, and syntax rules.
	/// </summary>
	public readonly ExpressionParserPlugin ParserPlugin = new();

	readonly ConcurrentDictionary<string, ExpressionNode> dictionary = new();
	readonly BlockingCollection<ExpressionNode> pendingCalculations = new();
	readonly ConcurrentDictionary<ExpressionNode, byte> queuedNodes = new();
	readonly WeakEventManager propertyChangedEventManager = new();

	sealed partial class QuitNode : ExpressionNode
	{
		public long RunId { get; }
		public QuitNode(long runId) : base()
		{
			RunId = runId;
		}
	}

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
		get => GetOrAddNode(nodeRef).InternalValue;
		set => SetValue(nodeRef, value, ExpressionValueKind.UserInput);
	}

	/// <summary>
	/// Gets the value of a node, cast to the specified type.
	/// </summary>
	/// <typeparam name="T">The expected value type.</typeparam>
	/// <param name="nodeRef">The node reference.</param>
	/// <returns>
	/// The value if present and compatible; otherwise the default value of <typeparamref name="T"/>.
	/// </returns>
	public T GetValue<T>(string nodeRef)
		=> (GetValue(nodeRef) is T value) ? value : default!;

	/// <summary>
	/// Gets the raw value of a node.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	/// <returns>The current value, or null if the node does not exist.</returns>
	public object? GetValue(string nodeRef)
		=> TryGetValueInfo(nodeRef, out var valueInfo) ? valueInfo?.InternalValue : null;

	/// <summary>
	/// Attempts to retrieve the <see cref="ExpressionNode"/> associated with a reference.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="valueInfo">When this method returns, contains the node if found; otherwise null.</param>
	/// <returns>True if the node exists; otherwise false.</returns>
	public bool TryGetValueInfo(string nodeRef, out ExpressionNode? valueInfo)
		=> dictionary.TryGetValue(nodeRef, out valueInfo);

	/// <summary>
	/// Retrieves an existing node or creates a new one if it does not exist.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="valueType">Optional value type constraint.</param>
	/// <param name="expression">Optional expression.</param>
	/// <returns>The existing or newly created node.</returns>
	public ExpressionNode GetOrAddNode(string nodeRef, Type? valueType = null, string? expression = null)
		=> dictionary.GetOrAdd(nodeRef, nodeRef => new ExpressionNode
		{
			Owner = this,
			NodeRef = nodeRef,
			ValueKind = ExpressionValueKind.Uninitialized,
			Expression = expression ?? string.Empty,
			ValueType = valueType,
			IsDeterministic = false
		});

	/// <summary>
	/// Sets the value of a node with an explicit value type.
	/// </summary>
	/// <typeparam name="T">The expected value type.</typeparam>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="value">The value to assign.</param>
	/// <param name="reason">The reason for the value change.</param>
	/// <returns>The affected node.</returns>
	public ExpressionNode SetValue<T>(string nodeRef, object? value, ExpressionValueKind reason = ExpressionValueKind.Default)
		=> SetValue(nodeRef, value, reason, typeof(T?));

	/// <summary>
	/// Sets the value of a node and propagates changes to dependent expressions.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="value">The value to assign.</param>
	/// <param name="valueKind">The reason for the value change.</param>
	/// <param name="valueType">Optional value type constraint.</param>
	/// <param name="isDeterministic">Indicates whether the value is deterministic.</param>
	/// <param name="trace">Whether to emit trace logging.</param>
	/// <returns>The affected node.</returns>
	public ExpressionNode SetValue(string nodeRef, object? value, ExpressionValueKind valueKind = ExpressionValueKind.Default, Type? valueType = default, bool isDeterministic = true, bool trace = true)
	{
		bool modified = false;
		var node = dictionary.GetOrAdd(nodeRef, nodeRef =>
		{
			modified = true;
			return new ExpressionNode
			{
				Owner = this,
				NodeRef = nodeRef,
				ValueKind = valueKind,
				ValueType = valueType,
				IsDeterministic = isDeterministic,
			};
		});
		node.ValueKind = valueKind;
		node.IsDeterministic = isDeterministic;
		RemoveDependencies(node);
		if (valueType is not null && node.ValueType != valueType)
		{
			node.ValueType = valueType;
			modified = true;
		}
		if (value.TryConvert(node.ValueType, out var coerceValue))
		{
			value = coerceValue;
		}
		if (!modified && value is null && node.InternalValue is null)
		{
			return node;
		}
		if (!modified && value is not null && value.Equals(node.InternalValue))
		{
			return node;
		}

		Logger?.LogTrace("SetValue {NodeRef} to {Value} (type={ValueType}, valueKind={ValueKind}, isDeterministic={IsDeterministic})", nodeRef, value, valueType, valueKind, isDeterministic);

		node.SetInternalValue(value);

		NotifyValueChanged(nodeRef);

		return node;
	}


	/// <summary>
	/// Notifies listeners that a node value has changed and schedules dependent recalculations.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	public void NotifyValueChanged(string nodeRef)
	{
		if (dictionary.TryGetValue(nodeRef, out var _node) && _node is ExpressionNode node)
		{
			InvokeOnUIThread(() =>
			{
				propertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs($"Item[{nodeRef}]"), nameof(PropertyChanged));
				node.OnValueChanged();
			});

			foreach (var outputKey in node.OutputNodeRefs.Keys)
			{
				RecalculateByNodeRef(outputKey);
			}
		}
	}


	/// <summary>
	/// Assigns an expression to a node with an explicit result type.
	/// </summary>
	/// <typeparam name="T">The expected result type.</typeparam>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="expression">The expression text.</param>
	/// <returns>The affected node.</returns>
	public ExpressionNode SetExpression<T>(string nodeRef, string expression)
		=> SetExpression(nodeRef, expression, typeof(T?));


	/// <summary>
	/// Assigns an expression to a node and enqueues it for calculation.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	/// <param name="expression">The expression text.</param>
	/// <param name="valueType">Optional result type constraint.</param>
	/// <returns>The affected node.</returns>
	public ExpressionNode SetExpression(string nodeRef, string expression, Type? valueType = default)
	{
		var node = GetOrAddNode(nodeRef);
		node.ValueKind = ExpressionValueKind.Uninitialized;
		if (valueType is not null && node.ValueType != valueType)
		{
			node.ValueType = valueType;
		}
		node.Expression = expression;

		RemoveDependencies(node);

		if (queuedNodes.TryAdd(node, 0))
		{
			pendingCalculations.Add(node);
			Logger?.LogTrace("SetExpression {NodeRef} to {Expression} (type={ValueType})", nodeRef, expression, valueType);
		}

		return node;
	}

	/// <summary>
	/// Enqueues a node for recalculation by reference.
	/// </summary>
	/// <param name="nodeRef">The node reference.</param>
	public void RecalculateByNodeRef(string nodeRef)
	{
		if (dictionary.TryGetValue(nodeRef, out var node))
		{
			RecalculateNode(node);
		}
	}

	/// <summary>
	/// Enqueues a specific node for recalculation.
	/// </summary>
	/// <param name="node">The node to recalculate.</param>
	public void RecalculateNode(ExpressionNode node)
	{
		if (queuedNodes.TryAdd(node, 0))
		{
			pendingCalculations.Add(node);
		}
	}

	/// <summary>
	/// Starts the background calculation loop.
	/// </summary>
	/// <param name="ct">A cancellation token used to stop the loop.</param>
	public void StartCalculationLoop(CancellationToken ct)
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
						var node = pendingCalculations.Take(ct);
						queuedNodes.TryRemove(node, out _);

						if (node is QuitNode quitNode)
						{
							if (quitNode.RunId != currentRunId)
							{
								Logger?.LogDebug(
									"Ignoring stale QuitNode (runId={QuitRunId}, currentRunId={CurrentRunId})",
									quitNode.RunId,
									currentRunId);
								continue;
							}

							isRunning = false;
							break;
						}

						switch (node.ValueKind)
						{
							case ExpressionValueKind.Uninitialized:
								if (!node.Initialize(ParserPlugin))
								{
									Logger?.LogError("Failed to initialize {NodeRef}'s expression {Expression}", node.NodeRef, node.Expression);
									continue;
								}
								UpdateDependencyGraph(node);
								if (node.Calculate(GetValue, ct))
								{
									NotifyValueChanged(node.NodeRef);
								}
								break;
							case ExpressionValueKind.Calculated:
							case ExpressionValueKind.PendingCalculation:
								if (node.Calculate(GetValue, ct))
								{
									NotifyValueChanged(node.NodeRef);
								}
								break;
							case ExpressionValueKind.ParseError:
							case ExpressionValueKind.CalculateError:
							default:
								// Do nothing, won't be re-enqueued
								Logger?.LogWarning("Node {NodeRef} is in error state ({ValueKind}), skipping calculation", node.NodeRef, node.ValueKind);
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
		pendingCalculations.Add(new QuitNode(currentRunId));
		await task;
		runningTask = null;
	}

	void UpdateDependencyGraph(ExpressionNode node)
	{
		foreach (var inputNodeRefs in node.InputNodeRefs.Keys)
		{
			var inputNode = dictionary.GetOrAdd(inputNodeRefs, nodeRef => new ExpressionNode
			{
				Owner = this,
				NodeRef = nodeRef,
				ValueKind = ExpressionValueKind.Uninitialized,
				ValueType = null,
				IsDeterministic = false
			});
			inputNode.OutputNodeRefs.TryAdd(node.NodeRef, 0);
		}
	}

	void RemoveDependencies(ExpressionNode node)
	{
		foreach (var inputNodeRefs in node.InputNodeRefs.Keys)
		{
			if (dictionary.TryGetValue(inputNodeRefs, out var inputNode))
			{
				Logger?.LogTrace("RemoveDependencies {InputNodeRef} and {NodeRef}", inputNode.NodeRef, node.NodeRef);
				inputNode.OutputNodeRefs.TryRemove(node.NodeRef, out _);
			}
		}

		node.InputNodeRefs.Clear();
	}

	/// <summary>
	/// Releases managed resources used by this instance.
	/// </summary>
	/// <param name="disposing">True when called from Dispose.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			pendingCalculations.Dispose();
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
		while (pendingCalculations.TryTake(out _))
		{
		}

		foreach (var node in dictionary.Values)
		{
			node.Clear();
		}
		dictionary.Clear();
	}
}
