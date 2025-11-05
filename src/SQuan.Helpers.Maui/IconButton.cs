// IconButton.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Represents a button control that displays an icon, with customizable font family, color, and size.
/// </summary>
/// <remarks>The <see cref="IconButton"/> class extends the functionality of a standard button by
/// allowing the display of an icon.
/// The icon's appearance can be customized using the <see cref="IconFontFamily"/>,
/// <see cref="IconColor"/>, and <see cref="IconSize"/> properties.
/// By default, the icon uses the <see cref="LucideIcons.FontFamily"/> font family,
/// icon color will match the button's text color, and a size of 24.0.</remarks>
public partial class IconButton : Button
{
	/// <summary>
	/// Bindable property for <see cref="Icon"/>.
	/// Defaults to <see cref="LucideIcons.Smile"/>.
	/// </summary>
	public static readonly BindableProperty IconProperty = BindableProperty.Create(
		nameof(Icon), typeof(string), typeof(IconButton), LucideIcons.Smile);

	/// <summary>
	/// Gets or sets the icon character to be displayed on the button.
	/// </summary>
	public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="IconColor"/>.
	/// </summary>
	public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
		nameof(IconColor), typeof(Color), typeof(IconButton), Colors.Black,
		coerceValue: (b, v) => (v is Color color) ? color : Colors.Black);

	/// <summary>
	/// Gets or sets the color of the icon displayed on the button.
	/// </summary>
	public Color IconColor
	{
		get => (Color)GetValue(IconColorProperty);
		set => SetValue(IconColorProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="IconFontFamily"/>.
	/// Defaults to <see cref="LucideIcons.FontFamily"/>.
	/// </summary>
	public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(
		nameof(IconFontFamily), typeof(string), typeof(IconButton), LucideIcons.FontFamily);

	/// <summary>
	/// Gets or sets the font family used for the icon displayed on the button.
	/// </summary>
	public string IconFontFamily
	{
		get => (string)GetValue(IconFontFamilyProperty);
		set => SetValue(IconFontFamilyProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="IconSize"/>.
	/// Defaults to 24.0.
	/// </summary>
	public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(
		nameof(IconSize), typeof(double), typeof(IconButton), 24.0);

	/// <summary>
	/// Gets or sets the size of the icon displayed on the button.
	/// </summary>
	public double IconSize
	{
		get => (double)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="IconButton"/> class.
	/// </summary>
	public IconButton()
	{
		this.SetBinding(IconButton.IconColorProperty, static (IconButton b) => b.TextColor, BindingMode.OneWay, source: this);
		var fontImageSource = new FontImageSource();
		fontImageSource.SetBinding(FontImageSource.GlyphProperty, static (IconButton b) => b.Icon, BindingMode.OneWay, source: this);
		fontImageSource.SetBinding(FontImageSource.ColorProperty, static (IconButton b) => b.IconColor, BindingMode.OneWay, source: this);
		fontImageSource.SetBinding(FontImageSource.FontFamilyProperty, static (IconButton b) => b.IconFontFamily, BindingMode.OneWay, source: this);
		fontImageSource.SetBinding(FontImageSource.SizeProperty, static (IconButton b) => b.IconSize, BindingMode.OneWay, source: this);
		ImageSource = fontImageSource;
	}
}
