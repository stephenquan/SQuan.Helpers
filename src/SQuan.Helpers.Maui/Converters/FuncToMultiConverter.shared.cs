// FuncToMultiConverter.shared.cs

using System.ComponentModel;
using System.Globalization;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a base class for converting multiple input values to a single output value using a specified function.
/// </summary>
public abstract class FuncToMultiConverterBase : IMultiValueConverter
{
	/// <summary>
	/// Converts the result of the conversion function to the target type, if necessary, using the specified culture information.
	/// </summary>
	/// <param name="result">The result of the conversion function.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	protected static object? ConvertResult(object? result, Type targetType, CultureInfo culture)
	{
		if (result is not null
			&& targetType != result.GetType()
			&& TypeDescriptor.GetConverter(targetType) is TypeConverter typeConverter
			&& typeConverter.CanConvertFrom(result.GetType()))
		{
			return typeConverter.ConvertFrom(null, culture, result);
		}
		return result;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	public abstract object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture);

	/// <summary>
	/// Not supported. This converter does not provide a ConvertBack implementation.
	/// </summary>
	/// <param name="value">The value produced by the binding target.</param>
	/// <param name="targetTypes">The array of types to convert to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>Does not return a value. This method always throws a NotSupportedException.</returns>
	/// <exception cref="NotSupportedException"></exception>
	public virtual object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}

/// <summary>
/// Represents a converter that converts a single input value to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, TReturn> convert;
	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to use.</param>
	public FuncToMultiConverter(Func<T1, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 1
			&& values[0] is T1 value1)
		{
			return ConvertResult(convert(value1), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts two input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, TReturn> convert;
	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to use.</param>
	public FuncToMultiConverter(Func<T1, T2, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 2
			&& values[0] is T1 value1
			&& values[1] is T2 value2)
		{
			return ConvertResult(convert(value1, value2), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts three input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="T3">The type of the third input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, T3, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, T3, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, T3, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to use.</param>
	public FuncToMultiConverter(Func<T1, T2, T3, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 3
			&& values[0] is T1 value1
			&& values[1] is T2 value2
			&& values[2] is T3 value3)
		{
			return ConvertResult(convert(value1, value2, value3), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts four input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="T3">The type of the third input value.</typeparam>
/// <typeparam name="T4">The type of the fourth input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, T3, T4, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, T3, T4, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, T3, T4, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert"></param>
	public FuncToMultiConverter(Func<T1, T2, T3, T4, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted result.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 4
			&& values[0] is T1 value1
			&& values[1] is T2 value2
			&& values[2] is T3 value3
			&& values[3] is T4 value4)
		{
			return ConvertResult(convert(value1, value2, value3, value4), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts five input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="T3">The type of the third input value.</typeparam>
/// <typeparam name="T4">The type of the fourth input value.</typeparam>
/// <typeparam name="T5">The type of the fifth input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, T3, T4, T5, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, T3, T4, T5, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, T3, T4, T5, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to be used by the converter.</param>
	public FuncToMultiConverter(Func<T1, T2, T3, T4, T5, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns></returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 5
			&& values[0] is T1 value1
			&& values[1] is T2 value2
			&& values[2] is T3 value3
			&& values[3] is T4 value4
			&& values[4] is T5 value5)
		{
			return ConvertResult(convert(value1, value2, value3, value4, value5), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts six input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="T3">The type of the third input value.</typeparam>
/// <typeparam name="T4">The type of the fourth input value.</typeparam>
/// <typeparam name="T5">The type of the fifth input value.</typeparam>
/// <typeparam name="T6">The type of the sixth input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, T3, T4, T5, T6, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, T3, T4, T5, T6, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, T3, T4, T5, T6, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to be used by the converter.</param>
	public FuncToMultiConverter(Func<T1, T2, T3, T4, T5, T6, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted value.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 6
			&& values[0] is T1 value1
			&& values[1] is T2 value2
			&& values[2] is T3 value3
			&& values[3] is T4 value4
			&& values[4] is T5 value5
			&& values[5] is T6 value6)
		{
			return ConvertResult(convert(value1, value2, value3, value4, value5, value6), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts seven input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="T3">The type of the third input value.</typeparam>
/// <typeparam name="T4">The type of the fourth input value.</typeparam>
/// <typeparam name="T5">The type of the fifth input value.</typeparam>
/// <typeparam name="T6">The type of the sixth input value.</typeparam>
/// <typeparam name="T7">The type of the seventh input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, T3, T4, T5, T6, T7, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, T3, T4, T5, T6, T7, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, T3, T4, T5, T6, T7, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to be used by the converter.</param>
	public FuncToMultiConverter(Func<T1, T2, T3, T4, T5, T6, T7, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted value.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 7
			&& values[0] is T1 value1
			&& values[1] is T2 value2
			&& values[2] is T3 value3
			&& values[3] is T4 value4
			&& values[4] is T5 value5
			&& values[5] is T6 value6
			&& values[6] is T7 value7)
		{
			return ConvertResult(convert(value1, value2, value3, value4, value5, value6, value7), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts eight input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="T3">The type of the third input value.</typeparam>
/// <typeparam name="T4">The type of the fourth input value.</typeparam>
/// <typeparam name="T5">The type of the fifth input value.</typeparam>
/// <typeparam name="T6">The type of the sixth input value.</typeparam>
/// <typeparam name="T7">The type of the seventh input value.</typeparam>
/// <typeparam name="T8">The type of the eighth input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, T3, T4, T5, T6, T7, T8, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to be used by the converter.</param>
	public FuncToMultiConverter(Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted value.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 8
			&& values[0] is T1 value1
			&& values[1] is T2 value2
			&& values[2] is T3 value3
			&& values[3] is T4 value4
			&& values[4] is T5 value5
			&& values[5] is T6 value6
			&& values[6] is T7 value7
			&& values[7] is T8 value8)
		{
			return ConvertResult(convert(value1, value2, value3, value4, value5, value6, value7, value8), targetType, culture);
		}
		return null;
	}
}

/// <summary>
/// Represents a converter that converts nine input values to a single output value using a specified function.
/// </summary>
/// <typeparam name="T1">The type of the first input value.</typeparam>
/// <typeparam name="T2">The type of the second input value.</typeparam>
/// <typeparam name="T3">The type of the third input value.</typeparam>
/// <typeparam name="T4">The type of the fourth input value.</typeparam>
/// <typeparam name="T5">The type of the fifth input value.</typeparam>
/// <typeparam name="T6">The type of the sixth input value.</typeparam>
/// <typeparam name="T7">The type of the seventh input value.</typeparam>
/// <typeparam name="T8">The type of the eighth input value.</typeparam>
/// <typeparam name="T9">The type of the ninth input value.</typeparam>
/// <typeparam name="TReturn">The type of the return value.</typeparam>
public class FuncToMultiConverter<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> : FuncToMultiConverterBase
{
	readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> convert;

	/// <summary>
	/// Initializes a new instance of the <see cref="FuncToMultiConverter{T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn}"/> class with the specified conversion function.
	/// </summary>
	/// <param name="convert">The conversion function to be used by the converter.</param>
	public FuncToMultiConverter(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> convert)
	{
		this.convert = convert;
	}

	/// <summary>
	/// Converts an array of input values to a single output value using the specified conversion function.
	/// </summary>
	/// <param name="values">An array of input values.</param>
	/// <param name="targetType">The type to convert the result to.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion.</param>
	/// <param name="culture">The culture information to use for the conversion.</param>
	/// <returns>The converted value.</returns>
	public override object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Length == 9
			&& values[0] is T1 value1
			&& values[1] is T2 value2
			&& values[2] is T3 value3
			&& values[3] is T4 value4
			&& values[4] is T5 value5
			&& values[5] is T6 value6
			&& values[6] is T7 value7
			&& values[7] is T8 value8
			&& values[8] is T9 value9)
		{
			return ConvertResult(convert(value1, value2, value3, value4, value5, value6, value7, value8, value9), targetType, culture);
		}
		return null;
	}
}
