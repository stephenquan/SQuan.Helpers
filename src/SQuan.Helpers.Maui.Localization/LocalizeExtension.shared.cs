// LocalizeExtension.shared.cs

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A markup extension that provides localized strings based on a specified key.
/// </summary>
[ContentProperty(nameof(Key))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class LocalizeExtension : LocalizeBuilder, IMarkupExtension<BindingBase>
{
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
		get => GetValue(X0Property);
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
		get => GetValue(X1Property);
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
		get => GetValue(X2Property);
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
		get => GetValue(X3Property);
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
		get => GetValue(X4Property);
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
		get => GetValue(X5Property);
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
		get => GetValue(X6Property);
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
		get => GetValue(X7Property);
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
		get => GetValue(X8Property);
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
		get => GetValue(X9Property);
		set => SetValue(X9Property, value);
	}

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
			this.TargetObject = targetObject;
			if (!IsSet(BindingContextProperty))
			{
				this.SetBinding(BindingContextProperty, static (BindableObject t) => t.BindingContext, BindingMode.OneWay, source: targetObject);
			}
		}

		this.SetBinding(
			LocalizeBuilder.ArgsProperty,
			new MultiBinding
			{
				Bindings = new (BindableProperty argProperty, Func<BindingBase> argBinding)[]
				{
					(X0Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X0, BindingMode.OneWay, source: this)),
					(X1Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X1, BindingMode.OneWay, source: this)),
					(X2Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X2, BindingMode.OneWay, source: this)),
					(X3Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X3, BindingMode.OneWay, source: this)),
					(X4Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X4, BindingMode.OneWay, source: this)),
					(X5Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X5, BindingMode.OneWay, source: this)),
					(X6Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X6, BindingMode.OneWay, source: this)),
					(X7Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X7, BindingMode.OneWay, source: this)),
					(X8Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X8, BindingMode.OneWay, source: this)),
					(X9Property, () => BindingBase.Create(static (LocalizeExtension ctx) => ctx.X9, BindingMode.OneWay, source: this))
				}
				.TakeWhile((arg) => this.IsSet(arg.argProperty))
				.Select((arg) => arg.argBinding())
				.ToList(),
				Mode = BindingMode.OneWay,
				Converter = new PassThruMultiValueConverter()
			});

		return Build();
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
}
