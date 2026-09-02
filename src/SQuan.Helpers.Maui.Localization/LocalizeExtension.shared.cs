// LocalizeExtension.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A markup extension that provides localized strings based on a specified key.
/// </summary>
[ContentProperty(nameof(Key))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class LocalizeExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Bindable property for <see cref="Key"/>.
	/// </summary>
	public static readonly BindableProperty KeyProperty =
		BindableProperty.Create(nameof(Key), typeof(string), typeof(LocalizeExtension), default(string));

	/// <summary>
	/// Gets or sets the localization key for the string to be translated.
	/// </summary>
	public string Key
	{
		get => (string)GetValue(KeyProperty);
		set => SetValue(KeyProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X0"/>.
	/// </summary>
	public static readonly BindableProperty X0Property =
		BindableProperty.Create(nameof(X0), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {0} to be used for formatting the localized string.
	/// </summary>
	public object? X0
	{
		get => GetValue(X0Property) as object;
		set => SetValue(X0Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X1"/>.
	/// </summary>
	public static readonly BindableProperty X1Property =
		BindableProperty.Create(nameof(X1), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {1} to be used for formatting the localized string.
	/// </summary>
	public object? X1
	{
		get => GetValue(X1Property) as object;
		set => SetValue(X1Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X2"/>.
	/// </summary>
	public static readonly BindableProperty X2Property =
		BindableProperty.Create(nameof(X2), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {2} to be used for formatting the localized string.
	/// </summary>
	public object? X2
	{
		get => GetValue(X2Property) as object;
		set => SetValue(X2Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X3"/>.
	/// </summary>
	public static readonly BindableProperty X3Property =
		BindableProperty.Create(nameof(X3), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {3} to be used for formatting the localized string.
	/// </summary>
	public object? X3
	{
		get => GetValue(X3Property) as object;
		set => SetValue(X3Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X4"/>.
	/// </summary>
	public static readonly BindableProperty X4Property =
		BindableProperty.Create(nameof(X4), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {4} to be used for formatting the localized string.
	/// </summary>
	public object? X4
	{
		get => GetValue(X4Property) as object;
		set => SetValue(X4Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X5"/>.
	/// </summary>
	public static readonly BindableProperty X5Property =
		BindableProperty.Create(nameof(X5), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {5} to be used for formatting the localized string.
	/// </summary>
	public object? X5
	{
		get => GetValue(X5Property) as object;
		set => SetValue(X5Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X6"/>.
	/// </summary>
	public static readonly BindableProperty X6Property =
		BindableProperty.Create(nameof(X6), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {6} to be used for formatting the localized string.
	/// </summary>
	public object? X6
	{
		get => GetValue(X6Property) as object;
		set => SetValue(X6Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X7"/>.
	/// </summary>
	public static readonly BindableProperty X7Property =
		BindableProperty.Create(nameof(X7), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {7} to be used for formatting the localized string.
	/// </summary>
	public object? X7
	{
		get => GetValue(X7Property) as object;
		set => SetValue(X7Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X8"/>.
	/// </summary>
	public static readonly BindableProperty X8Property =
		BindableProperty.Create(nameof(X8), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {8} to be used for formatting the localized string.
	/// </summary>
	public object? X8
	{
		get => GetValue(X8Property) as object;
		set => SetValue(X8Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="X9"/>.
	/// </summary>
	public static readonly BindableProperty X9Property =
		BindableProperty.Create(nameof(X9), typeof(object), typeof(LocalizeExtension), default(object));

	/// <summary>
	/// Gets or sets the argument value for {9} to be used for formatting the localized string.
	/// </summary>
	public object? X9
	{
		get => GetValue(X9Property) as object;
		set => SetValue(X9Property, value);
	}

	/// <summary>
	/// Bindable property for <see cref="InternalCurrentUICulture"/>.
	/// </summary>
	public static readonly BindableProperty InternalCurrentUICultureProperty =
		BindableProperty.Create(nameof(InternalCurrentUICulture), typeof(CultureInfo), typeof(LocalizeExtension), default(CultureInfo));

	/// <summary>
	/// Gets or sets the current culture to be used for localization.
	/// </summary>
	public CultureInfo? InternalCurrentUICulture
	{
		get => GetValue(InternalCurrentUICultureProperty) as CultureInfo;
		set => SetValue(InternalCurrentUICultureProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="InternalCurrentCulture"/>.
	/// </summary>
	public static readonly BindableProperty InternalCurrentCultureProperty =
		BindableProperty.Create(nameof(InternalCurrentCulture), typeof(CultureInfo), typeof(LocalizeExtension), default(CultureInfo));

	/// <summary>
	/// Gets or sets the current culture to be used for localization.
	/// </summary>
	public CultureInfo? InternalCurrentCulture
	{
		get => GetValue(InternalCurrentCultureProperty) as CultureInfo;
		set => SetValue(InternalCurrentCultureProperty, value);
	}

	BindableObject? targetObject;

	/// <summary>
	/// Provides a binding object based on the specified service provider.
	/// </summary>
	/// <param name="serviceProvider">An object that provides services for the binding. This parameter is typically used to resolve services or context
	/// information required to create the binding.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the binding to be used. The specific type and configuration of
	/// the binding depend on the implementation.</returns>
	BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
	{
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget
			&& provideValueTarget.TargetObject is BindableObject targetObject)
		{
			this.targetObject = targetObject;
			if (!IsSet(BindingContextProperty))
			{
				this.SetBinding(BindingContextProperty, static (BindableObject t) => t.BindingContext, BindingMode.OneWay, source: targetObject);
			}
		}

		return new MultiBinding
		{
			Bindings =
			{
				BindingBase.Create(static (LocalizeExtension ctx) => ctx.Key, BindingMode.OneWay, source: this),
				new MultiBinding
				{
					Bindings =
					{
						BindingBase.Create(static (LocalizeExtension ctx) => ctx.InternalCurrentUICulture, BindingMode.OneWay, source: this),
						BindingBase.Create(static (LocalizationManager lm) => lm.CurrentUICulture, BindingMode.OneWay, source: LocalizationManager.Current)
					},
					Mode = BindingMode.OneWay,
					Converter = new SelectCultureMultiValueConverter(),
				},
				new MultiBinding
				{
					Bindings =
					{
						BindingBase.Create(static (LocalizeExtension ctx) => ctx.InternalCurrentCulture, BindingMode.OneWay, source: this),
						BindingBase.Create(static (LocalizationManager lm) => lm.CurrentCulture, BindingMode.OneWay, source: LocalizationManager.Current)
					},
					Mode = BindingMode.OneWay,
					Converter = new SelectCultureMultiValueConverter(),
				},
				new MultiBinding
				{
					Bindings =
					{
						new Binding("X0", BindingMode.OneWay, source: this),
						new Binding("X1", BindingMode.OneWay, source: this),
						new Binding("X2", BindingMode.OneWay, source: this),
						new Binding("X3", BindingMode.OneWay, source: this),
						new Binding("X4", BindingMode.OneWay, source: this),
						new Binding("X5", BindingMode.OneWay, source: this),
						new Binding("X6", BindingMode.OneWay, source: this),
						new Binding("X7", BindingMode.OneWay, source: this),
						new Binding("X8", BindingMode.OneWay, source: this),
						new Binding("X9", BindingMode.OneWay, source: this)
					},
					Mode = BindingMode.OneWay,
					Converter = new PassThruMultiValueConverter()
				},
			},
			Mode = BindingMode.OneWay,
			Converter = new LocalizeMultiValueConverter()
		};
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);

	/// <inheritdoc/>
	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (targetObject is VisualElement targetElement && targetElement.GetLocalizeBehavior() is LocalizeGroupBehavior behavior)
		{
			this.SetBinding(InternalCurrentUICultureProperty, static (LocalizeGroupBehavior b) => b.CurrentUICulture, BindingMode.OneWay, source: behavior);
			this.SetBinding(InternalCurrentCultureProperty, static (LocalizeGroupBehavior b) => b.CurrentCulture, BindingMode.OneWay, source: behavior);
		}
	}
}
