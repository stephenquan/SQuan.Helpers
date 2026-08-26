namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// This attribute is used to support <see cref="ObservablePropertyAttribute"/> and <see cref="BindablePropertyAttribute"/> in generated properties.
/// When used, the generated property setter will also call observable object's OnPropertyChanging(string?)
/// for the properties specified in the attribute data.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
[System.Diagnostics.Conditional("SQUAN_HELPERS_MAUI_MVVM_KEEP_ATTRIBUTES")]
public sealed class NotifyPropertyChangingForAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NotifyPropertyChangingForAttribute"/> class.
	/// </summary>
	/// <param name="propertyName"></param>
	public NotifyPropertyChangingForAttribute(string propertyName)
	{
		PropertyNames = new[] { propertyName };
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NotifyPropertyChangingForAttribute"/> class with the specified property name and other property names.
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="otherPropertyNames"></param>
	public NotifyPropertyChangingForAttribute(string propertyName, params string[] otherPropertyNames)
	{
		PropertyNames = new[] { propertyName }.Concat(otherPropertyNames).ToArray();
	}

	/// <summary>
	/// The names of the properties to be notified.
	/// </summary>
	public string[] PropertyNames { get; }
}
