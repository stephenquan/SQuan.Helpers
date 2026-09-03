// InputExtras.shared.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides the InputExtras attached property.
/// </summary>
[AttachedBindableProperty<double>("BorderThickness", DefaultValue = 1.0, CoerceValueMethodName = nameof(OnCoerceBorderThicnkess))]
public partial class InputExtras
{
	static object OnCoerceBorderThicnkess(BindableObject bindable, object value)
	{
		if (bindable is VisualElement element)
		{
			var behavior = GetOrCreateBehavior<InputExtrasBehavior>(element);
			behavior.BorderThickness = (double)value;
		}
		return value;
	}

	static T GetOrCreateBehavior<T>(VisualElement element) where T : Behavior, new()
	{
		if (element.Behaviors.OfType<T>().FirstOrDefault() is T behavior)
		{
			return behavior;
		}

		behavior = new T();
		element.Behaviors.Add(behavior);
		return behavior;
	}

}
