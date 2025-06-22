using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality to convert a value into a localized string based on a specified culture, key, and arguments.
/// </summary>
public class LocalizeConverter : IValueConverter
{
	/// <summary>
	/// Converts a value to a localized string based on the specified culture, key, and arguments.
	/// </summary>
	/// <param name="value">The value representing the target <see cref="CultureInfo"/> for localization. Must be of type <see
	/// cref="CultureInfo"/>.</param>
	/// <param name="targetType">The type to which the value is being converted. This parameter is not used in the conversion process.</param>
	/// <param name="parameter">An array of parameters where the first element is the localization key (a <see cref="string"/>) and the second
	/// element is an array of arguments (an <see cref="object"/>[]) to format the localized string.</param>
	/// <param name="culture">The culture information for the conversion. This parameter is not used in the conversion process.</param>
	/// <returns>A localized string corresponding to the specified key and arguments in the provided culture, or an empty string if
	/// the input parameters are invalid.</returns>
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is CultureInfo cultureUICulture
			&& parameter is object[] parameters
			&& parameters.Length >= 2
			&& parameters[0] is string key
			&& parameters[1] is object?[] args)
		{
			return LocalizationManager.Current.GetString(key, cultureUICulture, args);
		}
		return string.Empty;
	}

	/// <summary>
	/// Converts a value back to its source type. This method is not implemented.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetType">The type to convert the value back to.</param>
	/// <param name="parameter">An optional parameter to use during the conversion.</param>
	/// <param name="culture">The culture to use during the conversion.</param>
	/// <returns>This method does not return a value as it is not implemented.</returns>
	/// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
