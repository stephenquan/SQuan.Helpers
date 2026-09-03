// InputExtrasBehavior.android.cs

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
public partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	//string? oldText;

	Android.Widget.EditText? editText;
	Android.Text.Method.IKeyListener? keyListener;
	Android.Text.Method.DigitsKeyListener integerKeyListener = Android.Text.Method.DigitsKeyListener.GetInstance("-0123456789");
	Android.Text.Method.DigitsKeyListener decimalKeyListener = Android.Text.Method.DigitsKeyListener.GetInstance("-0123456789.,");

	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, Android.Views.View platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		if (platformView is Android.Widget.EditText editText)
		{
			this.editText = editText;
			keyListener = editText.KeyListener;
			UpdateBorderThickness();
			UpdateMaskMode();
		}
	}

	protected override void OnDetachedFrom(InputView bindable, Android.Views.View platformView)
	{
		if (editText is not null)
		{
			editText.KeyListener = keyListener;
			editText = null;
		}
	}

	///// <inheritdoc />
	//protected override void OnDetachedFrom(InputView bindable, Android.Views.View platformView)
	//{
	//	if (platformView is Android.Widget.EditText editText)
	//	{
	//		editText.BeforeTextChanged -= EditText_BeforeTextChanged;
	//		editText.TextChanged -= EditText_TextChanged;
	//		editText.KeyListener = originalKeyListener;
	//	}
	//	base.OnDetachedFrom(bindable, platformView);
	//}

	//void EditText_BeforeTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
	//{
	//	oldText = e.Text?.ToString() ?? string.Empty;
	//}

	//void EditText_TextChanged(object? sender, Android.Text.TextChangedEventArgs e)
	//{
	//	if (sender is Android.Widget.EditText editText
	//		&& editText.Text is string newText
	//		&& this.Regex is string regex
	//		&& !string.IsNullOrEmpty(regex)
	//		&& !System.Text.RegularExpressions.Regex.IsMatch(newText, regex))
	//	{
	//		editText.Text = oldText;
	//		editText.SetSelection(editText?.Text?.Length ?? 0);
	//	}
	//}

	partial void UpdateBorderThickness()
	{
		if (editText is null)
		{
			return;
		}

		if (BorderThickness == 0)
		{
			this.Dispatcher.Dispatch(() =>
			{
				editText.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
			});
			return;
		}

		this.Dispatcher.Dispatch(() =>
		{
			editText.BackgroundTintList = null;
		});
	}

	partial void UpdateMaskMode()
	{
		if (editText is null)
		{
			return;
		}

		editText.KeyListener = MaskMode switch
		{
			InputMaskMode.None => keyListener,
			InputMaskMode.Integer => integerKeyListener,
			InputMaskMode.Decimal => decimalKeyListener,
			_ => keyListener
		};
	}
}
