// InputMaskBehavior.shared.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides behavior for input views that enforces input masking based on a regular expression or a set of allowed keys.
/// </summary>
[Obsolete("InputMaskBehavior is deprecated. Please use attached properties InputExtras.InputMask and InputExtras.InputPattern and set to the desired value instead.")]
public class InputMaskBehavior : Behavior<InputView>
{
	public static readonly BindableProperty RegexProperty =
		BindableProperty.Create(nameof(Regex), typeof(string), typeof(InputMaskBehavior), default(string), propertyChanged: OnMaskPropertyChanged);

	public static readonly BindableProperty KeysProperty =
		BindableProperty.Create(nameof(Keys), typeof(string), typeof(InputMaskBehavior), default(string), propertyChanged: OnMaskPropertyChanged);

	InputView? view;

	public string? Regex
	{
		get => (string?)GetValue(RegexProperty);
		set => SetValue(RegexProperty, value);
	}

	public string? Keys
	{
		get => (string?)GetValue(KeysProperty);
		set => SetValue(KeysProperty, value);
	}

	protected override void OnAttachedTo(InputView bindable)
	{
		base.OnAttachedTo(bindable);
		view = bindable;
		Apply();
	}

	protected override void OnDetachingFrom(InputView bindable)
	{
		view = null;
		base.OnDetachingFrom(bindable);
	}

	static void OnMaskPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((InputMaskBehavior)bindable).Apply();

	void Apply()
	{
		if (view is null)
		{
			return;
		}

		if (!string.IsNullOrEmpty(Regex))
		{
			InputExtras.SetInputMask(view, InputMask.Pattern);
			InputExtras.SetInputPattern(view, Regex);
			return;
		}

		if (!string.IsNullOrEmpty(Keys))
		{
			var escapedKeys = System.Text.RegularExpressions.Regex.Escape(Keys);
			InputExtras.SetInputMask(view, InputMask.Pattern);
			InputExtras.SetInputPattern(view, $"^[{escapedKeys}]*$");
			return;
		}

		InputExtras.SetInputMask(view, InputMask.None);
		InputExtras.SetInputPattern(view, string.Empty);
	}
}
