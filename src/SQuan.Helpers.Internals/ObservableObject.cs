// ObservableObject.cs

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SQuan.Helpers.Internals;

/// <summary>
/// Provides a base class that implements the INotifyPropertyChanged interface to support property change notifications.
/// </summary>
public partial class ObservableObject : INotifyPropertyChanged
{
	/// <summary>
	/// Sets the specified property to a new value and raises a property changed notification if the value has changed.
	/// </summary>
	/// <typeparam name="TProperty">The type of the property being set.</typeparam>
	/// <param name="storage">A reference to the field that stores the property's current value. This value will be updated if it differs from
	/// the new value.</param>
	/// <param name="value">The new value to assign to the property.</param>
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and is used for notification purposes.</param>
	/// <returns>true if the property value was changed and the notification was raised; otherwise, false.</returns>
	protected bool SetProperty<TProperty>(ref TProperty storage, TProperty value, [CallerMemberName] string? propertyName = null)
	{
		if (System.Collections.Generic.EqualityComparer<TProperty>.Default.Equals(storage, value))
		{
			return false;
		}
		storage = value;
		OnPropertyChanged(propertyName);
		return true;
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event for the specified property name.
	/// </summary>
	/// <param name="propertyName"></param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;
}
