// InputExtrasBehavior.windows.cs

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
public partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	Microsoft.UI.Xaml.Controls.TextBox? textBox;
	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, Microsoft.UI.Xaml.FrameworkElement platformView)
	{
		base.OnAttachedTo(bindable, platformView);

		if (platformView is Microsoft.UI.Xaml.Controls.TextBox textBox)
		{
			this.textBox = textBox;
			UpdateBorderThickness();
		}

		//if (platformView is Microsoft.UI.Xaml.Controls.TextBox textBox)
		//{
		//	textBox.BeforeTextChanging += TextBox_BeforeTextChanging;
		//}
		//if (platformView is Microsoft.UI.Xaml.Controls.TextBox textBox)
		//{
		//	textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
		//	textBox.Resources["TextControlBorderThemeThickness"] = new Microsoft.UI.Xaml.Thickness(0);
		//	textBox.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
		//}
	}

	/// <inheritdoc />
	protected override void OnDetachedFrom(InputView bindable, Microsoft.UI.Xaml.FrameworkElement platformView)
	{
		base.OnDetachedFrom(bindable, platformView);
		this.textBox = null;
	}

	//void TextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
	//{
	//	if (this.Regex is string regex
	//		&& !string.IsNullOrEmpty(regex)
	//		&& !System.Text.RegularExpressions.Regex.IsMatch(args.NewText, regex))
	//	{
	//		args.Cancel = true;
	//	}
	//}

	partial void UpdateBorderThickness()
	{
		if (textBox is not null)
		{
			textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(BorderThickness);
			textBox.Resources["TextControlBorderThemeThickness"] = new Microsoft.UI.Xaml.Thickness(BorderThickness);
			textBox.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(BorderThickness);
		}
	}
}
