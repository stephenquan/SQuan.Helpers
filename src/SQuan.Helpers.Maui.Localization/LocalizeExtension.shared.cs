// LocalizeExtension.shared.cs

using System.Globalization;
using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A markup extension that provides localized strings based on a specified key.
/// </summary>
[ContentProperty(nameof(Key))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class LocalizeExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Gets or sets the localization key for the string to be translated.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial string Key { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {0} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X0 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {1} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X1 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {2} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X2 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {3} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X3 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {4} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X4 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {5} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X5 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {6} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X6 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {7} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X7 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {8} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X8 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {9} to be used for formatting the localized string.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? X9 { get; set; }

	/// <summary>
	/// Gets or sets the current culture to be used for localization.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial CultureInfo? InternalCurrentUICulture { get; set; }

	/// <summary>
	/// Gets or sets the current culture to be used for localization.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial CultureInfo? InternalCurrentCulture { get; set; }

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
