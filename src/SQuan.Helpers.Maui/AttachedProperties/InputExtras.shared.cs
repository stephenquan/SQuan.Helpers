// InputExtras.shared.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides the InputExtras attached properties.
/// </summary>
[AttachedBindableProperty<double>("BorderThickness", DefaultValue = 1.0, CoerceValueMethodName = nameof(OnCoerceBorderThickness))]
[AttachedBindableProperty<InputMode>("InputMode", DefaultValue = InputMode.None, CoerceValueMethodName = nameof(OnCoerceInputMode))]
[AttachedBindableProperty<string>("InputPattern", DefaultValue = "", CoerceValueMethodName = nameof(OnCoerceInputPattern))]
public partial class InputExtras
{
	static object OnCoerceBorderThickness(BindableObject bindable, object value)
	{
		if (value is double d && d < 0)
		{
			value = 0d;
		}
		return OnCoerceValue<double>(bindable, value, (b, v) => b.BorderThickness = v);
	}

	static object OnCoerceInputMode(BindableObject bindable, object value)
		=> OnCoerceValue<InputMode>(bindable, value, (b, v) => b.InputMode = v);

	static object OnCoerceInputPattern(BindableObject bindable, object value)
		=> OnCoerceValue<string>(bindable, value, (b, v) => b.InputPattern = v);

	static object OnCoerceValue<T>(BindableObject bindable, object value, Action<InputExtrasBehavior, T> apply)
	{
		if (bindable is InputView inputView && value is T valueT)
		{
			apply(inputView.GetOrCreateBehavior<InputExtrasBehavior>(), valueT);
		}
		return value;
	}
}
