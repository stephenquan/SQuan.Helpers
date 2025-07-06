using System.Globalization;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides a mechanism to convert values based on the application's theme (light or dark).
/// </summary>
/// <remarks>This converter is designed for use in multi-binding scenarios where the application's theme
/// determines which value to return. The first input value must be of type <see cref="AppTheme"/>, representing the
/// current theme. The second and third input values correspond to the values for light and dark themes,
/// respectively.</remarks>
public class AppThemeConverter : IMultiValueConverter
{
	/// <summary>
	/// Converts a set of input values into an object based on the specified target type, parameter, and culture.
	/// </summary>
	/// <param name="values">An array of input values. The first element must be of type <see cref="AppTheme"/>. The second and third elements
	/// represent the values to return for light and dark themes, respectively.</param>
	/// <param name="targetType">The type of the object to return. This parameter is not used in the conversion logic but may be required by certain
	/// frameworks.</param>
	/// <param name="parameter">An optional parameter that can be used to influence the conversion logic. This parameter is not used in the current
	/// implementation.</param>
	/// <param name="culture">The culture information to use during the conversion. This parameter is not used in the current implementation.</param>
	/// <returns>The second element of <paramref name="values"/> if the theme is light, the third element if the theme is dark, or
	/// <see langword="null"/> if the input is invalid.</returns>
	public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values is not null && values.Length >= 3 && values[0] is AppTheme appTheme)
		{
			return (appTheme == AppTheme.Dark) ? values[2] : values[1];
		}
		return null;
	}

	/// <summary>
	/// Converts a value back to its source values in a multi-binding scenario.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetTypes">The array of types to which the data must be converted.</param>
	/// <param name="parameter">An optional parameter to use during the conversion process.</param>
	/// <param name="culture">The culture to use in the conversion.</param>
	/// <returns>An array of values that have been converted back to their source types.</returns>
	/// <exception cref="NotImplementedException">Always thrown, as this method is not implemented for <see cref="AppThemeConverter"/>.</exception>
	public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException("ConvertBack is not implemented for AppThemeConverter.");
	}
}
