// LocalizeGroupBehavior.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A behavior that allows grouping of localized elements and provides a way to set the current culture and UI culture for localization.
/// </summary>
public partial class LocalizeGroupBehavior : Behavior<VisualElement>
{
	/// <summary>
	/// Bindable property for the <see cref="CurrentUICulture"/>.
	/// </summary>
	public static readonly BindableProperty CurrentUICultureProperty =
		BindableProperty.Create(nameof(CurrentUICulture), typeof(CultureInfo), typeof(LocalizeGroupBehavior), null, BindingMode.OneWay);

	/// <summary>
	/// Gets or sets the current UI culture for localization.
	/// This property allows you to specify the culture to be used for retrieving localized resources within the scope of this behavior.
	/// </summary>
	public CultureInfo? CurrentUICulture
	{
		get => GetValue(CurrentUICultureProperty) as CultureInfo;
		set => SetValue(CurrentUICultureProperty, value);
	}

	/// <summary>
	/// Bindable property for the <see cref="CurrentCulture"/>.
	/// </summary>
	public static readonly BindableProperty CurrentCultureProperty =
		BindableProperty.Create(nameof(CurrentCulture), typeof(CultureInfo), typeof(LocalizeGroupBehavior), null, BindingMode.OneWay);
	/// <summary>
	/// Gets or sets the current culture for localization.
	/// This property allows you to specify the culture to be used for formatting dates, numbers, and other culture-specific data within the scope of this behavior.
	/// </summary>
	public CultureInfo? CurrentCulture
	{
		get => GetValue(CurrentCultureProperty) as CultureInfo;
		set => SetValue(CurrentCultureProperty, value);
	}

	/// <inheritdoc />
	protected override void OnAttachedTo(VisualElement bindable)
	{
		base.OnAttachedTo(bindable);
		bindable.SetBinding(
			VisualElement.FlowDirectionProperty,
			new MultiBinding
			{
				Bindings =
				{
					new MultiBinding
					{
						Bindings =
						{
							BindingBase.Create(static (LocalizeGroupBehavior b) => b.CurrentUICulture?.TextInfo.IsRightToLeft, BindingMode.OneWay, source: this),
							BindingBase.Create(static (LocalizationManager lm) => lm.CurrentUICulture.TextInfo.IsRightToLeft, BindingMode.OneWay, source: LocalizationManager.Current)
						},
						Mode = BindingMode.OneWay,
						Converter = new SelectCultureMultiValueConverter()
					},
					BindingBase.Create(static (FlowDirection d) => d, BindingMode.OneWay, source: FlowDirection.RightToLeft),
					BindingBase.Create(static (FlowDirection d) => d, BindingMode.OneWay, source: FlowDirection.LeftToRight)
				},
				Mode = BindingMode.OneWay,
				Converter = new MultiBoolToObjectConverter()
			});
	}

	/// <inheritdoc />
	protected override void OnDetachingFrom(VisualElement bindable)
	{
		base.OnDetachingFrom(bindable);
		bindable.RemoveBinding(VisualElement.FlowDirectionProperty);
	}
}

