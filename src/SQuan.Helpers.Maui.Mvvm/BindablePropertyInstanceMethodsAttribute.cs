// BindablePropertyInstanceMethodsAttribute.cs

namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// This attribute supplements only the CommunityToolkit.Maui.BindablePropertyAttribute by instructing the
/// source generator to generate static method wrappers for referenced PropertyChangedMethodName,
/// PropertyChangingMethodName or CoerceValueMethodName methods.
/// 
/// This allows for the use of partial methods in conjunction with bindable properties, enabling
/// more flexible and maintainable code.
/// </summary>
/// <remarks>
/// Bindable properties declared directly with .NET MAUI are not supported.
/// </remarks>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class BindablePropertyInstanceMethodsAttribute : Attribute
{
}
