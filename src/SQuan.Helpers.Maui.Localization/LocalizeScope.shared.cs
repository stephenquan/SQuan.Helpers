// LocalizeScope.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Represents a layout container that applies a specific culture to its child elements for localization purposes.
/// </summary>
[Obsolete("This class is experimental and may be removed or changed in future versions.")]
public partial class LocalizeScope : StackLayout
{
	/// <summary>
	/// Gets or sets the scope-specific numeric, date, and string formatting culture.
	/// </summary>
	public CultureInfo? Culture { get; set; }

	/// <summary>
	/// Gets or sets the scope-specific culture for retrieving localized resources, such as strings.
	/// </summary>
	public CultureInfo? UICulture { get; set; }

	/// <summary>
	/// Gets or sets the resolver used to provide localized resources.
	/// </summary>
	public LocalizeResolver? Resolver { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="LocalizeScope"/> class.
	/// </summary>
	public LocalizeScope()
	{
	}
}
