// FlowDirectionBinding.cs

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides a binding that determines the flow direction based on the current UI culture's text direction.
/// </summary>
[Obsolete("Use a binding to LocalizationManager.Current.CurrentUICulture.TextInfo.IsRightToLeft with BoolToObjectConverter instead.")]
public static class FlowDirectionBinding
{
	/// <summary>
	/// Creates a binding that determines the flow direction based on the current UI culture's text direction.
	/// </summary>
	/// <returns>This method is obsolete and will always return null.</returns>
	[Obsolete("Use a binding to LocalizationManager.Current.CurrentUICulture.TextInfo.IsRightToLeft with CommunityToolkit.Maui's BoolToObjectConverter instead.")]
	public static BindingBase? Create()
		=> new MultiBinding
		{
			Bindings =
			[
				BindingBase.Create(static (LocalizationManager lm) => lm.CurrentUICulture.TextInfo.IsRightToLeft, BindingMode.OneWay, source: LocalizationManager.Current),
				BindingBase.Create(static (FlowDirection d) => d, BindingMode.OneWay, source: FlowDirection.RightToLeft),
				BindingBase.Create(static (FlowDirection d) => d, BindingMode.OneWay, source: FlowDirection.LeftToRight),
			],
			Mode = BindingMode.OneWay,
			Converter = new MultiBoolToObjectConverter()
		};
}
