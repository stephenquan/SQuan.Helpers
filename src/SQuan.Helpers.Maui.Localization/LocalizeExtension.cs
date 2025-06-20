using CommunityToolkit.Mvvm.Messaging;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A markup extension that provides localized strings based on a specified key.
/// </summary>
[ContentProperty(nameof(Key))]
[AcceptEmptyServiceProvider]
public partial class LocalizeExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Gets or sets the localization key for the string to be translated.
	/// </summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(TranslatedValue))]
	public partial string Key { get; set; } = string.Empty;

	/// <summary>
	/// Gets the localized string for the specified key.
	/// </summary>
	public string TranslatedValue => LocalizationManager.Current.GetString(Key);

	/// <summary>
	/// Initializes a new instance of the <see cref="LocalizeExtension"/> class.
	/// </summary>
	public LocalizeExtension()
	{
		WeakReferenceMessenger.Default.Register<CultureChangedMessage>(this, (r, m) =>
		{
			OnPropertyChanged(nameof(TranslatedValue));
		});
	}

	/// <summary>
	/// Provides a binding object based on the specified service provider.
	/// </summary>
	/// <param name="serviceProvider">An object that provides services for the binding. This parameter is typically used to resolve services or context
	/// information required to create the binding.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the binding to be used. The specific type and configuration of
	/// the binding depend on the implementation.</returns>
	BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
		=> BindingBase.Create(static (LocalizeExtension ctx) => ctx.TranslatedValue, BindingMode.OneWay, source: this);

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
}
