// BorderlessBehavior.MaciOS.cs

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
public partial class BorderlessBehavior : PlatformBehavior<InputView>
{
	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, UIKit.UIView platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		switch (platformView)
		{
			case UIKit.UITextField textField:
				textField.Layer.BorderWidth = 0;
				break;
			case UIKit.UITextView textView:
				textView.Layer.BorderWidth = 0;
				break;
		}
	}
}
