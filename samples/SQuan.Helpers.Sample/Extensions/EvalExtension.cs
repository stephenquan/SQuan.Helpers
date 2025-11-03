// EvalExtension.cs

using CommunityToolkit.Maui.Converters;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Sample;

/// <summary>
/// A markup extension that evaluates a mathematical expression and provides the result as a binding.
/// </summary>
/// <remarks>The <see cref="EvalExtension"/> class allows you to define a mathematical expression as a string
/// and bind its result to a property.
/// The expression can reference up to six variables (X0 through X5), which can be dynamically updated.
/// This extension is useful for scenarios where calculated values need to be bound in XAML.</remarks>
[ContentProperty(nameof(Expression))]
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class EvalExtension : BindableObject, IMarkupExtension<BindingBase>
{
	readonly MultiMathExpressionConverter multiMathExpressionConverter = new MultiMathExpressionConverter();

	/// <summary>Gets or sets the mathematical expression to evaluate.</summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial string Expression { get; set; } = string.Empty;

	/// <summary>Gets or sets the value of variable X0 in the expression.</summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X0 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X1 in the expression.</summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X1 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X2 in the expression.</summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X2 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X3 in the expression.</summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X3 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X4 in the expression.</summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X4 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X5 in the expression.</summary>
	[BindableProperty, NotifyPropertyChangedFor(nameof(ReturnValue))] public partial object? X5 { get; set; } = null;

	/// <summary>Gets the result of evaluating the mathematical expression.</summary>
	public object? ReturnValue
	{
		get
		{
			if (string.IsNullOrEmpty(this.Expression))
			{
				return null;
			}

			try
			{
				return multiMathExpressionConverter.Convert(
					new object?[] { X0, X1, X2, X3, X4, X5 },
					typeof(object),
					this.Expression,
					System.Globalization.CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				return $"Exception: {ex.Message}";
			}
		}
	}

	/// <summary>
	/// Provides the binding for the evaluated expression.
	/// Sets the extension's BindingContext to the target object's BindingContext,
	/// then returns a one-way binding to the <see cref="ReturnValue"/> property.
	/// </summary>
	/// <param name="serviceProvider">Service provider for markup extension context.</param>
	/// <returns>A <see cref="BindingBase"/> that binds to the evaluated result.</returns>
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

	/// <summary>
	/// Provides the binding for the evaluated expression for non-generic markup extension usage.
	/// This method delegates to the generic <see cref="IMarkupExtension{T}.ProvideValue(IServiceProvider)"/> implementation.
	/// </summary>
	/// <param name="serviceProvider">Service provider for markup extension context.</param>
	/// <returns>
	/// A <see cref="BindingBase"/> that binds to the evaluated result of the mathematical expression.
	/// </returns>
	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
}
