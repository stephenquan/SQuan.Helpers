// InputExtrasBehavior.android.cs

namespace SQuan.Helpers.Maui;

/// <inheritdoc />
public partial class InputExtrasBehavior : PlatformBehavior<InputView>
{
	//string? oldText;
	//Android.Text.Method.IKeyListener? originalKeyListener;
	Android.Widget.EditText? editText;

	/// <inheritdoc />
	protected override void OnAttachedTo(InputView bindable, Android.Views.View platformView)
	{
		base.OnAttachedTo(bindable, platformView);
		if (platformView is Android.Widget.EditText editText)
		{
			this.editText = editText;
			UpdateBorderThickness();
		}
		//	originalKeyListener = editText.KeyListener;
		//	editText.BeforeTextChanged += EditText_BeforeTextChanged;
		//	editText.TextChanged += EditText_TextChanged;
		//	if (this.Keys is string keys && !string.IsNullOrEmpty(keys))
		//	{
		//		editText.KeyListener = Android.Text.Method.DigitsKeyListener.GetInstance(keys);
		//	}
		//}
		//platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
	}

	protected override void OnDetachedFrom(InputView bindable, Android.Views.View platformView)
	{
		editText = null;
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
		if (editText is not null)
		{
			if (BorderThickness == 0)
			{
				this.Dispatcher.Dispatch(() =>
				{
					editText.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
				});
			}
			else
			{
				this.Dispatcher.Dispatch(() =>
				{
					editText.BackgroundTintList = null;
				});
			}
		}
	}
}