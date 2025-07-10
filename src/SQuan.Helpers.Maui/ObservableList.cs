using System.Collections.Specialized;
using System.ComponentModel;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a collection of items that supports notification of changes to its elements.
/// </summary>
/// <remarks>The <see cref="ObservableList{T}"/> class extends <see cref="List{T}"/> and implements <see
/// cref="INotifyCollectionChanged"/> to provide change notifications when the collection is modified. This makes it
/// suitable for scenarios where UI elements or other observers need to react to changes in the collection.</remarks>
/// <typeparam name="T">The type of elements contained in the collection.</typeparam>
public class ObservableList<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
	/// <summary>
	/// Adds the elements of the specified collection to the current collection.
	/// </summary>
	/// <remarks>Each element in the specified collection is added individually to the current collection. If the
	/// collection is empty, no elements are added.</remarks>
	/// <param name="collection">The collection whose elements are to be added. Cannot be null.</param>
	public void AddRange(IList<T> collection)
	{
		if (collection is null || collection.Count == 0)
		{
			return;
		}

		foreach (var item in collection)
		{
			Add(item);
		}

		InvokeCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList()));
		InvokePropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
	}

	/// <summary>
	/// Removes all items from the collection and raises the appropriate change notifications.
	/// </summary>
	public new void Clear()
	{
		if (this.Count == 0)
		{
			return;
		}

		base.Clear();
		InvokeCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		InvokePropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
	}

	/// <summary>
	/// Invalidates the current state of the collection, signaling that its contents have changed.
	/// </summary>
	/// <remarks>This method raises both the
	/// <see cref="INotifyCollectionChanged.CollectionChanged"/> event with a <see cref="NotifyCollectionChangedAction.Reset"/> action
	/// and the <see cref="INotifyPropertyChanged.PropertyChanged"/> event with an empty property name.
	/// It is typically used to notify observers that the entire collection should be refreshed.</remarks>
	public void Invalidate()
	{
		InvokeCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		InvokePropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
	}

	/// <summary>
	/// Raises the <see cref="CollectionChanged"/> event with the specified sender and event data.
	/// </summary>
	/// <remarks>This method invokes the <see cref="CollectionChanged"/> event, allowing subscribers to respond to
	/// changes in the collection. Ensure that <paramref name="e"/> is properly initialized with the details of the
	/// collection change before calling this method.</remarks>
	/// <param name="sender">The source of the event. This can be <see langword="null"/> if the sender is not specified.</param>
	/// <param name="e">The event data containing information about the collection change. Must not be <see langword="null"/>.</param>
	public void InvokeCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		CollectionChanged?.Invoke(sender, e);
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event to notify subscribers of a property change.
	/// </summary>
	/// <remarks>This method invokes the <see cref="PropertyChanged"/> event if there are any subscribers. Ensure
	/// that <paramref name="e"/> is not <see langword="null"/> and contains a valid property name.</remarks>
	/// <param name="sender">The source of the event, typically the object whose property has changed. Can be <see langword="null"/>.</param>
	/// <param name="e">An instance of <see cref="PropertyChangedEventArgs"/> containing the name of the property that changed.</param>
	public void InvokePropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		PropertyChanged?.Invoke(sender, e);
	}

	/// <summary>
	/// Occurs when the collection is changed, such as when items are added, removed, or replaced.
	/// </summary>
	/// <remarks>This event is typically used to notify subscribers of changes to the collection's contents.
	/// Handlers for this event can inspect the <see cref="NotifyCollectionChangedEventArgs"/> parameter to determine the
	/// nature of the change.</remarks>
	public event NotifyCollectionChangedEventHandler? CollectionChanged;

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	/// <remarks>This event is typically used to notify subscribers that a property of the object has been updated.
	/// It is commonly used in data-binding scenarios to update UI elements when the underlying data changes.</remarks>
	public event PropertyChangedEventHandler? PropertyChanged;
}
