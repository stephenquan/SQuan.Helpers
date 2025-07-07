using System.ComponentModel;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a wrapper around an <see cref="IDictionary{TKey, TValue}"/> that provides convenient access to its
/// elements and supports additional functionality such as property change notifications.
/// </summary>
/// <remarks>The <see cref="DictionaryWrapper"/> class allows interaction with an underlying dictionary using
/// string keys. It provides indexed access to dictionary elements and supports dynamic updates to the dictionary. This
/// class is particularly useful for scenarios where dictionary manipulation needs to be combined with additional
/// behaviors, such as notifying changes to bound UI elements.</remarks>
public partial class DictionaryWrapper : INotifyPropertyChanged
{
	IDictionary<string, object?>? internalDict { get; set; }

	/// <summary>
	/// // Provides access to the dictionary using string keys.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public object? this[string key]
	{
		get
		{
			if (internalDict is IDictionary<string, object?> dict)
			{
				if (dict.TryGetValue(key, out var value))
				{
					return value;
				}
			}
			return null;
		}
		set
		{
			if (internalDict is IDictionary<string, object?> dict)
			{
				if (value is null)
				{
					if (dict.ContainsKey(key))
					{
						dict.Remove(key);
						OnPropertyChanged($"Item[{key}]");
					}
					return;
				}

				if (dict.ContainsKey(key))
				{
					object? oldValue = dict[key];
					if (oldValue is not null && oldValue.Equals(value))
					{
						return;
					}
					dict[key] = value;
					OnPropertyChanged($"Item[{key}]");
					return;
				}

				dict.Add(key, value);
				OnPropertyChanged($"Item[{key}]");
			}
		}
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event to notify subscribers that a property value has changed.
	/// </summary>
	/// <remarks>This method is typically called within a property's setter to notify bound controls or other
	/// listeners of changes to the property's value.</remarks>
	/// <param name="propertyName">The name of the property that changed. This value can be <see langword="null"/> or empty to indicate that all
	/// properties have changed.</param>
	public void OnPropertyChanged(string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DictionaryWrapper"/> class, wrapping the specified dictionary.
	/// </summary>
	/// <remarks>The <see cref="DictionaryWrapper"/> provides a convenient way to interact with an underlying  <see
	/// cref="IDictionary{TKey, TValue}"/> while potentially adding additional functionality or constraints.</remarks>
	/// <param name="dictionary">The dictionary to wrap. This must not be null.</param>
	public DictionaryWrapper(IDictionary<string, object?> dictionary)
	{
		internalDict = dictionary;

		if (dictionary is INotifyPropertyChanged notifyPropertyChanged)
		{
			notifyPropertyChanged.PropertyChanged += (s, e) =>
			{
				OnPropertyChanged($"Item[{e.PropertyName}]");
			};
		}
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	/// <remarks>This event is typically used to notify subscribers that a property of the object has been updated.
	/// It is commonly implemented in classes that support data binding.</remarks>
	public event PropertyChangedEventHandler? PropertyChanged;
}
