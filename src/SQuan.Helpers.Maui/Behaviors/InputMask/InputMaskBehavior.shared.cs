// InputMaskBehavior.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides behavior for input views that enforces input masking based on a regular expression or a set of allowed keys.
/// </summary>
public partial class InputMaskBehavior : PlatformBehavior<InputView>
{
	/// <summary>
	/// Bindable property for the <see cref="Regex"/>.
	/// </summary>
	public static readonly BindableProperty RegexProperty
		= BindableProperty.Create(nameof(Regex), typeof(string), typeof(InputMaskBehavior), string.Empty);

	/// <summary>
	/// Gets or sets the regular expression that defines the allowed input pattern.
	/// If specified, the behavior will prevent any input that does not match this regular expression.
	/// </summary>
	public string Regex
	{
		get => (string)GetValue(RegexProperty);
		set => SetValue(RegexProperty, value);
	}

	/// <summary>
	/// Bindable property for the <see cref="Keys"/>.
	/// </summary>
	public static readonly BindableProperty KeysProperty
		= BindableProperty.Create(nameof(Keys), typeof(string), typeof(InputMaskBehavior), string.Empty);

	/// <summary>
	/// Gets or sets a string containing the allowed characters for input. This is used on platforms that support key filtering (e.g., Android).
	/// </summary>
	public string Keys
	{
		get => (string)GetValue(KeysProperty);
		set => SetValue(KeysProperty, value);
	}
}
