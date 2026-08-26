// BindablePropertyInstanceMethodsAttribute.cs

namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// This attribute supplements the CommunityToolkit.Maui.BindablePropertyAttribute by instructing the
/// source generator to generate static method wrappers for referenced PropertyChangedMethodName,
/// PropertyChangingMethodName or CoerceValueMethodName methods.
/// 
/// This allows for the use of partial methods in conjunction with bindable properties, enabling
/// more flexible and maintainable code.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
[System.Diagnostics.Conditional("SQUAN_HELPERS_MAUI_MVVM_KEEP_ATTRIBUTES")]
public class BindablePropertyInstanceMethodsAttribute : Attribute
{
}
