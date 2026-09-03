// VisualElementExtensions.shared.cs

namespace SQuan.Helpers.Maui;

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
