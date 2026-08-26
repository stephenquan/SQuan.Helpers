// BPExtrasAttribute.cs

namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// An attribute to supplement the CommunityToolkit.Maui.BindablePropertyAttribute 
/// by instructing the BPExtrasGenerator source generator to generate additional static method
/// wrappers for corresponding instance or partial methods.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
[System.Diagnostics.Conditional("SQUAN_HELPERS_MAUI_MVVM_KEEP_ATTRIBUTES")]
public class BPExtrasAttribute : Attribute
{
}
