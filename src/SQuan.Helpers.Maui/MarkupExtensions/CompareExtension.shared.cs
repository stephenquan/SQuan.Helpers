// CompareExtension.shared.cs

using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Converters;

namespace SQuan.Helpers.Maui;

/// <summary>
/// This is a wrapper for the <see cref="CompareConverter"/> that allows it to be used as a markup extension in XAML.
/// </summary>
[ContentProperty(nameof(Value))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class CompareExtension : BaseBindableObjectMarkupExtension
{
	/// <summary>
	/// Gets or sets the value to be converted to an object.
	/// </summary>
	[BindableProperty]
	public partial IComparable Value { get; set; }

	/// <summary>
	/// Gets or sets the value to compare the bound value against.
	/// </summary>
	[BindableProperty]
	public partial IComparable ComparingValue { get; set; }

	/// <summary>
	/// Gets or sets the comparison operator to use when comparing the bound value with the specified value.
	/// </summary>
	[BindableProperty]
	public partial CompareConverter.OperatorType ComparisonOperator { get; set; } = CompareConverter.OperatorType.Equal;

	/// <summary>
	/// Gets or sets the value to return when the comparison result is true.
	/// </summary>
	[BindableProperty]
	public partial object? TrueObject { get; set; }

	/// <summary>
	/// Gets or sets the value to return when the comparison result is false.
	/// </summary>
	[BindableProperty]
	public partial object? FalseObject { get; set; }

	/// <summary>
	/// Returns the MultiBinding that uses the <see cref="CompareConverter"/> to compare the bound value against the specified comparison value.
	/// </summary>
	/// <param name="serviceProvider">The service provider used to retrieve services.</param>
	/// <returns>The binding for the markup extension.</returns>
	public override BindingBase ProvideBindingValue(IServiceProvider serviceProvider)
		=> new MultiBinding
		{
			Bindings =
			{
				BindingBase.Create(static (CompareExtension ctx) => ctx.Value, BindingMode.OneWay, source: this),
				BindingBase.Create(static (CompareExtension ctx) => ctx.ComparingValue, BindingMode.OneWay, source: this),
				BindingBase.Create(static (CompareExtension ctx) => ctx.ComparisonOperator, BindingMode.OneWay, source: this),
				BindingBase.Create(static (CompareExtension ctx) => ctx.TrueObject, BindingMode.OneWay, source: this),
				BindingBase.Create(static (CompareExtension ctx) => ctx.FalseObject, BindingMode.OneWay, source: this)
			},
			Mode = BindingMode.OneWay,
			Converter = new FuncToMultiConverter<IComparable, IComparable, CompareConverter.OperatorType, object?, object?, object?>(
				(value, comparingValue, comparisonOperator, trueObject, falseObject)
					=>
					{
						if (comparingValue
							is not null
							&& value is not null
							&& comparingValue.GetType() != value.GetType()
							&& TypeDescriptor.GetConverter(value.GetType()) is TypeConverter converter
							&& converter.CanConvertFrom(comparingValue.GetType())
							&& converter.ConvertFrom(null, CultureInfo.CurrentCulture, comparingValue) is IComparable _comparingValue)
						{
							comparingValue = _comparingValue;
						}
						if (value is IComparable _value)
						{
							return new CompareConverter
							{
								ComparingValue = comparingValue,
								ComparisonOperator = comparisonOperator,
								TrueObject = trueObject,
								FalseObject = falseObject,
							}
							.ConvertFrom(_value, CultureInfo.CurrentCulture);
						}
						return null;
					})
		};
}
