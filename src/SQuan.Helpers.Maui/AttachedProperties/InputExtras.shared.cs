// InputExtras.shared.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides the InputExtras attached property.
/// </summary>
[AttachedBindableProperty<double>("BorderThickness", DefaultValue = 1.0, CoerceValueMethodName = nameof(OnCoerceBorderThickness))]
[AttachedBindableProperty<InputMode>("InputMode", DefaultValue = InputMode.None, CoerceValueMethodName = nameof(OnCoerceInputMode))]
[AttachedBindableProperty<string>("InputPattern", DefaultValue = "", CoerceValueMethodName = nameof(OnCoerceInputPattern))]
public partial class InputExtras
{
	static object OnCoerceBorderThickness(BindableObject bindable, object value)
		=> OnCoerceValue(bindable, value, (behavior, v) => behavior.BorderThickness = (double)v);
	static object OnCoerceInputMode(BindableObject bindable, object value)
		=> OnCoerceValue(bindable, value, (behavior, v) => behavior.InputMode = (InputMode)v);
	static object OnCoerceInputPattern(BindableObject bindable, object value)
		=> OnCoerceValue(bindable, value, (behavior, v) => behavior.InputPattern = (string)v);
	static object OnCoerceValue(BindableObject bindable, object value, Action<InputExtrasBehavior, object> apply)
	{
		if (bindable is VisualElement element)
		{
			var behavior = element.GetOrCreateBehavior<InputExtrasBehavior>();
			apply(behavior, value);
		}
		return value;
	}
}
