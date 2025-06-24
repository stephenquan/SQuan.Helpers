using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Converts a <see cref="bool"/> value to a <see cref="FlowDirection"/> value and vice versa.
/// </summary>
public class RightToLeftToFlowDirectionConverter : IValueConverter
{
	/// <summary>
	/// Converts a boolean value indicating text direction into a corresponding <see cref="FlowDirection"/> value.
	/// </summary>
	/// <param name="value">An object representing a boolean value. If <see langword="true"/>, the result is <see
	/// cref="FlowDirection.RightToLeft"/>; if <see langword="false"/>, the result is <see
	/// cref="FlowDirection.LeftToRight"/>.</param>
	/// <param name="targetType">The type of the binding target property. This parameter is not used in the conversion.</param>
	/// <param name="parameter">An optional parameter to be used in the conversion. This parameter is not used in the conversion.</param>
	/// <param name="culture">The culture to be used in the conversion. This parameter is not used in the conversion.</param>
	/// <returns>A <see cref="FlowDirection"/> value corresponding to the input boolean.  Returns <see
	/// cref="FlowDirection.MatchParent"/> if the input <paramref name="value"/> is not a boolean.</returns>
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is bool isRightToLeft)
		{
			return isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
		}
		return FlowDirection.MatchParent; // Default case
	}

	/// <summary>
	/// Converts a value back to its source type in a data binding scenario.
	/// </summary>
	/// <param name="value">The value produced by the binding target to be converted back.</param>
	/// <param name="targetType">The type to which the value should be converted.</param>
	/// <param name="parameter">An optional parameter to use during the conversion process.</param>
	/// <param name="culture">The culture to use during the conversion process.</param>
	/// <returns>The converted value, or <see langword="null"/> if the conversion is not implemented.</returns>
	/// <exception cref="NotImplementedException">Always thrown, as this method is not implemented.</exception>
	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
