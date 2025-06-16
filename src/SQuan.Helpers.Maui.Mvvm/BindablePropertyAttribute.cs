namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// Indicates that a property is bindable, allowing it to be used in data binding scenarios.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class BindablePropertyAttribute : Attribute
{
	/// <summary>Gets or sets the default binding mode for the bindable property.</summary>
	public string DefaultBindingMode { get; set; } = "OneWay";
}
