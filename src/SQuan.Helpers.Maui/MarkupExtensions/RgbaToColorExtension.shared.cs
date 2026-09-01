// RgbaToColorExtension.shared.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// A markup extension that converts RGBA values to a <see cref="Color"/> object using the <see cref="RgbaToColorConverter"/>.
/// </summary>
[ContentProperty(nameof(Red))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class RgbaToColorExtension : BaseBindableObjectMarkupExtension
{
	/// <summary>
	/// Gets or sets the red component of the color (0-255).
	/// </summary>
	[BindableProperty]
	public partial int Red { get; set; } = 255;

	/// <summary>
	/// Gets or sets the green component of the color (0-255).
	/// </summary>
	[BindableProperty]
	public partial int Green { get; set; } = 255;

	/// <summary>
	/// Gets or sets the blue component of the color (0-255).
	/// </summary>
	[BindableProperty]
	public partial int Blue { get; set; } = 255;

	/// <summary>
	/// Gets or sets the alpha (transparency) component of the color (0-255).
	/// </summary>
	[BindableProperty]
	public partial int Alpha { get; set; } = 255;

	/// <summary>
	/// Provides the binding value for the markup extension, which is a <see cref="MultiBinding"/> that combines the
	/// RGBA values and uses the <see cref="RgbaToColorConverter"/> to convert them into a <see cref="Color"/> object.
	/// </summary>
	/// <param name="serviceProvider">The service provider.</param>
	/// <returns>A <see cref="MultiBinding"/> that combines the RGBA values and uses the <see cref="RgbaToColorConverter"/> to convert them into a <see cref="Color"/> object.</returns>
	public override BindingBase ProvideBindingValue(IServiceProvider serviceProvider)
	{
		return new MultiBinding
		{
			Bindings =
			{
				BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Red, BindingMode.OneWay, source: this),
				BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Green, BindingMode.OneWay, source: this),
				BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Blue, BindingMode.OneWay, source: this),
				BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Alpha, BindingMode.OneWay, source: this)
			},
			Mode = BindingMode.OneWay,
			Converter = new RgbaToColorConverter()
		};
	}
}
