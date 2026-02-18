// ElementToViewConverter.cs

using System.Globalization;
namespace SQuan.Helpers.Maui;

/// <summary>
/// Converts a bound <see cref="Element"/> value to a <see cref="View"/> instance
/// for use in data binding scenarios.
/// </summary>
public class ElementToViewConverter : IValueConverter
{
	/// <summary>
	/// Attempts to cast the specified value to a <see cref="View"/>.
	/// </summary>
	/// <param name="value">The source value produced by the binding source.</param>
	/// <param name="targetType">The type of the binding target property (ignored).</param>
	/// <param name="parameter">An optional parameter to be used in the converter (ignored).</param>
	/// <param name="culture">The culture to use in the converter (ignored).</param>
	/// <returns>
	/// The <see cref="View"/> instance if the cast succeeds; otherwise, <see langword="null"/>.
	/// </returns>
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value as View;

	/// <summary>
	/// Not supported. This converter does not provide a ConvertBack implementation.
	/// </summary>
	/// <param name="value">The value that is produced by the binding target.</param>
	/// <param name="targetType">The type to convert to.</param>
	/// <param name="parameter">An optional parameter to be used in the converter.</param>
	/// <param name="culture">The culture to use in the converter.</param>
	/// <returns>This method always throws a <see cref="NotImplementedException"/>.</returns>
	/// <exception cref="NotImplementedException">Always thrown; ConvertBack is not supported.</exception>
	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
