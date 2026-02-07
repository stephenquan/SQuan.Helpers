// InputMaskBehavior.Windows.cs

using Microsoft.UI.Xaml.Controls;

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
public partial class InputMaskBehavior : PlatformBehavior<InputView>
{
	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, Microsoft.UI.Xaml.FrameworkElement platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		if (platformView is Microsoft.UI.Xaml.Controls.TextBox textBox)
		{
			textBox.BeforeTextChanging += TextBox_BeforeTextChanging;
		}
	}

	void TextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
	{
		if (this.Regex is string regex
			&& !string.IsNullOrEmpty(regex)
			&& !System.Text.RegularExpressions.Regex.IsMatch(args.NewText, regex))
		{
			args.Cancel = true;
		}
	}
}
