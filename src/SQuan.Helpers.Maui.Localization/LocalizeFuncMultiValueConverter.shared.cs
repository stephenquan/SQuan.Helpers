// LocalizeFuncMultiValueConverter.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

class LocalizeFuncMultiValueConverter<TProvider> : IMultiValueConverter
{
	public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length >= 4
			&& values[0] is Func<CultureInfo, TProvider> localizationProvider
			&& values[1] is CultureInfo currentUICulture
			&& values[2] is CultureInfo currentCulture
			&& values[3] is object?[] args)
		{
			TProvider localizedValue = localizationProvider(currentUICulture);
			if (localizedValue is string localizedString
				&& typeof(TProvider) == typeof(string))
			{
				return string.Format(currentCulture, localizedString, args);
			}
			return localizedValue;
		}
		return null;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
