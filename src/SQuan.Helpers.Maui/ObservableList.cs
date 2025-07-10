using System.Collections.Specialized;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a collection of items that supports notification of changes to its elements.
/// </summary>
/// <remarks>The <see cref="ObservableList{T}"/> class extends <see cref="List{T}"/> and implements  <see
/// cref="INotifyCollectionChanged"/> to provide change notifications when the collection is modified. This makes it
/// suitable for scenarios where UI elements or other observers need to react to changes  in the collection.</remarks>
/// <typeparam name="T">The type of elements contained in the collection.</typeparam>
public class ObservableList<T> : List<T>, INotifyCollectionChanged
{
	/// <summary>
	/// Raises a collection reset notification to indicate that the entire collection has changed.
	/// </summary>
	/// <remarks>This method triggers a <see cref="NotifyCollectionChangedAction.Reset"/> event, signaling that the
	/// collection  has been cleared or significantly altered. Subscribers to the collection's change notifications should 
	/// refresh their views or data accordingly.</remarks>
	public void InvokeCollectionReset()
	{
		InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	/// <summary>
	/// Raises the <see cref="CollectionChanged"/> event with the specified event data.
	/// </summary>
	/// <remarks>Use this method to notify subscribers of changes to the collection. Ensure that the event data
	/// provided accurately reflects the changes made to the collection.</remarks>
	/// <param name="e">The event data for the <see cref="CollectionChanged"/> event. This must not be <c>null</c>.</param>
	public void InvokeCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		CollectionChanged?.Invoke(this, e);
	}

	/// <summary>
	/// Occurs when the collection is changed, such as when items are added, removed, or replaced.
	/// </summary>
	/// <remarks>This event is typically used to notify subscribers of changes to the collection's contents.
	/// Handlers for this event can inspect the <see cref="NotifyCollectionChangedEventArgs"/> parameter to determine the
	/// nature of the change.</remarks>
	public event NotifyCollectionChangedEventHandler? CollectionChanged;
}
