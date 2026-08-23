// MultiBoolToObjectConverter.shared.cs

using System.ComponentModel;
using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

class MultiBoolToObjectConverter : IMultiValueConverter
{
	public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		object? result = null;
		if (values.Length == 3)
		{
			result = (values[0] is bool conditional) && conditional ? values[1] : values[2];
		}

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

	public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
