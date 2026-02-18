// StatusBarExtras.cs

using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides attached bindable properties for customizing the status bar appearance in a .NET MAUI application.
/// </summary>
[AttachedBindableProperty<Color>("StatusBarColor", CoerceValueMethodName = nameof(CoerceStatusBarColor))]
[AttachedBindableProperty<StatusBarStyle>("StatusBarStyle", CoerceValueMethodName = nameof(CoerceStatusBarStyle))]
public static partial class StatusBarExtras
{
	static object CoerceStatusBarColor(BindableObject bindable, object value)
		=> PlatformInvokeWithCoerceValue<Color?>(value, CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor);
	static object CoerceStatusBarStyle(BindableObject bindable, object value)
		=> PlatformInvokeWithCoerceValue<StatusBarStyle>(value, CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle);
	static object PlatformInvokeWithCoerceValue<T>(object value, Action<T> action)
	{
#if ANDROID || IOS
		action((T)value);
#endif
		return value;
	}
}
