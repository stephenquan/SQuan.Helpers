namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides a markup extension that dynamically resolves a value based on the application's current theme.
/// </summary>
/// <remarks>This class allows developers to specify values for light and dark themes, which are automatically
/// resolved at runtime based on the application's current theme. It is commonly used in XAML to bind UI elements to
/// theme-specific values.</remarks>
public partial class AppThemeExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Identifies the bindable property for the light theme value.
	/// </summary>
	/// <remarks>This property is used to define the value associated with the light theme in the application. It
	/// can be bound to a UI element or other components to dynamically adjust behavior or appearance based on the light
	/// theme.</remarks>
	public static readonly BindableProperty LightProperty =
		BindableProperty.Create(nameof(Light), typeof(object), typeof(AppThemeExtension));

	/// <summary>
	/// Gets or sets the value associated with the <see cref="LightProperty"/> dependency property.
	/// </summary>
	public object? Light
	{
		get => GetValue(LightProperty);
		set => SetValue(LightProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="Dark"/> value.
	/// </summary>
	public static readonly BindableProperty DarkProperty =
		BindableProperty.Create(nameof(Dark), typeof(object), typeof(AppThemeExtension));

	/// <summary>
	/// Gets or sets the value that will be used when the application is in dark theme.
	/// </summary>
	public object? Dark
	{
		get => GetValue(DarkProperty);
		set => SetValue(DarkProperty, value);
	}

	/// <summary>
	/// Gets the value corresponding to the application's current theme.
	/// </summary>
	public object? ResultValue
	{
		get
		{
			if (Application.Current is Application app)
			{
				switch (app.RequestedTheme)
				{
					case AppTheme.Light:
						return Light;
					case AppTheme.Dark:
						return Dark;
				}
			}
			return Light;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AppThemeExtension"/> class.
	/// </summary>
	/// <remarks>This constructor sets up the <see cref="AppThemeExtension"/> object for use.  Additional
	/// configuration or initialization may be required depending on the intended usage.</remarks>
	public AppThemeExtension()
	{
		if (Application.Current is Application app)
		{
			app.RequestedThemeChanged += (s, e) =>
			{
				OnPropertyChanged(nameof(ResultValue));
			};
		}
	}

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
		return BindingBase.Create(static (object? o) => o, BindingMode.OneWay, source: ResultValue);
	}
}
