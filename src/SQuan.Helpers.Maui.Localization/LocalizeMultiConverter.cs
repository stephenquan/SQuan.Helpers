using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;


/// <summary>
/// Provides functionality to convert a value into a localized string based on a specified culture, key, and arguments.
/// </summary>
public class LocalizeMultiConverter : IMultiValueConverter
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="values"></param>
	/// <param name="targetType"></param>
	/// <param name="parameter"></param>
	/// <param name="culture"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
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
