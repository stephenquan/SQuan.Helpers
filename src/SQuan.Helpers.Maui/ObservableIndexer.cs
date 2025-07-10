using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text.Json.Serialization;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Providers an indexer wrapper for a dictionary that supports dynamic member access and property change notifications.
/// </summary>
/// <remarks>The <see cref="ObservableIndexer"/> class provides functionality for dynamically binding to
/// dictionary structures and raising property change notifications when values are added, updated, or removed. It
/// supports dynamic member access and implements <see cref="IDictionary{TKey, TValue}"/> for standard dictionary
/// operations.  This class is particularly useful in scenarios where dynamic data binding is required, such as in UI
/// frameworks that rely on <see cref="INotifyPropertyChanged"/> for updating bound controls.</remarks>
public partial class ObservableIndexer : DynamicObject, IDictionary<string, object?>, INotifyPropertyChanged
{
	IDictionary<string, object?> internalDict { get; set; }

	/// <summary>
	/// Retrieves the value associated with the specified key.
	/// </summary>
	/// <param name="key">The key whose associated value is to be retrieved. Cannot be null.</param>
	/// <returns>The value associated with the specified key, or <see langword="null"/> if the key does not exist or the dictionary
	/// is not initialized.</returns>
	public object? GetValue(string key)
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

	/// <summary>
	/// Sets the value associated with the specified key in the dictionary.
	/// </summary>
	/// <remarks>If the specified <paramref name="value"/> is <see langword="null"/>, the key is removed from the
	/// dictionary if it exists. If the key already exists and the new value is equal to the current value, no changes are
	/// made. Otherwise, the key-value pair is added or updated in the dictionary, and a property change notification is
	/// raised.</remarks>
	/// <param name="key">The key whose value is to be set. Cannot be <see langword="null"/> or empty.</param>
	/// <param name="value">The value to associate with the specified key. If <see langword="null"/>, the key will be removed from the
	/// dictionary.</param>
	public void SetValue(string key, object? value)
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
	/// <summary>
	/// // Provides access to the dictionary using string keys.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public object? this[string key]
	{
		get => GetValue(key);
		set => SetValue(key, value);
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
	/// Initializes a new instance of the <see cref="ObservableIndexer"/> class, allowing dynamic binding to nested
	/// dictionary structures.
	/// </summary>
	/// <remarks>This class supports dynamic binding to dictionary structures and raises property change
	/// notifications for nested dictionaries that implement <see cref="INotifyPropertyChanged"/>. If the <paramref
	/// name="path"/> parameter is provided, the constructor attempts to traverse the dictionary hierarchy using the keys
	/// specified in the path. If a key does not exist or the value is not a dictionary, the traversal stops.</remarks>
	/// <param name="dictionary">The root dictionary containing key-value pairs to bind to. Cannot be null.</param>
	/// <param name="path">An optional dot-separated string representing the path to a nested dictionary within the root dictionary. If
	/// specified, the constructor navigates through the keys in the path to locate the target dictionary.</param>
	public ObservableIndexer(IDictionary<string, object?> dictionary, string? path = null)
	{
		string prefix = "";
		string delimiter = "";
		if (path is string _path)
		{
			var keys = _path.Split(".");
			foreach (var key in keys)
			{
				if (!dictionary.TryGetValue(key, out var value) || value is not IDictionary<string, object?> subDict)
				{
					throw new ArgumentException($"Key '{prefix}{delimiter}{key}' does not exist or is not a dictionary");
				}
				dictionary = subDict;
				prefix += delimiter + key;
				delimiter = ".";
			}
		}

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
	/// Attempts to retrieve the value of a dynamic member by name.
	/// </summary>
	/// <remarks>This method is typically used in dynamic object scenarios to resolve member access at runtime. The
	/// method always returns <see langword="true"/>, but the caller should inspect the <paramref name="result"/>  to
	/// determine whether the requested member exists or has a valid value.</remarks>
	/// <param name="binder">Provides information about the dynamic member being accessed, including its name.</param>
	/// <param name="result">When the method returns, contains the value of the requested member if found; otherwise, <see langword="null"/>.</param>
	/// <returns><see langword="true"/> if the member value was successfully retrieved; otherwise, <see langword="false"/>.</returns>
	public override bool TryGetMember(GetMemberBinder binder, out object? result)
	{
		result = GetValue(binder.Name);
		return true;
	}

	/// <summary>
	/// Attempts to set the value of a dynamic member with the specified name.
	/// </summary>
	/// <remarks>This method is typically used in dynamic object scenarios to intercept and handle member
	/// assignment. The member name is provided by the <paramref name="binder"/> parameter, and the value is assigned using
	/// the specified <paramref name="value"/>.</remarks>
	/// <param name="binder">Provides information about the dynamic member being set, including its name.</param>
	/// <param name="value">The value to assign to the dynamic member.</param>
	/// <returns>Always returns <see langword="true"/>, indicating that the operation was successful.</returns>
	public override bool TrySetMember(SetMemberBinder binder, object? value)
	{
		SetValue(binder.Name, value);
		return true;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	/// <remarks>This event is typically used to notify subscribers that a property of the object has been updated.
	/// It is commonly implemented in classes that support data binding.</remarks>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void Add(string key, object? value)
	{
		internalDict.Add(key, value);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public bool ContainsKey(string key)
		=> internalDict.ContainsKey(key);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public bool Remove(string key)
		=> internalDict.Remove(key);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
		=> internalDict.TryGetValue(key, out value);

	/// <summary>
	/// 
	/// </summary>
	public ICollection<string> Keys
		=> internalDict.Keys;

	/// <summary>
	/// 
	/// </summary>
	public ICollection<object?> Values
		=> internalDict.Values;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="item"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void Add(KeyValuePair<string, object?> item)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	public void Clear()
		=> internalDict.Clear();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public bool Contains(KeyValuePair<string, object?> item)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="array"></param>
	/// <param name="arrayIndex"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// 
	/// </summary>
	public bool Remove(KeyValuePair<string, object?> item)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// 
	/// </summary>
	[JsonIgnore]
	public int Count
	{
		get => GetValue(nameof(Count)) is int count ? count : 0;
		set => SetValue(nameof(Count), value);
	}

	/// <summary>
	/// 
	/// </summary>
	public bool IsReadOnly
		=> false;

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
		=> internalDict.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
		=> GetEnumerator();
}
