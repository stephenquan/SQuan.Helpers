// LocalizeExtension.shared.cs

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
	[BindableProperty]
	public partial string Key { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {0} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X0 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {1} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X1 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {2} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X2 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {3} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X3 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {4} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X4 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {5} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X5 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {6} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X6 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {7} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X7 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {8} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X8 { get; set; }

	/// <summary>
	/// Gets or sets the argument value for {9} to be used for formatting the localized string.
	/// </summary>
	[BindableProperty]
	public partial object? X9 { get; set; }

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

		return LocalizeBindingBase.Create(
			BindingBase.Create(static (LocalizeExtension ctx) => ctx.Key, BindingMode.OneWay, source: this),
			new List<BindingBase>
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
				new Binding("X9", BindingMode.OneWay, source: this),
			});
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
}
