using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Sample;

[ContentProperty(nameof(Expression))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class EvalExtension : BindableObject, IMarkupExtension<BindingBase>
{
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial string Expression { get; set; } = string.Empty;
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X0 { get; set; } = null;
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X1 { get; set; } = null;
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X2 { get; set; } = null;
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X3 { get; set; } = null;
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X4 { get; set; } = null;
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X5 { get; set; } = null;

	public MathParser Parser { get; } = new MathParser();

	public object? ReturnValue
	{
		get
		{
			if (Parser.Expression != this.Expression)
			{
				if (!Parser.Prepare(this.Expression))
				{
					return null;
				}
			}
			return Parser.Evaluate(new object?[] { X0, X1, X2, X3, X4, X5 });
		}
	}

	/// <summary>
	/// Provides a binding object based on the specified service provider.
	/// </summary>
	/// <param name="serviceProvider">An object that provides services for the binding. This parameter is typically used to resolve services or context
	/// information required to create the binding.</param>
	/// <returns>A <see cref="BindingBase"/> instance that represents the binding to be used. The specific type and configuration of
	/// the binding depend on the implementation.</returns>
	BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
	{
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
		{
			if (provideValueTarget.TargetObject is BindableObject targetObject)
			{
				this.SetBinding(BindableObject.BindingContextProperty, static (BindableObject t) => t.BindingContext, BindingMode.OneWay, source: targetObject);
			}
		}
		return BindingBase.Create(static (EvalExtension ctx) => ctx.ReturnValue, BindingMode.OneWay, source: this);
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
}
