namespace SQuan.Helpers.Maui.Mvvm;

/// <summary>
/// This attribute is used to support <see cref="ObservablePropertyAttribute"/> and <see cref="BindablePropertyAttribute"/> in generated properties.
/// When used, the generated property setter will also call observable object's OnPropertyChanged(string?)
/// for the properties specified in the attribute data.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
[System.Diagnostics.Conditional("SQUAN_HELPERS_MAUI_MVVM_KEEP_ATTRIBUTES")]
public sealed class NotifyPropertyChangedForAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NotifyPropertyChangedForAttribute"/> class.
	/// </summary>
	/// <param name="propertyName"></param>
	public NotifyPropertyChangedForAttribute(string propertyName)
	{
		PropertyNames = new[] { propertyName };
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NotifyPropertyChangedForAttribute"/> class with the specified property name and other property names.
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="otherPropertyNames"></param>
	public NotifyPropertyChangedForAttribute(string propertyName, params string[] otherPropertyNames)
	{
		PropertyNames = new[] { propertyName }.Concat(otherPropertyNames).ToArray();
	}

	/// <summary>
	/// The names of the properties to be notified.
	/// </summary>
	public string[] PropertyNames { get; }
}
