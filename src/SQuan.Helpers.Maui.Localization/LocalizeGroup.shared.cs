// LocalizeGroup.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides attached properties for managing localization settings in a .NET MAUI application.
/// </summary>
public partial class LocalizeGroup
{
	/// <summary>
	/// Bindable property for setting the current UI culture in a .NET MAUI application.
	/// </summary>
	public static readonly BindableProperty CurrentUICultureProperty
		= BindableProperty.CreateAttached(
			"CurrentUICulture", typeof(CultureInfo), typeof(LocalizeGroup), null, coerceValue: OnCoerceCurrentUICulture);

	/// <summary>
	/// Gets the current UI culture for the specified bindable object.
	/// </summary>
	/// <param name="bindable">The bindable object from which to get the current UI culture.</param>
	/// <returns>The current UI culture of the specified bindable object.</returns>
	public static CultureInfo? GetCurrentUICulture(BindableObject bindable)
	{
		return (CultureInfo?)bindable.GetValue(CurrentUICultureProperty);
	}

	/// <summary>
	/// Sets the current UI culture for the specified bindable object.
	/// </summary>
	/// <param name="bindable">The bindable object on which to set the current UI culture.</param>
	/// <param name="value">The culture info value to set as the current UI culture.</param>
	public static void SetCurrentUICulture(BindableObject bindable, CultureInfo? value)
	{
		bindable.SetValue(CurrentUICultureProperty, value);
	}

	/// <summary>
	/// Bindable property for setting the current culture in a .NET MAUI application.
	/// </summary>
	public static readonly BindableProperty CurrentCultureProperty
		= BindableProperty.CreateAttached(
			"CurrentCulture", typeof(CultureInfo), typeof(LocalizeGroup), null, coerceValue: OnCoerceCurrentCulture);

	/// <summary>
	/// Gets the current culture for the specified bindable object.
	/// </summary>
	/// <param name="bindable">The bindable object from which to get the current culture.</param>
	/// <returns>The current culture of the specified bindable object.</returns>
	public static CultureInfo? GetCurrentCulture(BindableObject bindable)
	{
		return (CultureInfo?)bindable.GetValue(CurrentCultureProperty);
	}

	/// <summary>
	/// Sets the current culture for the specified bindable object.
	/// </summary>
	/// <param name="bindable">The bindable object on which to set the current culture.</param>
	/// <param name="value">The culture info value to set as the current culture.</param>
	public static void SetCurrentCulture(BindableObject bindable, CultureInfo? value)
	{
		bindable.SetValue(CurrentCultureProperty, value);
	}

	static LocalizeGroupBehavior GetLocalizeBehavior(VisualElement element)
	{
		if (element.Behaviors.OfType<LocalizeGroupBehavior>().FirstOrDefault() is LocalizeGroupBehavior localizeBehavior)
		{
			return localizeBehavior;
		}

		localizeBehavior = new LocalizeGroupBehavior();
		element.Behaviors.Add(localizeBehavior);
		return localizeBehavior;
	}

	static object OnCoerceCurrentUICulture(BindableObject bindable, object value)
	{
		if (bindable is VisualElement element)
		{
			CultureInfo? cultureInfo = value as CultureInfo;
			GetLocalizeBehavior(element).CurrentUICulture = cultureInfo;
		}
		return value;
	}

	static object OnCoerceCurrentCulture(BindableObject bindable, object value)
	{
		if (bindable is VisualElement element)
		{
			GetLocalizeBehavior(element).CurrentCulture = value as CultureInfo;
		}
		return value;
	}
}
