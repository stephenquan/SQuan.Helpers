// FuncToMultiConverterTests.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.UnitTests;

public class FuncToMultiConverterTests
{
	static readonly CultureInfo culture = CultureInfo.InvariantCulture;

	[Fact]
	public void Convert_OneValidValue_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int>(value => value * 2);

		var result = converter.Convert([21], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_TwoValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int>((value1, value2) => value1 + value2);

		var result = converter.Convert([19, 23], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_ThreeValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int, int>((value1, value2, value3) => value1 + value2 + value3);

		var result = converter.Convert([10, 12, 20], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_FourValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int, int, int>((value1, value2, value3, value4) => value1 + value2 + value3 + value4);

		var result = converter.Convert([9, 10, 11, 12], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_FiveValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int, int, int, int>((value1, value2, value3, value4, value5) => value1 + value2 + value3 + value4 + value5);

		var result = converter.Convert([6, 7, 8, 9, 12], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_SixValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int, int, int, int, int>((value1, value2, value3, value4, value5, value6) => value1 + value2 + value3 + value4 + value5 + value6);

		var result = converter.Convert([2, 4, 6, 8, 10, 12], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_SevenValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int, int, int, int, int, int>((value1, value2, value3, value4, value5, value6, value7) => value1 + value2 + value3 + value4 + value5 + value6 + value7);

		var result = converter.Convert([3, 3, 6, 6, 6, 9, 9], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_EightValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int, int, int, int, int, int, int>((value1, value2, value3, value4, value5, value6, value7, value8) => value1 + value2 + value3 + value4 + value5 + value6 + value7 + value8);

		var result = converter.Convert([1, 2, 3, 4, 5, 7, 9, 11], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_NineValidValues_ReturnsFunctionResult()
	{
		var converter = new FuncToMultiConverter<int, int, int, int, int, int, int, int, int, int>((value1, value2, value3, value4, value5, value6, value7, value8, value9) => value1 + value2 + value3 + value4 + value5 + value6 + value7 + value8 + value9);

		var result = converter.Convert([1, 2, 3, 4, 5, 6, 6, 7, 8], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_StringResultWithConvertibleTarget_ConvertsToTargetType()
	{
		var converter = new FuncToMultiConverter<int, string>(value => value.ToString(culture));

		var result = converter.Convert([42], typeof(int), null, culture);

		Assert.Equal(42, result);
	}

	[Fact]
	public void Convert_IncorrectValueCount_ReturnsNull()
	{
		var converter = new FuncToMultiConverter<int, int, int>((value1, value2) => value1 + value2);

		var result = converter.Convert([42], typeof(int), null, culture);

		Assert.Null(result);
	}

	[Fact]
	public void Convert_IncorrectValueType_ReturnsNull()
	{
		var converter = new FuncToMultiConverter<int, int>(value => value * 2);

		var result = converter.Convert(["42"], typeof(int), null, culture);

		Assert.Null(result);
	}

	[Fact]
	public void ConvertBack_AnyValues_ThrowsNotSupportedException()
	{
		var converter = new FuncToMultiConverter<int, int>(value => value);

		Assert.Throws<NotSupportedException>(() => converter.ConvertBack(42, [typeof(int)], null, culture));
	}
}
