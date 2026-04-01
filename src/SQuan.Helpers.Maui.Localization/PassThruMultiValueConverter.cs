// PassThruMultiValueConverter.cs

namespace SQuan.Helpers.Maui.Localization;

class PassThruMultiValueConverter : IMultiValueConverter
{
	public object? Convert(object?[] values, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
		=> values;
	public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, System.Globalization.CultureInfo culture)
		=> value is object?[] values ? values : throw new ArgumentException("Expected an array of objects.", nameof(value));
}
