// LocalizeMultiValueConverter.shared.cs

using System.ComponentModel;
using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality to convert a value into a localized string based on a specified culture, key, and arguments.
/// </summary>
class LocalizeMultiValueConverter : IMultiValueConverter
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
	/// <returns>A localized string corresponding to the specified key and culture.
	/// If additional values are provided, they are used as formatting arguments.
	/// Returns an empty string if the input values are invalid or do not meet the required conditions.</returns>
	public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		object? result = null;

		if (values.Length >= 7
			&& values[2] is CultureInfo currentUICulture
			&& values[3] is CultureInfo currentCulture
			&& values[4] is object?[] args)
		{
			if (values[5] is IValueConverter valueConverter)
			{
				if (args.Length >= 1)
				{
					args = new object?[] { valueConverter.Convert(args[0], typeof(object), values[6], culture) };
				}
			}
			else if (values[5] is IMultiValueConverter multiValueConverter)
			{
				args = new object?[] { multiValueConverter.Convert(args, typeof(object), values[6], culture) };
			}

			if (values[1] is Func<CultureInfo, object> localizationProvider)
			{
				var localizedValue = localizationProvider(currentUICulture);
				if (localizedValue is string localizedString)
				{
					result = LocalizationManager.Current.GetString(localizedString, currentUICulture, currentCulture, args);
				}
				else
				{
					result = localizedValue;
				}
			}
			else if (values[0] is string key && !string.IsNullOrEmpty(key))
			{
				result = LocalizationManager.Current.GetString(key, currentUICulture, currentCulture, args);
			}
			else if (args.Length == 1)
			{
				result = args[0];
			}
			else
			{
				result = args;
			}
		}

		if (targetType.IsValueType)
		{
			if (result is null)
			{
				return Activator.CreateInstance(targetType);
			}
			else if (TypeDescriptor.GetConverter(targetType) is TypeConverter converter
				&& converter.CanConvertFrom(result.GetType()))
			{
				return converter.ConvertFrom(result);
			}
		}

		return result;
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
