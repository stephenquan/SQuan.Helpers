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

		int startIndex = Count;

		foreach (var item in collection)
		{
			base.Add(item);
		}

		OnCollectionAdd(collection, startIndex);
		OnCountChanged();
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
		OnCollectionReset();
		OnPropertyChanged(string.Empty);
	}

	/// <summary>
	/// Raises the <see cref="CollectionChanged"/> event to notify subscribers that the collection has been reset.
	/// </summary>
	/// <remarks>This method triggers a reset action, indicating that the entire collection has changed. Subscribers
	/// should handle this event to update their state accordingly.</remarks>
	public void OnCollectionReset()
	{
		CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	/// <summary>
	/// Handles the addition of items to the collection and raises the <see cref="CollectionChanged"/> event.
	/// </summary>
	/// <remarks>This method invokes the <see cref="CollectionChanged"/> event with an <see
	/// cref="NotifyCollectionChangedEventArgs"/>  indicating the <see cref="NotifyCollectionChangedAction.Add"/> action.
	/// If <paramref name="changedItems"/> is <see langword="null"/> or empty, the method does nothing.</remarks>
	/// <param name="changedItems">The list of items that were added to the collection. Cannot be <see langword="null"/> and must contain at least one
	/// item.</param>
	/// <param name="startIndex">The index in the collection at which the items were added.</param>
	public void OnCollectionAdd(IList<T> changedItems, int startIndex)
	{
		if (changedItems is null || changedItems.Count == 0)
		{
			return;
		}

		CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (System.Collections.IList)changedItems, startIndex));
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event to notify subscribers that a property value has changed.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This value can be <see langword="null"/> or empty to indicate that all
	/// properties have changed.</param>
	public void OnPropertyChanged(string? propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// Notifies listeners that the value of the Count property has changed.
	/// </summary>
	/// <remarks>This method raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the
	/// Count property.</remarks>
	public void OnCountChanged()
	{
		OnPropertyChanged(nameof(Count));
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
