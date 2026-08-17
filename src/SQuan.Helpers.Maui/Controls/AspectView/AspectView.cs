// AspectView.cs

using System.Globalization;
using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui;

/// <summary>
/// A view that maintains a specific aspect ratio for its content.
/// </summary>
public partial class AspectView : ContentView
{
	/// <summary>
	/// Gets or sets the aspect ratio (width divided by height) for the content.
	/// This property is used to constrain the size of the content within the available space while maintaining the specified aspect ratio.
	/// </summary>
	[BindableProperty(UseStaticCallbacks = true, CoerceValueMethodName = nameof(CoerceAspectRatio))]
	public partial double AspectRatio { get; set; } = 1.0;

	static object CoerceAspectRatio(BindableObject bindable, object value)
		=> (value is double aspectRatio && aspectRatio > 0.0) ? aspectRatio : 1.0;

	/// <summary>
	/// Gets the size of the content within the <see cref="AspectView"/>.
	/// </summary>
	[BindableProperty(UseStaticCallbacks = true)]
	public partial Size ContentSize { get; internal set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="AspectView"/> class.
	/// </summary>
	public AspectView()
	{
		ControlTemplate = new(() =>
		{
			var cp = new ContentPresenter();
			var topGrid = new Grid { cp };
			this.SetBinding(ContentSizeProperty,
				new MultiBinding
				{
					Bindings =
					{
						BindingBase.Create<VisualElement, double>(static g => g.Width, BindingMode.OneWay, source: topGrid),
						BindingBase.Create<VisualElement, double>(static g => g.Height, BindingMode.OneWay, source: topGrid),
						BindingBase.Create<AspectView, double>(static a => a.AspectRatio, BindingMode.OneWay, source: this),
					},
					Mode = BindingMode.OneWay,
					Converter = new ContentSizeConverter()
				});
			cp.SetBinding(WidthRequestProperty, BindingBase.Create<AspectView, double>(static a => a.ContentSize.Width, BindingMode.OneWay, source: this));
			cp.SetBinding(HeightRequestProperty, BindingBase.Create<AspectView, double>(static a => a.ContentSize.Height, BindingMode.OneWay, source: this));
			return topGrid;
		});
	}

	/// <summary>
	/// Converts width, height, and aspect ratio values into a <see cref="Size"/> that preserves the desired aspect ratio.
	/// </summary>
	class ContentSizeConverter : IMultiValueConverter
	{
		/// <summary>
		/// Converts the specified width, height, and aspect ratio values into a <see cref="Size"/> that maintains the aspect ratio.
		/// </summary>
		/// <param name="values">An array containing the width, height, and aspect ratio values.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture to be used in the converter.</param>
		/// <returns>A <see cref="Size"/> that maintains the specified aspect ratio.</returns>
		public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
			=> values is { Length: 3 }
			&& values[0] is double width && values[1] is double height && values[2] is double aspectRatio
			&& width > 0 && height > 0 && aspectRatio > 0
				? (height * aspectRatio < width
					? new Size(height * aspectRatio, height)
					: new Size(width, width / aspectRatio))
				: Size.Zero;

		/// <summary>
		/// Not supported. This converter does not provide a ConvertBack implementation.
		/// </summary>
		/// <param name="value">The value produced by the binding target.</param>
		/// <param name="targetTypes">The array of types to convert to.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture to be used in the converter.</param>
		/// <returns>Throws a <see cref="NotImplementedException"/> since ConvertBack is not supported.</returns>
		/// <exception cref="NotImplementedException"></exception>
		public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
			=> throw new NotImplementedException("ConvertBack is not implemented for ContentSizeConverter.");
	}
}
