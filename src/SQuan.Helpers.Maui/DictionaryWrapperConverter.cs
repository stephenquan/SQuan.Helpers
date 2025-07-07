using System.Globalization;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides conversion between a dictionary of string keys and object values and a <see cref="DictionaryWrapper"/>.
/// </summary>
/// <remarks>This converter is typically used in scenarios where a dictionary needs to be wrapped in a <see
/// cref="DictionaryWrapper"/> for easier manipulation or access. The <see cref="Convert"/> method performs the
/// wrapping, while the <see cref="ConvertBack"/> method is not implemented.</remarks>
public class DictionaryWrapperConverter : IValueConverter
{
	/// <summary>
	/// Converts the specified value into a wrapped dictionary if it is a dictionary of string keys and nullable object
	/// values.
	/// </summary>
	/// <param name="value">The value to convert. Must be an <see cref="IDictionary{TKey, TValue}"/> with string keys and nullable object
	/// values.</param>
	/// <param name="targetType">The type to convert the value to. This parameter is not used in the conversion process.</param>
	/// <param name="parameter">An optional parameter for the conversion. This parameter is not used in the conversion process.</param>
	/// <param name="culture">The culture information to use during the conversion. This parameter is not used in the conversion process.</param>
	/// <returns>A wrapped dictionary if <paramref name="value"/> is an <see cref="IDictionary{TKey, TValue}"/> with string keys and
	/// nullable object values; otherwise, <see langword="null"/>.</returns>
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is IDictionary<string, object?> dict)
		{
			return new DictionaryWrapper(dict);
		}
		return null;
	}

	/// <summary>
	/// Converts a value back to its source type in a data binding scenario.
	/// </summary>
	/// <param name="value">The value produced by the binding target to be converted.</param>
	/// <param name="targetType">The type of the binding source property.</param>
	/// <param name="parameter">An optional parameter to use during the conversion process.</param>
	/// <param name="culture">The culture to use in the conversion.</param>
	/// <returns>The converted value, or <see langword="null"/> if the conversion cannot be performed.</returns>
	/// <exception cref="NotImplementedException">This method is not implemented.</exception>
	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
