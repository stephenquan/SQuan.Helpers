using System.Globalization;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides a value converter that transforms dictionaries with string keys and nullable object values into a wrapped
/// dictionary suitable for data binding scenarios.
/// </summary>
/// <remarks>This class implements the <see cref="IValueConverter"/> interface, enabling it to be used in XAML 
/// data binding operations. The <see cref="Convert"/> method wraps dictionaries into a <c>BindableDynamic</c>  object,
/// while the <see cref="ConvertBack"/> method is not implemented and will throw a  <see
/// cref="NotImplementedException"/>.</remarks>
public class BindableDynamicConverter : IValueConverter
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
			return new BindableDynamic(dict);
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
