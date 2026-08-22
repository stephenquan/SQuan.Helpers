// SelectCultureMultiValueConverter.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A multi-value converter that priortizes the group culture if available, otherwise falls back to the global culture.
/// </summary>
public class SelectCultureMultiValueConverter : IMultiValueConverter
{
	/// <summary>
	/// Converts an array of values into a single culture object, prioritizing the group culture if available, otherwise falling back to the global culture.
	/// </summary>
	/// <param name="values">An array of values where the first element is the group culture and the last element is the global culture.</param>
	/// <param name="targetType">The type of the binding target property.</param>
	/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
	/// <param name="culture">The culture to be used in the converter.</param>
	/// <returns>The appropriate culture, prioritizing the group culture if available, otherwise falling back to the global culture.</returns>
	public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
		=> values.FirstOrDefault(v => v is CultureInfo) ?? values.LastOrDefault();

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
