// InputMaskBehavior.Android.cs

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
public partial class InputMaskBehavior : PlatformBehavior<InputView>
{
	string? oldText;
	Android.Text.Method.IKeyListener? originalKeyListener;

	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, Android.Views.View platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		if (platformView is Android.Widget.EditText editText)
		{
			originalKeyListener = editText.KeyListener;
			editText.BeforeTextChanged += EditText_BeforeTextChanged;
			editText.TextChanged += EditText_TextChanged;
			if (this.Keys is string keys && !string.IsNullOrEmpty(keys))
			{
				editText.KeyListener = Android.Text.Method.DigitsKeyListener.GetInstance(keys);
			}
		}
	}

	/// <inheritdoc />
	protected override void OnDetachedFrom(InputView bindable, Android.Views.View platformView)
	{
		if (platformView is Android.Widget.EditText editText)
		{
			editText.BeforeTextChanged -= EditText_BeforeTextChanged;
			editText.TextChanged -= EditText_TextChanged;
			editText.KeyListener = originalKeyListener;
		}
		base.OnDetachedFrom(bindable, platformView);
	}

	void EditText_BeforeTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
	{
		oldText = e.Text?.ToString() ?? string.Empty;
	}

	void EditText_TextChanged(object? sender, Android.Text.TextChangedEventArgs e)
	{
		if (sender is Android.Widget.EditText editText
			&& editText.Text is string newText
			&& this.Regex is string regex
			&& !string.IsNullOrEmpty(regex)
			&& !System.Text.RegularExpressions.Regex.IsMatch(newText, regex))
		{
			editText.Text = oldText;
			editText.SetSelection(editText?.Text?.Length ?? 0);
		}
	}
}
