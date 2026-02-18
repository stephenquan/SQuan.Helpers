// AspectView.cs

using System.Globalization;

namespace SQuan.Helpers.Maui;

/// <summary>
/// A view that maintains a specific aspect ratio for its content.
/// </summary>
public partial class AspectView : ContentView
{
	/// <summary>
	/// Bindable property for the <see cref="AspectRatio"/>.
	/// </summary>
	public static readonly BindableProperty AspectRatioProperty
		= BindableProperty.Create(nameof(AspectRatio), typeof(double), typeof(AspectView), 1.0);

	/// <summary>
	/// Gets or sets the aspect ratio (width divided by height) for the content.
	/// This property is used in conjunction with the <see cref="Aspect"/> property to determine how the content is scaled and positioned within the view.
	/// </summary>
	public double AspectRatio
	{
		get => (double)GetValue(AspectRatioProperty);
		set => SetValue(AspectRatioProperty, value);
	}

	/// <summary>
	/// The size of the content within the <see cref="AspectView"/>.
	/// This property is used to calculate the appropriate size of the content based on the specified aspect ratio and the available space.
	/// </summary>
	public static readonly BindableProperty ContentSizeProperty
		= BindableProperty.Create(nameof(ContentSize), typeof(Size), typeof(AspectView), default(Size));

	/// <summary>
	/// Gets the size of the content within the <see cref="AspectView"/>.
	/// </summary>
	public Size ContentSize
	{
		get => (Size)GetValue(ContentSizeProperty);
		internal set => SetValue(ContentSizeProperty, value);
	}

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
			cp.SetBinding(
				WidthRequestProperty,
				BindingBase.Create<AspectView, double>(static a => a.ContentSize.Width, BindingMode.OneWay, source: this));
			cp.SetBinding(
				HeightRequestProperty,
				BindingBase.Create<AspectView, double>(static a => a.ContentSize.Height, BindingMode.OneWay, source: this));
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
		public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
			=> values.Length == 3
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
		public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}
