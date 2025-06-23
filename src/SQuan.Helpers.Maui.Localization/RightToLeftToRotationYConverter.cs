using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Converts a <see langword="bool"/> value indicating right-to-left layout direction  into a corresponding <see
/// cref="FlowDirection"/> value.
/// </summary>
/// <remarks>This converter is typically used in UI scenarios where a boolean value representing  a right-to-left
/// layout (e.g., for languages like Arabic or Hebrew) needs to be  translated into a <see cref="FlowDirection"/> value
/// for layout purposes.</remarks>
public class RightToLeftToRotationYConverter : IValueConverter
{
	/// <summary>
	/// Converts a boolean value indicating right-to-left layout into a rotation angle.
	/// </summary>
	/// <param name="value">The value to convert. Expected to be a <see langword="bool"/> indicating whether the layout is right-to-left.</param>
	/// <param name="targetType">The type of the binding target property. This parameter is not used in the conversion.</param>
	/// <param name="parameter">An optional parameter to use in the conversion. This parameter is not used in the conversion.</param>
	/// <param name="culture">The culture to use in the conversion. This parameter is not used in the conversion.</param>
	/// <returns>A <see cref="double"/> representing the rotation angle. Returns <c>180.0</c> if <paramref name="value"/> is <see
	/// langword="true"/>;  otherwise, returns <c>0.0</c>.</returns>
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value is bool isRightToLeft && isRightToLeft ? 180.0 : 0.0;

	/// <summary>
	/// Converts a value back to its source type in a data binding scenario.
	/// </summary>
	/// <param name="value">The value produced by the binding target to be converted back.</param>
	/// <param name="targetType">The type to which the value should be converted.</param>
	/// <param name="parameter">An optional parameter to use during the conversion process.</param>
	/// <param name="culture">The culture to use during the conversion process.</param>
	/// <returns>The converted value. The default implementation throws a <see cref="NotImplementedException"/>.</returns>
	/// <exception cref="NotImplementedException">Always thrown by the default implementation.</exception>
	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
