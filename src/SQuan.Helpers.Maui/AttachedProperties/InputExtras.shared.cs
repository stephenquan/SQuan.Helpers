// InputExtras.shared.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides the InputExtras attached property.
/// </summary>
[AttachedBindableProperty<double>("BorderThickness", DefaultValue = 1.0, CoerceValueMethodName = nameof(OnCoerceBorderThicnkess))]
[AttachedBindableProperty<InputMaskMode>("MaskMode", DefaultValue = InputMaskMode.None, CoerceValueMethodName = nameof(OnCoerceMaskMode))]
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

	static object OnCoerceMaskMode(BindableObject bindable, object value)
	{
		if (bindable is VisualElement element)
		{
			var behavior = element.GetOrCreateBehavior<InputExtrasBehavior>();
			behavior.MaskMode = (InputMaskMode)value;
		}
		return value;
	}
}

/// <summary>
/// Provides extension methods for VisualElement.
/// </summary>
public static class VisualElementExtensions
{
	/// <summary>
	/// Gets an existing behavior of type T from the VisualElement or creates a new one if it doesn't exist.
	/// </summary>
	/// <typeparam name="T">The type of the behavior.</typeparam>
	/// <param name="element">The VisualElement to get or create the behavior for.</param>
	/// <returns>The existing or newly created behavior of type T.</returns>
	public static T GetOrCreateBehavior<T>(this VisualElement element) where T : Behavior, new()
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
