// BoolToObjectExtension.shared.cs

using System.Globalization;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Converters;

namespace SQuan.Helpers.Maui;

/// <summary>
/// This is a wrapper for the <see cref="BoolToObjectConverter"/> that allows it to be used as a markup extension in XAML.
/// </summary>
[ContentProperty(nameof(Value))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class BoolToObjectExtension : BaseBindableObjectMarkupExtension
{
	/// <summary>
	/// Gets or sets the boolean value to be converted to an object.
	/// </summary>
	[BindableProperty]
	public partial bool Value { get; set; } = true;

	/// <summary>
	/// Gets or sets the value to return when the bound boolean value is true.
	/// </summary>
	[BindableProperty]
	public partial object? TrueObject { get; set; }

	/// <summary>
	/// Gets or sets the value to return when the bound boolean value is false.
	/// </summary>
	[BindableProperty]
	public partial object? FalseObject { get; set; }

	/// <summary>
	/// Returns the MultiBinding that uses the <see cref="BoolToObjectConverter"/> to convert the boolean value to an object based on the specified true and false values.
	/// </summary>
	/// <param name="serviceProvider">The service provider used to retrieve services.</param>
	/// <returns>The binding for the markup extension.</returns>
	public override BindingBase ProvideBindingValue(IServiceProvider serviceProvider)
		=> new MultiBinding
		{
			Bindings =
			{
				BindingBase.Create(static (BoolToObjectExtension ctx) => ctx.Value, BindingMode.OneWay, source: this),
				BindingBase.Create(static (BoolToObjectExtension ctx) => ctx.TrueObject, BindingMode.OneWay, source: this),
				BindingBase.Create(static (BoolToObjectExtension ctx) => ctx.FalseObject, BindingMode.OneWay, source: this)
			},
			Mode = BindingMode.OneWay,
			Converter = new FuncToMultiConverter<bool, object?, object?, object?>(
				(value, trueObject, falseObject)
					=> new BoolToObjectConverter
					{
						TrueObject = trueObject,
						FalseObject = falseObject
					}
					.ConvertFrom(value, CultureInfo.CurrentCulture)),
		};
}
