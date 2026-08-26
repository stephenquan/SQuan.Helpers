namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// Indicates that a field or property is observable, allowing changes to its value to be tracked and responded to.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("SQUAN_HELPERS_MAUI_MVVM_KEEP_ATTRIBUTES")]
public class ObservablePropertyAttribute : Attribute
{
}
