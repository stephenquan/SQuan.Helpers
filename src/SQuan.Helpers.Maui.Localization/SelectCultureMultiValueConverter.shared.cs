// SelectCultureMultiValueConverter.shared.cs

using System.ComponentModel;
using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A multi-value converter that selects a culture from an array of CultureInfo or IsRightToLeft values.
/// </summary>
public class SelectCultureMultiValueConverter : IMultiValueConverter
{
	/// <summary>
	/// Converts an array of CultureInfo or IsRightToLeft values to a single value.
	/// The converter will prioritize the group culture if present, otherwise it will fallback on global culture.
	/// </summary>
	/// <param name="values"></param>
	/// <param name="targetType"></param>
	/// <param name="parameter"></param>
	/// <param name="culture"></param>
	/// <returns></returns>
	public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		object? result = values.FirstOrDefault(v => v is not null);

		if (result is string text && targetType != typeof(string))
		{
			TypeConverter? converter = TypeDescriptor.GetConverter(targetType);
			if (converter?.CanConvertFrom(typeof(string)) == true)
			{
				return converter.ConvertFromInvariantString(text);
			}
		}
		return result;
	}

	/// <summary>
	/// Converts a single value back to an array of values. This method is not implemented and will throw a NotImplementedException if called.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetTypes">The array of types to convert to.</param>
	/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
	/// <param name="culture">The culture to be used in the converter.</param>
	/// <returns>An array of values converted from the single value.</returns>
	/// <exception cref="NotImplementedException"></exception>
	public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
