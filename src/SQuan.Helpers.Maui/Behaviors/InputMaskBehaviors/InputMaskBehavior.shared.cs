// InputMaskBehavior.shared.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides behavior for input views that enforces input masking based on a regular expression or a set of allowed keys.
/// </summary>
[Obsolete("InputMaskBehavior is deprecated. Please use attached properties InputExtras.InputMode and InputExtras.InputPattern and set them to the desired values instead.")]
public partial class InputMaskBehavior : Behavior<InputView>
{
	/// <summary>
	/// Gets or sets the regular expression pattern for the input mask.
	/// </summary>
	[BindableProperty(PropertyChangedMethodName = nameof(OnMaskPropertyChanged))]
	public partial string Regex { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the allowed keys for the input mask.
	/// </summary>
	[BindableProperty(PropertyChangedMethodName = nameof(OnMaskPropertyChanged))]
	public partial string Keys { get; set; } = string.Empty;

	InputView? view;

	/// <inheritdoc/>
	protected override void OnAttachedTo(InputView bindable)
	{
		base.OnAttachedTo(bindable);
		view = bindable;
		Apply();
	}

	/// <inheritdoc/>
	protected override void OnDetachingFrom(InputView bindable)
	{
		InputExtras.SetInputMode(bindable, InputMode.None);
		InputExtras.SetInputPattern(bindable, string.Empty);
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
			InputExtras.SetInputMask(view, InputMode.Pattern);
			InputExtras.SetInputPattern(view, Regex);
			return;
		}

		if (!string.IsNullOrEmpty(Keys))
		{
			var escapedKeys = System.Text.RegularExpressions.Regex.Escape(Keys);
			InputExtras.SetInputMask(view, InputMode.Pattern);
			InputExtras.SetInputPattern(view, $"^[{escapedKeys}]*$");
			return;
		}

		InputExtras.SetInputMask(view, InputMode.None);
		InputExtras.SetInputPattern(view, string.Empty);
	}
}
