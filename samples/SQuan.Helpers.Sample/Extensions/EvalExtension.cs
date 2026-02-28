// EvalExtension.cs

using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Markup;
using BindablePropertyAttribute = CommunityToolkit.Maui.BindablePropertyAttribute;

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
	static MultiMathExpressionConverter multiMathExpressionConverter = new MultiMathExpressionConverter();

	/// <summary>Gets or sets the mathematical expression to evaluate.</summary>
	[BindableProperty] public partial string Expression { get; set; } = string.Empty;

	/// <summary>Gets or sets the value of variable X0 in the expression.</summary>
	[BindableProperty] public partial object? X0 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X1 in the expression.</summary>
	[BindableProperty] public partial object? X1 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X2 in the expression.</summary>
	[BindableProperty] public partial object? X2 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X3 in the expression.</summary>
	[BindableProperty] public partial object? X3 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X4 in the expression.</summary>
	[BindableProperty] public partial object? X4 { get; set; } = null;

	/// <summary>Gets or sets the value of variable X5 in the expression.</summary>
	[BindableProperty] public partial object? X5 { get; set; } = null;

	/// <summary>Gets or sets the expected return type of the evaluated expression.</summary>
	[BindableProperty] public partial Type? ReturnType { get; set; }

	/// <summary>Gets the result of evaluating the mathematical expression.</summary>
	[BindableProperty]
	public partial object? ReturnValue { get; set; }


	public EvalExtension()
	{
		this.SetBinding(
			ReturnValueProperty,
			new MultiBinding
			{
				Bindings =
				{
					new Binding("X0", BindingMode.OneWay, source: this),
					new Binding("X1", BindingMode.OneWay, source: this),
					new Binding("X2", BindingMode.OneWay, source: this),
					new Binding("X3", BindingMode.OneWay, source: this),
					new Binding("X4", BindingMode.OneWay, source: this),
					new Binding("X5", BindingMode.OneWay, source: this),
					new Binding("Expression", BindingMode.OneWay, source: this),
				},
				Converter = new FuncMultiConverter<object?, object?[]>(static (values) =>
				{
					if (values is not null
						&& values.Length == 7
						&& values[6] is string expression
						&& !string.IsNullOrEmpty(expression))
					{
						var xValues = values.Take(6).ToArray();
						try
						{
							return multiMathExpressionConverter.Convert(
								xValues,
								typeof(object),
								expression,
								System.Globalization.CultureInfo.InvariantCulture);
						}
						catch (Exception ex)
						{
							return $"Exception: {ex.Message}";
						}
					}
					return null;
				})
			});
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
			this.ReturnType ??= provideValueTarget.TargetProperty switch
			{
				BindableProperty tbp => tbp.ReturnType,
				System.Reflection.PropertyInfo pi => pi.PropertyType,
				_ => null
			};
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
