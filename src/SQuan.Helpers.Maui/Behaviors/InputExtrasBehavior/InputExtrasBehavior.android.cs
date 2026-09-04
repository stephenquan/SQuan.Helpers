// InputExtrasBehavior.android.cs

using System.Text.RegularExpressions;

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	Android.Widget.EditText? editText;
	Android.Text.Method.IKeyListener? originalKeyListener;
	string originalText = string.Empty;
	Android.Text.Method.DigitsKeyListener integerKeyListener = Android.Text.Method.DigitsKeyListener.GetInstance("-0123456789");
	Android.Text.Method.DigitsKeyListener decimalKeyListener = Android.Text.Method.DigitsKeyListener.GetInstance("-0123456789.,");

	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, Android.Views.View platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		if (platformView is Android.Widget.EditText editText)
		{
			this.editText = editText;
			originalKeyListener = editText.KeyListener;
			editText.BeforeTextChanged += EditText_BeforeTextChanged;
			editText.TextChanged += EditText_TextChanged;
			UpdateBorderThickness();
			UpdateInputMask();
		}
	}

	protected override void OnDetachedFrom(InputView bindable, Android.Views.View platformView)
	{
		if (editText is not null)
		{
			editText.BeforeTextChanged -= EditText_BeforeTextChanged;
			editText.TextChanged -= EditText_TextChanged;
			editText.KeyListener = originalKeyListener;
			editText = null;
		}
		base.OnDetachedFrom(bindable, platformView);
	}

	void EditText_BeforeTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
	{
		originalText = e.Text?.ToString() ?? string.Empty;
	}

	void EditText_TextChanged(object? sender, Android.Text.TextChangedEventArgs e)
	{
		switch (InputMode)
		{
			case InputMode.None:
				return;
			case InputMode.Integer:
				RevertIfNotMatchPattern(sender, e.Text?.ToString() ?? string.Empty, IntegerRegex());
				return;
			case InputMode.Decimal:
				RevertIfNotMatchPattern(sender, e.Text?.ToString() ?? string.Empty, DecimalRegex());
				return;
			case InputMode.Pattern:
				RevertIfNotMatchPattern(sender, e.Text?.ToString() ?? string.Empty, InputPattern ?? string.Empty);
				break;
			default:
				break;
		}
	}

	void RevertIfNotMatchPattern(object? sender, string newText, Regex regex)
	{
		if (sender is Android.Widget.EditText editText
			&& !string.IsNullOrEmpty(newText)
			&& !regex.IsMatch(newText))
		{
			editText.Text = originalText;
			editText.SetSelection(editText?.Text?.Length ?? 0);
		}
	}

	void RevertIfNotMatchPattern(object? sender, string newText, string pattern)
	{
		if (sender is not Android.Widget.EditText editText
			|| string.IsNullOrEmpty(newText)
			|| string.IsNullOrEmpty(pattern))
		{
			return;
		}

		bool isMatch = false;
		try
		{
			isMatch = Regex.IsMatch(newText, pattern);
		}
		catch (ArgumentException)
		{
			return;
		}

		if (!isMatch)
		{
			editText.Text = originalText;
			editText.SetSelection(editText?.Text?.Length ?? 0);
		}
	}

	partial void UpdateBorderThickness()
	{
		if (editText is null)
		{
			return;
		}

		var localEditText = editText;

		if (BorderThickness == 0)
		{
			this.Dispatcher.Dispatch(() =>
			{
				localEditText.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
			});
			return;
		}

		this.Dispatcher.Dispatch(() =>
		{
			localEditText.BackgroundTintList = null;
		});
	}

	partial void UpdateInputMask()
	{
		if (editText is null)
		{
			return;
		}

		editText.KeyListener = InputMode switch
		{
			InputMode.None => originalKeyListener,
			InputMode.Integer => integerKeyListener,
			InputMode.Decimal => decimalKeyListener,
			_ => originalKeyListener
		};
	}
}
