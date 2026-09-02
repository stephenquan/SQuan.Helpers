// FuncToConverter.shared.cs

using System.ComponentModel;
using System.Globalization;

namespace SQuan.Helpers.Maui;

/// <summary>
/// A converter that uses a specified function to convert a value of type <typeparamref name="T"/> to a value of type <typeparamref name="TReturn"/>.
/// </summary>
/// <typeparam name="T">The type of the input value.</typeparam>
/// <typeparam name="TReturn">The type of the output value.</typeparam>
public class FuncToConverter<T, TReturn> : IValueConverter
{
	readonly Func<T, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToConverter{T, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The function to use for converting values.</param>
	public FuncToConverter(Func<T, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts a single input value to an output value using the specified conversion function.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is T valueT)
		{
			TReturn result = convert(valueT);
			if (result is not null
				&& targetType != typeof(TReturn)
				&& TypeDescriptor.GetConverter(targetType) is TypeConverter targetConverter
				&& targetConverter.CanConvertFrom(typeof(TReturn)))
			{
				return targetConverter.ConvertFrom(null, culture, result);
			}
			return result;
		}
		return null;
	}

	/// <summary>
	/// Converts a value back to its original type. This method is not implemented and will throw a <see cref="NotImplementedException"/> if called.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	/// <exception cref="NotImplementedException"></exception>
	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
