// LocalizeGroupBehavior.shared.cs

using System.Globalization;
using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A behavior that allows grouping of localized elements and provides a way to set the current culture and UI culture for localization.
/// </summary>
public partial class LocalizeGroupBehavior : Behavior<VisualElement>
{
	/// <summary>
	/// Gets or sets the current UI culture for localization.
	/// This property allows you to specify the culture to be used for retrieving localized resources within the scope of this behavior.
	/// </summary>
	[InternalBindableProperty(UseStaticCallbacks = true)]
	public partial CultureInfo? CurrentUICulture { get; set; }

	/// <summary>
	/// Gets or sets the current culture for localization.
	/// This property allows you to specify the culture to be used for formatting dates, numbers, and other culture-specific data within the scope of this behavior.
	/// </summary>
	[InternalBindableProperty(UseStaticCallbacks = true)]
	public partial CultureInfo? CurrentCulture { get; set; }

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
					BindingBase.Create(static (LocalizeGroupBehavior b) => b.CurrentUICulture?.TextInfo.IsRightToLeft, BindingMode.OneWay, source: this),
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

