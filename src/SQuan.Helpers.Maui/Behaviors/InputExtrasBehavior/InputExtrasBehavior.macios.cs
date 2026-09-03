// InputExtrasBehavior.macios.cs

using System.Diagnostics.CodeAnalysis;

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
[SuppressMessage(
	"Design",
	"CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
	Justification = "This behavior does not own delegate lifetimes. UIKit owns delegates; they are swapped and released in OnAttachedTo/OnDetachedFrom.")]
partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	UIKit.UITextField? textField;
	UIKit.UITextView? textView;
	UIKit.UIColor? originalBackgroundColor;
	UIKit.UITextBorderStyle originalBorderStyle = UIKit.UITextBorderStyle.RoundedRect;
	UIKit.IUITextFieldDelegate? originalTextFieldDelegate;
	UIKit.IUITextViewDelegate? originalTextViewDelegate;

	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, UIKit.UIView platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		if (platformView is UIKit.UITextField textField)
		{
			originalBackgroundColor = textField.BackgroundColor;
			originalBorderStyle = textField.BorderStyle;
			originalTextFieldDelegate = textField.Delegate;
			textField.Delegate = new BlockingTextFieldDelegate(this, textField);
			this.textField = textField;
			UpdateBorderThickness();
		}
		else if (platformView is UIKit.UITextView textView)
		{
			originalBackgroundColor = textView.BackgroundColor;
			originalTextViewDelegate = textView.Delegate;
			textView.Delegate = new BlockingTextViewDelegate(this, textView);
			this.textView = textView;
			UpdateBorderThickness();
		}
	}

	/// <inheritdoc />
	protected override void OnDetachedFrom(InputView bindable, UIKit.UIView platformView)
	{
		if (textField is not null)
		{
			textField.BackgroundColor = originalBackgroundColor;
			textField.BorderStyle = originalBorderStyle;
			if (originalTextFieldDelegate is not null)
			{
				textField.Delegate = originalTextFieldDelegate;
				originalTextFieldDelegate = null;
			}
			textField = null;
		}

		if (textView is not null)
		{
			textView.BackgroundColor = originalBackgroundColor;
			if (originalTextViewDelegate is not null)
			{
				textView.Delegate = originalTextViewDelegate;
				originalTextViewDelegate = null;
			}
			textView = null;
		}

		base.OnDetachedFrom(bindable, platformView);
	}

	partial void UpdateBorderThickness()
	{
		if (textField is not null)
		{
			textField.Layer.BorderWidth = (System.Runtime.InteropServices.NFloat)BorderThickness;
			textField.BackgroundColor = BorderThickness == 0 ? UIKit.UIColor.Clear : originalBackgroundColor;
			textField.BorderStyle = BorderThickness == 0 ? UIKit.UITextBorderStyle.None : originalBorderStyle;
		}

		if (textView is not null)
		{
			textView.Layer.BorderWidth = (System.Runtime.InteropServices.NFloat)BorderThickness;
			textView.BackgroundColor = BorderThickness == 0 ? UIKit.UIColor.Clear : originalBackgroundColor;
		}
	}

	partial void UpdateInputMask()
	{
	}

	class BlockingTextFieldDelegate : UIKit.UITextFieldDelegate
	{
		InputExtrasBehavior owner;
		public BlockingTextFieldDelegate(InputExtrasBehavior owner, UIKit.UITextField view) => this.owner = owner;
		public override bool ShouldChangeCharacters(UIKit.UITextField textField, Foundation.NSRange range, string replacementString)
			=> BlockingTextHelper.ShouldChangeText(owner, textField.Text ?? string.Empty, range, replacementString);
	}

	class BlockingTextViewDelegate : UIKit.UITextViewDelegate
	{
		InputExtrasBehavior owner;
		public BlockingTextViewDelegate(InputExtrasBehavior owner, UIKit.UITextView view) => this.owner = owner;
		public override bool ShouldChangeText(UIKit.UITextView textView, Foundation.NSRange range, string replacementString)
			=> BlockingTextHelper.ShouldChangeText(owner, textView.Text ?? string.Empty, range, replacementString);
	}

	static class BlockingTextHelper
	{
		public static bool ShouldChangeText(InputExtrasBehavior owner, string oldText, Foundation.NSRange range, string replacementString)
		{
			switch (owner.InputMask)
			{
				case InputMask.None:
					return true;
				case InputMask.Integer:
					return IntegerRegex().IsMatch(oldText.Substring(0, (int)range.Location) + replacementString + oldText.Substring((int)(range.Location + range.Length)));
				case InputMask.Decimal:
					return DecimalRegex().IsMatch(oldText.Substring(0, (int)range.Location) + replacementString + oldText.Substring((int)(range.Location + range.Length)));
				case InputMask.Pattern:
					return System.Text.RegularExpressions.Regex.IsMatch(oldText.Substring(0, (int)range.Location) + replacementString + oldText.Substring((int)(range.Location + range.Length)), owner.InputPattern ?? string.Empty);
			}
			return true;
		}
	}
}
