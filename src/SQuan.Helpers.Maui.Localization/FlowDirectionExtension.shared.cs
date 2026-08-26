// FlowDirectionExtension.shared.cs

using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// A markup extension that provides localized strings based on the current text direction (left-to-right or right-to-left).
/// </summary>
[ContentProperty(nameof(LeftToRight))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class FlowDirectionExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <summary>
	/// Gets or sets the value to be used when the current text direction is left-to-right.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? LeftToRight { get; set; } = FlowDirection.LeftToRight;

	/// <summary>
	/// Gets or sets the value to be used when the current text direction is right-to-left.
	/// </summary>
	[InternalBindableProperty(InstanceMethods = false)]
	public partial object? RightToLeft { get; set; } = FlowDirection.RightToLeft;

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
