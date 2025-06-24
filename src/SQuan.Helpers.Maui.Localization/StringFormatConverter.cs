using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a mechanism to format a string using multiple input values in a data binding scenario.
/// </summary>
public class StringFormatConverter : IMultiValueConverter
{
	/// <summary>
	/// Converts an array of values into a formatted string or returns the first value as a string.
	/// </summary>
	/// <param name="values">An array of objects where the first element is expected to be a format string, and subsequent elements are used as
	/// format arguments. If the array contains only one element, it is returned as a string.</param>
	/// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion. This parameter is not used.</param>
	/// <param name="culture">The culture to use for formatting the string.</param>
	/// <returns>A formatted string if the <paramref name="values"/> array contains more than one element; otherwise, the first
	/// element as a string. Returns an empty string if <paramref name="values"/> is null, empty, or does not contain a
	/// string as the first element.</returns>
	public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values is not null
			&& values.Length >= 1
			&& values[0] is string str)
		{
			return values.Length == 1 ? str : string.Format(culture, str, values.Skip(1).ToArray());
		}
		return string.Empty;
	}

	/// <summary>
	/// Converts a single value back to an array of values for binding purposes.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetTypes">The array of target types to which the data should be converted.</param>
	/// <param name="parameter">An optional parameter to use during the conversion. May be <see langword="null"/>.</param>
	/// <param name="culture">The culture to use during the conversion.</param>
	/// <returns>An array of objects that have been converted back to their respective target types.</returns>
	/// <exception cref="NotImplementedException">This method is not implemented and will always throw this exception.</exception>
	public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
