// FlowDirectionExtension.shared.cs

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A markup extension that provides localized strings based on the current text direction (left-to-right or right-to-left).
/// </summary>
[ContentProperty(nameof(LeftToRight))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class FlowDirectionExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Bindable property for the <see cref="LeftToRight"/>.
	/// </summary>
	public static readonly BindableProperty LeftToRightProperty
		= BindableProperty.Create(nameof(LeftToRight), typeof(object), typeof(FlowDirectionExtension), FlowDirection.LeftToRight);

	/// <summary>
	/// Gets or sets the value to be used when the current text direction is left-to-right.
	/// </summary>
	public object? LeftToRight
	{
		get => GetValue(LeftToRightProperty) as object;
		set => SetValue(LeftToRightProperty, value);
	}

	/// <summary>
	/// Bindable property for the <see cref="RightToLeft"/>.
	/// </summary>
	public static readonly BindableProperty RightToLeftProperty
		= BindableProperty.Create(nameof(RightToLeft), typeof(object), typeof(FlowDirectionExtension), FlowDirection.RightToLeft);

	/// <summary>
	/// Gets or sets the value to be used when the current text direction is right-to-left.
	/// </summary>
	public object? RightToLeft
	{
		get => GetValue(RightToLeftProperty) as object;
		set => SetValue(RightToLeftProperty, value);
	}

	/// <summary>
	/// Provides the value of the markup extension based on the current text direction.
	/// </summary>
	/// <param name="serviceProvider">
	/// An object that provides services for the binding.
	/// This parameter is typically used to resolve services or context information required to create the binding.
	/// </param>
	/// <returns>
	/// A <see cref="BindingBase"/> instance that represents the binding to be used.
	/// The specific type and configuration of the binding depend on the implementation.
	/// </returns>
	BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
	{
		if (!IsSet(BindingContextProperty)
			&& serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget
			&& provideValueTarget.TargetObject is BindableObject targetObject)
		{
			this.SetBinding(BindingContextProperty, static (BindableObject t) => t.BindingContext, BindingMode.OneWay, source: targetObject);
		}

		return new MultiBinding
		{
			Bindings =
			[
				BindingBase.Create(static (LocalizationManager lm) => lm.CurrentUICulture.TextInfo.IsRightToLeft, BindingMode.OneWay, source: LocalizationManager.Current),
				BindingBase.Create(static (FlowDirectionExtension ctx) => ctx.RightToLeft, BindingMode.OneWay, source: this),
				BindingBase.Create(static (FlowDirectionExtension ctx) => ctx.LeftToRight, BindingMode.OneWay, source: this),
			],
			Mode = BindingMode.OneWay,
			Converter = new MultiBoolToObjectConverter()
		};
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
}
