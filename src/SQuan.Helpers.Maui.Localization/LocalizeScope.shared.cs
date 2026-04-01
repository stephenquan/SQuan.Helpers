// LocalizeScope.shared.cs

using System.Globalization;
using SQuan.Helpers.Internals;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Represents a layout container that applies a specific culture to its child elements for localization purposes.
/// </summary>
public partial class LocalizeScope : StackLayout
{
	/// <summary>
	/// Gets or sets the scope-specific numeric, date, and string formatting culture.
	/// </summary>
	[BindableProperty]
	public partial CultureInfo? Culture { get; set; }

	/// <summary>
	/// Gets or sets the scope-specific culture for retrieving localized resources, such as strings.
	/// </summary>
	[BindableProperty]
	public partial CultureInfo? UICulture { get; set; }

	/// <summary>
	/// Gets or sets the resolver used to provide localized resources.
	/// </summary>
	[BindableProperty]
	public partial LocalizeResolver? Resolver { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="LocalizeScope"/> class.
	/// </summary>
	public LocalizeScope()
	{
		this.SetBinding(
			FlowDirectionProperty,
			new Binding("UICulture.TextInfo.IsRightToLeft", BindingMode.OneWay, new RightToLeftToFlowDirectionConverter(), source: this));
	}
}
