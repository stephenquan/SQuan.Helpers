using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality to convert a value into a localized string based on a specified culture, key, and arguments.
/// </summary>
public class LocalizeMultiConverter : IMultiValueConverter
{
	/// <summary>
	/// Converts a set of input values into a localized string based on the specified key and culture.
	/// </summary>
	/// <param name="values">An array of input values where: <list type="bullet"> <item><description>The first element must be a <see
	/// cref="string"/> representing the localization key.</description></item> <item><description>The second element must
	/// be a <see cref="CultureInfo"/> representing the target culture.</description></item> <item><description>Optional
	/// additional elements are used as formatting arguments for the localized string.</description></item> </list></param>
	/// <param name="targetType">The type of the binding target property. This parameter is not used in this implementation.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion. This parameter is not used in this implementation.</param>
	/// <param name="culture">The culture to use in the conversion. This parameter is not used in this implementation.</param>
	/// <returns>A localized string corresponding to the specified key and culture. If additional values are provided, they are used
	/// as formatting arguments. Returns an empty string if the input values are invalid or do not meet the required
	/// conditions.</returns>
	public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length >= 2
			&& values[0] is string key
			&& values[1] is CultureInfo cultureUICulture)
		{
			if (values.Length == 2)
			{
				return LocalizationManager.Current.GetString(key, cultureUICulture);
			}
			return LocalizationManager.Current.GetString(key, cultureUICulture, values.Skip(2).ToArray());
		}

		return string.Empty;
	}

	/// <summary>
	/// Converts a binding target value back to the source values in a multi-binding scenario.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetTypes">The array of types to which the source values are converted.</param>
	/// <param name="parameter">An optional parameter to use during the conversion process.</param>
	/// <param name="culture">The culture to use during the conversion process.</param>
	/// <returns>An array of objects that represents the converted source values.</returns>
	/// <exception cref="NotImplementedException">This method is not implemented.</exception>
	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
