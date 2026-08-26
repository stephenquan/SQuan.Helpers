// BindablePropertyAttribute.cs

namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// Indicates that a property is bindable, allowing it to be used in data binding scenarios.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
[System.Diagnostics.Conditional("SQUAN_HELPERS_MAUI_MVVM_KEEP_ATTRIBUTES")]
public class BindablePropertyAttribute : Attribute
{
	/// <summary>
	/// Gets or sets the default binding mode for the bindable property.
	/// </summary>
	public string DefaultBindingMode { get; set; } = "OneWay";

	/// <summary>
	/// Gets or sets the method name that will be used for CoerceValue logic.
	/// </summary>
	public string CoerceValue { get; set; } = string.Empty;
}
