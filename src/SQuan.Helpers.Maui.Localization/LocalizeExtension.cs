namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A markup extension that provides localized strings based on a specified key.
/// </summary>
[ContentProperty(nameof(Key))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class LocalizeExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Bindable proeprty for <see cref="Key"/>."/>
	/// </summary>
	public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(string), typeof(LocalizeExtension), string.Empty);

	/// <summary>
	/// Gets or sets the localization key for the string to be translated.
	/// </summary>
	public string Key
	{
		get => (string)GetValue(KeyProperty);
		set => SetValue(KeyProperty, value);
	}

	/// <summary>
	/// Gets or sets the argument value for {0} to be used for formatting the localized string.
	/// </summary>
	public object? X0 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {1} to be used for formatting the localized string.
	/// </summary>
	public object? X1 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {2} to be used for formatting the localized string.
	/// </summary>
	public object? X2 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {3} to be used for formatting the localized string.
	/// </summary>
	public object? X3 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {4} to be used for formatting the localized string.
	/// </summary>
	public object? X4 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {5} to be used for formatting the localized string.
	/// </summary>
	public object? X5 { get; set; }

	/// <summary>
	/// Provides a binding object based on the specified service provider.
	/// </summary>
	/// <param name="serviceProvider">An object that provides services for the binding. This parameter is typically used to resolve services or context
	/// information required to create the binding.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the binding to be used. The specific type and configuration of
	/// the binding depend on the implementation.</returns>
	BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
	{
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
		{
			if (provideValueTarget.TargetObject is BindableObject targetObject)
			{
				this.SetBinding(
					BindableObject.BindingContextProperty,
					static (BindableObject t) => t.BindingContext, BindingMode.OneWay, source: targetObject);
			}
		}

		object?[] args = [X0, X1, X2, X3, X4, X5];

		return LocalizeBinding.Create(
			BindingBase.Create(static (LocalizeExtension ctx) => ctx.Key, BindingMode.OneWay, source: this),
			args.ToArray());
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
}
