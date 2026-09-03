// InputExtrasBehavior.macios.cs

using System.Diagnostics.CodeAnalysis;

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
[SuppressMessage(
	"Design",
	"CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
	Justification = "This behavior does not own delegate lifetimes. UIKit owns delegates; they are swapped and released in OnAttachedTo/OnDetachedFrom.")]
public partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	UIKit.UITextField? textField;
	UIKit.UITextView? textView;
	UIKit.UIColor? backgroundColor;
	UIKit.UITextBorderStyle borderStyle = UIKit.UITextBorderStyle.RoundedRect;
	//UIKit.IUITextFieldDelegate? originalTextFieldDelegate;
	//BlockingTextFieldDelegate? blockingTextFieldDelegate;
	//UIKit.IUITextViewDelegate? originalTextViewDelegate;
	//BlockingTextViewDelegate? blockingTextViewDelegate;

	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, UIKit.UIView platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		//switch (platformView)
		//{
		//	case UIKit.UITextField textField:
		//		originalTextFieldDelegate = textField.Delegate;
		//		blockingTextFieldDelegate = new BlockingTextFieldDelegate(this, textField);
		//		textField.Delegate = blockingTextFieldDelegate;
		//		break;
		//	case UIKit.UITextView textView:
		//		originalTextViewDelegate = textView.Delegate;
		//		blockingTextViewDelegate = new BlockingTextViewDelegate(this, textView);
		//		textView.Delegate = blockingTextViewDelegate;
		//		break;
		//}
		//switch (platformView)
		//{
		//	case UIKit.UITextField textField:
		//		textField.Layer.BorderWidth = 0;
		//		break;
		//	case UIKit.UITextView textView:
		//		textView.Layer.BorderWidth = 0;
		//		break;
		//}
		if (platformView is UIKit.UITextField textField)
		{
			this.backgroundColor = textField.BackgroundColor;
			this.borderStyle = textField.BorderStyle;
			this.textField = textField;
			UpdateBorderThickness();
		}
		else if (platformView is UIKit.UITextView textView)
		{
			this.textView = textView;
			UpdateBorderThickness();
		}
	}

	/// <inheritdoc />
	protected override void OnDetachedFrom(InputView bindable, UIKit.UIView platformView)
	{
		textField = null;
		textView = null;
	}

	//		case UIKit.UITextField textField:
	//			if (originalTextFieldDelegate is not null)
	//			{
	//				textField.Delegate = originalTextFieldDelegate;
	//				originalTextFieldDelegate = null;
	//			}
	//			blockingTextFieldDelegate = null;
	//			break;
	//		case UIKit.UITextView textView:
	//			if (originalTextViewDelegate is not null)
	//			{
	//				textView.Delegate = originalTextViewDelegate;
	//				originalTextViewDelegate = null;
	//			}
	//			blockingTextViewDelegate = null;
	//			break;
	//	}
	//	base.OnDetachedFrom(bindable, platformView);
	//}

	//class BlockingTextFieldDelegate : UIKit.UITextFieldDelegate
	//{
	//	InputMaskBehavior owner;
	//	public BlockingTextFieldDelegate(InputMaskBehavior owner, UIKit.UITextField view) => this.owner = owner;
	//	public override bool ShouldChangeCharacters(UIKit.UITextField textField, Foundation.NSRange range, string replacementString) => BlockingTextHelper.ShouldChangeText(owner, textField.Text ?? string.Empty, range, replacementString);
	//}

	//class BlockingTextViewDelegate : UIKit.UITextViewDelegate
	//{
	//	InputMaskBehavior owner;
	//	public BlockingTextViewDelegate(InputMaskBehavior owner, UIKit.UITextView view) => this.owner = owner;
	//	public override bool ShouldChangeText(UIKit.UITextView textView, Foundation.NSRange range, string replacementString) => BlockingTextHelper.ShouldChangeText(owner, textView.Text ?? string.Empty, range, replacementString);
	//}

	//static class BlockingTextHelper
	//{
	//	public static bool ShouldChangeText(InputMaskBehavior owner, string oldText, Foundation.NSRange range, string replacementString)
	//	{
	//		if (owner.Regex is string regex && !string.IsNullOrEmpty(regex))
	//		{
	//			string newText = oldText.Substring(0, (int)range.Location) + replacementString + oldText.Substring((int)(range.Location + range.Length));
	//			return System.Text.RegularExpressions.Regex.IsMatch(newText, regex);
	//		}
	//		return true;
	//	}
	//}

	partial void UpdateBorderThickness()
	{
		if (textField is not null)
		{
			textField.Layer.BorderWidth = (System.Runtime.InteropServices.NFloat)BorderThickness;
			if (BorderThickness == 0)
			{
				textField.BackgroundColor = UIKit.UIColor.Clear;
				textField.BorderStyle = UIKit.UITextBorderStyle.None;
			}
			else
			{
				textField.BackgroundColor = backgroundColor;
				textField.BorderStyle = borderStyle;
			}
		}
		if (textView is not null)
		{
			textView.Layer.BorderWidth = (System.Runtime.InteropServices.NFloat)BorderThickness;
		}
	}

	partial void UpdateMaskMode()
	{
	}
}
