// InputExtras.shared.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides the InputExtras attached property.
/// </summary>
[AttachedBindableProperty<double>("BorderThickness", DefaultValue = 1.0, CoerceValueMethodName = nameof(OnCoerceBorderThicnkess))]
[AttachedBindableProperty<InputMask>("InputMask", DefaultValue = InputMask.None, CoerceValueMethodName = nameof(OnCoerceInputMask))]
[AttachedBindableProperty<string>("InputPattern", DefaultValue = "", CoerceValueMethodName = nameof(OnCoerceInputPattern))]
public partial class InputExtras
{
	static object OnCoerceBorderThicnkess(BindableObject bindable, object value)
	{
		if (bindable is VisualElement element)
		{
			var behavior = element.GetOrCreateBehavior<InputExtrasBehavior>();
			behavior.BorderThickness = (double)value;
		}
		return value;
	}

	static object OnCoerceInputMask(BindableObject bindable, object value)
	{
		if (bindable is VisualElement element)
		{
			var behavior = element.GetOrCreateBehavior<InputExtrasBehavior>();
			behavior.InputMask = (InputMask)value;
		}
		return value;
	}

	static object OnCoerceInputPattern(BindableObject bindable, object value)
	{
		if (bindable is VisualElement element)
		{
			var behavior = element.GetOrCreateBehavior<InputExtrasBehavior>();
			behavior.InputPattern = (string)value;
		}
		return value;
	}
}
