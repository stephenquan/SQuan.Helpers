// InputExtrasBehavior.windows.cs

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	Microsoft.UI.Xaml.Controls.TextBox? textBox;

	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, Microsoft.UI.Xaml.FrameworkElement platformView)
	{
		base.OnAttachedTo(bindable, platformView);

		if (platformView is Microsoft.UI.Xaml.Controls.TextBox textBox)
		{
			textBox.BeforeTextChanging += TextBox_BeforeTextChanging;
			this.textBox = textBox;
			UpdateBorderThickness();
		}
	}

	/// <inheritdoc />
	protected override void OnDetachedFrom(InputView bindable, Microsoft.UI.Xaml.FrameworkElement platformView)
	{
		if (textBox is not null)
		{
			textBox.BeforeTextChanging -= TextBox_BeforeTextChanging;
			textBox = null;
		}

		base.OnDetachedFrom(bindable, platformView);
	}

	void TextBox_BeforeTextChanging(Microsoft.UI.Xaml.Controls.TextBox sender, Microsoft.UI.Xaml.Controls.TextBoxBeforeTextChangingEventArgs args)
	{
		switch (InputMode)
		{
			case InputMode.None:
				return;
			case InputMode.Integer:
				args.Cancel = !string.IsNullOrEmpty(args.NewText) && !IntegerRegex().IsMatch(args.NewText);
				return;
			case InputMode.Decimal:
				args.Cancel = !string.IsNullOrEmpty(args.NewText) && !DecimalRegex().IsMatch(args.NewText);
				return;
			case InputMode.Pattern:
				if (string.IsNullOrEmpty(args.NewText) || string.IsNullOrEmpty(InputPattern))
				{
					return;
				}
				try { args.Cancel = !System.Text.RegularExpressions.Regex.IsMatch(args.NewText, InputPattern); }
				catch (ArgumentException) { args.Cancel = false; }
				return;
			default:
				break;
		}
	}

	partial void UpdateBorderThickness()
	{
		if (textBox is not null)
		{
			textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(BorderThickness);
			textBox.Resources["TextControlBorderThemeThickness"] = new Microsoft.UI.Xaml.Thickness(BorderThickness);
			textBox.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(BorderThickness);
		}
	}

	partial void UpdateInputMask()
	{
	}
}
