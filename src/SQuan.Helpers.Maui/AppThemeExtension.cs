// AppThemeExtension.cs

using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides a markup extension that dynamically resolves a value based on the application's current theme.
/// </summary>
public partial class AppThemeExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Gets or sets the value that will be used when the application is in light theme.
	/// </summary>
	[BindableProperty(UseStaticCallbacks = true)]
	public partial object? Light { get; set; }

	/// <summary>
	/// Gets or sets the value that will be used when the application is in dark theme.
	/// </summary>
	[BindableProperty(UseStaticCallbacks = true)]
	public partial object? Dark { get; set; }

	/// <summary>
	/// Provides the value of the markup extension for the specified service provider.
	/// </summary>
	/// <remarks>This method is typically called by the XAML processor during object creation to obtain the value of
	/// the markup extension.</remarks>
	/// <param name="serviceProvider">An object that provides services for the markup extension.</param>
	/// <returns>The object value provided by the markup extension. The returned value depends on the implementation of <see
	/// cref="IMarkupExtension{BindingBase}.ProvideValue(IServiceProvider)"/>.</returns>
	public object ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);

	BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
	{
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget && provideValueTarget.TargetObject is BindableObject targetObject)
		{
			this.SetBinding(BindableObject.BindingContextProperty, static (BindableObject t) => t.BindingContext, BindingMode.OneWay, source: targetObject);
		}
		return AppThemeBindingBase.Create(
			BindingBase.Create(static (object o) => o, BindingMode.OneWay, source: Light),
			BindingBase.Create(static (object o) => o, BindingMode.OneWay, source: Dark));
	}
}
