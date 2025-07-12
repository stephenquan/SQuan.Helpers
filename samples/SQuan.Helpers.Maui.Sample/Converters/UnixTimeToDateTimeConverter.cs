using System.Globalization;

namespace SQuan.Helpers.Maui.Sample;

public class UnixTimeToDateTimeConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is long unixTime)
		{
			return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
		}
		return null;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is DateTime dateTime)
		{
			return DateTimeOffset.FromFileTime(dateTime.ToFileTimeUtc()).ToUnixTimeMilliseconds();
		}
		throw new NotImplementedException();
	}
}
