// AppThemeExtension.cs

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides a markup extension that dynamically resolves a value based on the application's current theme.
/// </summary>
public partial class AppThemeExtension : BaseBindableObjectMarkupExtension
{
	/// <summary>
	/// Bindable property for the <see cref="Light"/>.
	/// </summary>
	public static readonly BindableProperty LightProperty
		= BindableProperty.Create(nameof(Light), typeof(object), typeof(AppThemeExtension), default(object));

	/// <summary>
	/// Gets or sets the value that will be used when the application is in light theme.
	/// </summary>
	public object? Light
	{
		get => GetValue(LightProperty);
		set => SetValue(LightProperty, value);
	}

	/// <summary>
	/// Gets or sets the value that will be used when the application is in dark theme.
	/// </summary>
	public static readonly BindableProperty DarkProperty
		= BindableProperty.Create(nameof(Dark), typeof(object), typeof(AppThemeExtension), default(object));

	/// <summary>
	/// Gets or sets the value that will be used when the application is in dark theme.
	/// </summary>
	public object? Dark
	{
		get => GetValue(DarkProperty);
		set => SetValue(DarkProperty, value);
	}

	/// <summary>
	/// Provides the binding value based on the current application theme.
	/// </summary>
	/// <param name="serviceProvider">An object that provides services for the markup extension.</param>
	/// <returns>The binding value based on the current application theme.</returns>
	public override BindingBase ProvideBindingValue(IServiceProvider serviceProvider)
	{
		return AppThemeBindingBase.Create(
			BindingBase.Create(static (AppThemeExtension ctx) => ctx.Light, BindingMode.OneWay, source: this),
			BindingBase.Create(static (AppThemeExtension ctx) => ctx.Dark, BindingMode.OneWay, source: this));
	}
}
