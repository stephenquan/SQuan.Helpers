// LocalizeBuilder.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Represents a builder for creating localized bindings.
/// This class allows you to specify a localization key, optional arguments for formatting, and a localization provider function.
/// It provides methods to set bindings for the key and arguments, and it constructs a <see cref="BindingBase"/> that can be applied to a target property.
/// </summary>
public partial class LocalizeBuilder : BindableObject
{
	/// <summary>
	/// Bindable property for <see cref="Key"/>.
	/// </summary>
	public static readonly BindableProperty KeyProperty =
		BindableProperty.Create(nameof(Key), typeof(string), typeof(LocalizeBuilder), default(string));

	/// <summary>
	/// Gets or sets the localization key for the string to be translated.
	/// </summary>
	public string Key
	{
		get => (string)GetValue(KeyProperty);
		set => SetValue(KeyProperty, value);
	}

	/// <summary>
	/// Get the localization provider function that retrieves the localized string based on the specified culture.
	/// </summary>
	public static readonly BindableProperty LocalizationProviderProperty
		= BindableProperty.Create(nameof(LocalizationProvider), typeof(Func<CultureInfo, object>), typeof(LocalizeBuilder), default(Func<CultureInfo, object>));

	/// <summary>
	/// Gets or sets the localization provider function that retrieves the localized string based on the specified culture.
	/// </summary>
	public Func<CultureInfo, object>? LocalizationProvider
	{
		get => (Func<CultureInfo, object>?)GetValue(LocalizationProviderProperty);
		set => SetValue(LocalizationProviderProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="Args"/>.
	/// </summary>
	public static readonly BindableProperty ArgsProperty =
		BindableProperty.Create(nameof(Args), typeof(object?[]), typeof(LocalizeBuilder), default(object?[]));

	/// <summary>
	/// Gets or sets the argument values to be used for formatting the localized string. This property is an array of objects that can be used as placeholders in the localized string.
	/// </summary>
	public object?[]? Args
	{
		get => GetValue(ArgsProperty) as object?[];
		set => SetValue(ArgsProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="Converter"/>.
	/// </summary>
	public static readonly BindableProperty ConverterProperty
		= BindableProperty.Create(nameof(Converter), typeof(object), typeof(LocalizeBuilder), null);

	/// <summary>
	/// Gets or sets the converter to be used for converting the localized string to the target type.
	/// </summary>
	public object? Converter
	{
		get => GetValue(ConverterProperty);
		set => SetValue(ConverterProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="ConverterParameter"/>.
	/// </summary>
	public static readonly BindableProperty ConverterParameterProperty
		= BindableProperty.Create(nameof(ConverterParameter), typeof(object), typeof(LocalizeBuilder), null);

	/// <summary>
	/// Gets or sets the parameter to be passed to the converter when converting the localized string to the target type.
	/// </summary>
	public object? ConverterParameter
	{
		get => GetValue(ConverterParameterProperty);
		set => SetValue(ConverterParameterProperty, value);
	}

	/// <summary>
	/// Bindable property for <see cref="InternalCurrentUICulture"/>.
	/// </summary>
	public static readonly BindableProperty InternalCurrentUICultureProperty =
		BindableProperty.Create(nameof(InternalCurrentUICulture), typeof(CultureInfo), typeof(LocalizeBuilder), default(CultureInfo));

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
		BindableProperty.Create(nameof(InternalCurrentCulture), typeof(CultureInfo), typeof(LocalizeBuilder), default(CultureInfo));

	/// <summary>
	/// Gets or sets the current culture to be used for localization.
	/// </summary>
	public CultureInfo? InternalCurrentCulture
	{
		get => GetValue(InternalCurrentCultureProperty) as CultureInfo;
		set => SetValue(InternalCurrentCultureProperty, value);
	}

	/// <summary>
	/// Sets the key binding for the localization key.
	/// </summary>
	/// <param name="keyBinding">The binding that provides the localization key.</param>
	/// <returns>The current <see cref="LocalizeBuilder"/> instance.</returns>
	public LocalizeBuilder SetKeyBinding(BindingBase keyBinding)
	{
		this.SetBinding(KeyProperty, keyBinding);
		return this;
	}

	/// <summary>
	/// Sets the argument values for formatting the localized string.
	/// </summary>
	/// <param name="args">The argument values to be used for formatting the localized string.</param>
	/// <returns>The current <see cref="LocalizeBuilder"/> instance.</returns>
	public LocalizeBuilder SetArgs(object?[] args)
	{
		this.Args = args;
		return this;
	}

	/// <summary>
	/// Sets the argument bindings for formatting the localized string.
	/// </summary>
	/// <param name="bindings">The bindings to be used for formatting the localized string.</param>
	/// <returns>The current <see cref="LocalizeBuilder"/> instance.</returns>
	public LocalizeBuilder SetArgs(params BindingBase[] bindings)
	{
		this.SetBinding(ArgsProperty, new MultiBinding
		{
			Bindings = bindings,
			Mode = BindingMode.OneWay,
			Converter = new PassThruMultiValueConverter()
		});
		return this;
	}

	/// <summary>
	/// Gets or sets the target object to which the localization binding will be applied.
	/// This is typically set when the binding is created and used to resolve the binding context for localization.
	/// </summary>
	public BindableObject? TargetObject { get; set; }

	/// <summary>
	/// Builds the localization binding based on the specified parameters and returns a <see cref="BindingBase"/> that can be applied to a target property.
	/// </summary>
	/// <returns>A <see cref="BindingBase"/> instance representing the localization binding.</returns>
	public BindingBase Build()
	{
		return new MultiBinding
		{
			Bindings =
			{
				BindingBase.Create(static (LocalizeBuilder ctx) => ctx.Key, BindingMode.OneWay, source: this),
				BindingBase.Create(static (LocalizeBuilder ctx) => ctx.LocalizationProvider, BindingMode.OneWay, source: this),
				new MultiBinding
				{
					Bindings =
					{
						BindingBase.Create(static (LocalizeBuilder ctx) => ctx.InternalCurrentUICulture, BindingMode.OneWay, source: this),
						BindingBase.Create(static (LocalizationManager lm) => lm.CurrentUICulture, BindingMode.OneWay, source: LocalizationManager.Current)
					},
					Mode = BindingMode.OneWay,
					Converter = new SelectCultureMultiValueConverter(),
				},
				new MultiBinding
				{
					Bindings =
					{
						BindingBase.Create(static (LocalizeBuilder ctx) => ctx.InternalCurrentCulture, BindingMode.OneWay, source: this),
						BindingBase.Create(static (LocalizationManager lm) => lm.CurrentCulture, BindingMode.OneWay, source: LocalizationManager.Current)
					},
					Mode = BindingMode.OneWay,
					Converter = new SelectCultureMultiValueConverter(),
				},
				/*
				new MultiBinding
				{
					Bindings =new (BindableProperty argProperty, Func<BindingBase> argBinding)[]
					{
						(X0Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X0, BindingMode.OneWay, source: this)),
						(X1Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X1, BindingMode.OneWay, source: this)),
						(X2Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X2, BindingMode.OneWay, source: this)),
						(X3Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X3, BindingMode.OneWay, source: this)),
						(X4Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X4, BindingMode.OneWay, source: this)),
						(X5Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X5, BindingMode.OneWay, source: this)),
						(X6Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X6, BindingMode.OneWay, source: this)),
						(X7Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X7, BindingMode.OneWay, source: this)),
						(X8Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X8, BindingMode.OneWay, source: this)),
						(X9Property, () => BindingBase.Create(static (LocalizeBuilder ctx) => ctx.X9, BindingMode.OneWay, source: this))
					}
					.TakeWhile((arg) => this.IsSet(arg.argProperty))
					.Select((arg) => arg.argBinding())
					.ToList(),
					Mode = BindingMode.OneWay,
					Converter = new PassThruMultiValueConverter()
				},
				 */
				BindingBase.Create(static (LocalizeBuilder ctx) => ctx.Args, BindingMode.OneWay, source: this),
				BindingBase.Create(static (LocalizeBuilder ctx) => ctx.Converter, BindingMode.OneWay, source: this),
				BindingBase.Create(static (LocalizeBuilder ctx) => ctx.ConverterParameter, BindingMode.OneWay, source: this)
			},
			Mode = BindingMode.OneWay,
			Converter = new LocalizeMultiValueConverter()
		};
	}

	/// <inheritdoc/>
	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (TargetObject is VisualElement targetElement && targetElement.GetLocalizeBehavior() is LocalizeGroupBehavior behavior)
		{
			this.SetBinding(InternalCurrentUICultureProperty, static (LocalizeGroupBehavior b) => b.CurrentUICulture, BindingMode.OneWay, source: behavior);
			this.SetBinding(InternalCurrentCultureProperty, static (LocalizeGroupBehavior b) => b.CurrentCulture, BindingMode.OneWay, source: behavior);
		}
	}
}
