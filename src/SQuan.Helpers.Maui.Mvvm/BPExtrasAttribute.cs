// BPExtrasAttribute.cs

namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// An attribute to supplement the CommunityToolkit.Maui.BindablePropertyAttribute 
/// by instructing the BPExtrasGenerator source generator to generate additional static method
/// wrappers for corresponding instance or partial methods.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class BPExtrasAttribute : Attribute
{
}
