// InputViewHelper.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides helper functionality for configuring input masking and border styling on an Entry or Editor control.
/// </summary>
public partial class InputViewHelper : BindableObject
{
	/// <summary>
	/// Gets the Entry or Editor control associated with this helper.
	/// </summary>
	public InputView InputView { get; init; }

	/// <summary>
	/// Bindable property for <see cref="BorderThickness"/>.
	/// </summary>
	public static readonly BindableProperty BorderThicknessProperty
		= BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(InputViewHelper), 1, BindingMode.OneWay,
			propertyChanged: (b, o, n) => ((InputViewHelper)b).ApplyBorderStyle());

	/// <summary>
	/// Gets or sets the thickness of the border around the input control.
	/// </summary>
	public int BorderThickness
	{
		get => (int)GetValue(BorderThicknessProperty);
		set => SetValue(BorderThicknessProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="MaskType"/>.
	/// </summary>
	public static readonly BindableProperty MaskTypeProperty
	= BindableProperty.Create(nameof(MaskType), typeof(InputViewMaskKind), typeof(InputViewHelper), InputViewMaskKind.None, BindingMode.OneWay,
		propertyChanged: (b, o, n) => ((InputViewHelper)b).ApplyMask());

	/// <summary>
	/// Gets or sets the type of input mask to apply to the input control.
	/// </summary>
	public InputViewMaskKind MaskType
	{
		get => (InputViewMaskKind)GetValue(MaskTypeProperty);
		set => SetValue(MaskTypeProperty, value);
	}

#if IOS || MACCATALYST
	bool textFieldDelegateSet = false;
	bool textViewDelegateSet = false;
#elif WINDOWS
	bool beforeTextChangingSubscribed = false;
#endif

#if IOS || MACCATALYST || WINDOWS
	internal System.Text.RegularExpressions.Regex AllowedRegex { get; set; } = new("^-?[0-9]*[.,]?[0-9]*$");
#elif ANDROID
	string allowedText = "0123456789,.-";
#endif

	/// <summary>
	/// Initializes a new instance of the <see cref="InputViewHelper"/> class.
	/// </summary>
	/// <param name="inputView">An Entry or Editor control.</param>
	public InputViewHelper(InputView inputView)
	{
		this.InputView = inputView;
		if (InputView.Handler?.PlatformView is not null)
		{
			Apply();
		}
		else
		{
			InputView.HandlerChanged += InputView_HandlerChanged;
		}
	}

	void InputView_HandlerChanged(object? sender, EventArgs e)
	{
		if (InputView.Handler is not null)
		{
			Apply();
		}

		this.InputView.HandlerChanged -= InputView_HandlerChanged;
	}

	void Apply()
	{
		ApplyBorderStyle();
		ApplyMask();
	}

	void ApplyBorderStyle()
	{
		switch (InputView.Handler?.PlatformView)
		{
#if IOS || MACCATALYST
			case UIKit.UITextField textField:
				textField.Layer.BorderWidth = BorderThickness;
				break;
			case UIKit.UITextView textView:
				textView.Layer.BorderWidth = BorderThickness;
				break;
#elif ANDROID
			case Microsoft.Maui.Platform.MauiAppCompatEditText editText:
				editText.BackgroundTintList = BorderThickness switch
				{
					> 0 => null,
					_ =>Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent),
				};
				break;
#elif WINDOWS
			case Microsoft.UI.Xaml.Controls.TextBox textBox:
				textBox.Resources["TextControlBorderThemeThickness"] = new Microsoft.UI.Xaml.Thickness(BorderThickness);
				textBox.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(BorderThickness);
				break;
#endif
			case null:
				break;
			default:
				break;
		}
	}

	void ApplyMask()
	{
		switch (InputView.Handler?.PlatformView)
		{
#if IOS || MACCATALYST
			case UIKit.UITextField textField:
				if (!textFieldDelegateSet)
				{
					textField.Delegate = new BlockingTextFieldDelegate(this, textField);
					textFieldDelegateSet = true;
				}
				break;
			case UIKit.UITextView textView:
				if (!textViewDelegateSet)
				{
					textView.Delegate = new BlockingTextViewDelegate(this, textView);
					textViewDelegateSet = true;
				}
				break;
#elif ANDROID
			case Microsoft.Maui.Platform.MauiAppCompatEditText editText:
				switch (MaskType)
				{
					case InputViewMaskKind.None:
						break;
					case InputViewMaskKind.Numeric:
						editText.KeyListener = Android.Text.Method.DigitsKeyListener.GetInstance(allowedText);
						break;
					default:
						break;
				}
				break;
#elif WINDOWS
			case Microsoft.UI.Xaml.Controls.TextBox textBox:
				if (!beforeTextChangingSubscribed)
				{
					textBox.BeforeTextChanging += TextBox_BeforeTextChanging;
					beforeTextChangingSubscribed = true;
				}
				break;
#endif
			case null:
				break;
			default:
				break;
		}
	}

#if IOS || MACCATALYST
	class BlockingTextFieldDelegate : UIKit.UITextFieldDelegate
	{
		InputViewHelper owner;
		public BlockingTextFieldDelegate(InputViewHelper owner, UIKit.UITextField view)
		{
			this.owner = owner;
		}
		public override bool ShouldChangeCharacters(UIKit.UITextField textField, Foundation.NSRange range, string replacementString)
			=> BlockingTextHelper.ShouldChangeText(owner, textField.Text ?? string.Empty, range, replacementString);
	}
	class BlockingTextViewDelegate : UIKit.UITextViewDelegate
	{
		InputViewHelper owner;
		public BlockingTextViewDelegate(InputViewHelper owner, UIKit.UITextView view)
		{
			this.owner = owner;
		}
		public override bool ShouldChangeText(UIKit.UITextView textView, Foundation.NSRange range, string replacementString)
			=> BlockingTextHelper.ShouldChangeText(owner, textView.Text ?? string.Empty, range, replacementString);
	}
	static class BlockingTextHelper
	{
		public static bool ShouldChangeText(InputViewHelper owner, string oldText, Foundation.NSRange range, string replacementString)
		{
			if (owner.MaskType == InputViewMaskKind.None)
			{
				return true;
			}
			var newText = oldText.Substring(0, (int)range.Location)
						  + replacementString
						  + oldText.Substring((int)(range.Location + range.Length));
			return owner.AllowedRegex.IsMatch(newText);
		}
	}
#elif WINDOWS
	void TextBox_BeforeTextChanging(Microsoft.UI.Xaml.Controls.TextBox s, Microsoft.UI.Xaml.Controls.TextBoxBeforeTextChangingEventArgs e)
	{
		if (MaskType == InputViewMaskKind.None)
		{
			return;
		}
		if (AllowedRegex.IsMatch(e.NewText) == false)
		{
			e.Cancel = true;
		}
	}
#endif
}
