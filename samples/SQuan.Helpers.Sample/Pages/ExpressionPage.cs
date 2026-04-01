// ExpressionPage.cs

using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using SQuan.Helpers.Maui;

namespace SQuan.Helpers.Sample;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
	"Design",
	"CA1001:Types that own disposable fields should be disposable",
	Justification = "CTS is created in OnNavigatedTo and cancelled/disposed in OnNavigatedFrom as part of the page navigation lifecycle, so implementing IDisposable on this framework-managed ContentPage is unnecessary.")]
public partial class ExpressionPage : ContentPage
{
	public static ILogger? Logger { get; private set; } = IPlatformApplication.Current?.Services.GetService<ILogger<ExpressionPage>>();
	public ExpressionManager EM { get; }
	CancellationTokenSource? cts;

	public ExpressionNode<double> X1 { get; }
	public ExpressionNode<double> X2 { get; }
	public ExpressionNode<double> Sum { get; }
	public ExpressionNode<double> Product { get; }
	public ExpressionNode<double> Hypotenuse { get; }
	public ExpressionNode<string> SerialCode { get; }
	public ExpressionNode<double> BadExpression { get; }
	public ExpressionNode<double> RandomNumber { get; }

	public ExpressionPage()
	{
		EM = new();
		EM.SetInvokeOnUIThread(Dispatcher);
		X1 = EM.SetValue<double>("/survey/x1", 3);
		X2 = EM.SetValue<double>("/survey/x2", 4);
		Sum = EM.SetExpression<double>("/survey/sum", "/survey/x1 + /survey/x2");
		Product = EM.SetExpression<double>("/survey/product", "/survey/x1 * /survey/x2");
		EM.SetDefault<double>("/survey/product", "now()");
		Hypotenuse = EM.SetExpression<double>("/survey/hypotenuse", "sqrt(pow(/survey/x1, 2) + pow(/survey/x2, 2))");
		SerialCode = EM.SetExpression<string>("/survey/serialcode", "concat(/survey/x1, '-ABC-', /survey/x2)");
		BadExpression = EM.SetExpression<double>("/survey/parseerror", "invalid_expression(");
		RandomNumber = EM.SetExpression<double>("/survey/random", "random()");

		Content = new ScrollView
		{
			Content = new VerticalStackLayout
			{
				Spacing = 10,
				Padding = new Thickness(10, 10, 10, 250),
				Children =
				{
					new Label { Text = "Enter X1" },
					new Entry { }.Bind(Entry.TextProperty, X1.BindValue(BindingMode.TwoWay)),
					new Label { Text = "Enter X2" },
					new Entry { }.Bind(Entry.TextProperty, X2.BindValue(BindingMode.TwoWay)),
					new Label { }.Bind(Label.TextProperty,
						Sum.BindTextValue(),
						Sum.BindValueType(),
						Sum.BindValueKind(),
						Sum.BindIsDeterministic(),
						"Sum: {0} (valueType: {1}, valueKind: {2}, isDeterministic: {3})"),
					new Label { }.Bind(Label.TextProperty,
						Product.BindTextValue(),
						Product.BindValueType(),
						Product.BindValueKind(),
						Product.BindIsDeterministic(),
						"Product: {0} (valueType: {1}, valueKind: {2}, isDeterministic: {3})"),
					new Entry { }.Bind(Entry.TextProperty, Product.BindValue(BindingMode.TwoWay)),
					new Button { Text = "Recalculate" }
						.Invoke(b => b.Clicked += (s, e) => EM.Recalculate(Product)),
					new Button { Text= "ResetToDefault"}
						.Invoke(b => b.Clicked += (s, e) => EM.ResetToDefault(Product)),
					new Label { }.Bind(Label.TextProperty,
						Hypotenuse.BindTextValue(),
						Hypotenuse.BindValueType(),
						Hypotenuse.BindValueKind(),
						Hypotenuse.BindIsDeterministic(),
						"Hypotenuse: {0} (valueType: {1}, valueKind: {2}, isDeterministic: {3})"),
					new Label { }.Bind(Label.TextProperty,
						SerialCode.BindValue(),
						SerialCode.BindValueType(),
						SerialCode.BindValueKind(),
						SerialCode.BindIsDeterministic(),
						"Serial Code: {0} (valueType: {1}, valueKind: {2}, isDeterministic: {3})"),
					new Label { }.Bind(Label.TextProperty,
						BadExpression.BindValue(),
						BadExpression.BindValueType(),
						BadExpression.BindValueKind(),
						BadExpression.BindIsDeterministic(),
						"Bad Expression: {0} (valueType: {1}, valueKind: {2}, isDeterministic: {3})"),
					new Label { }.Bind(Label.TextProperty,
						RandomNumber.BindValue(),
						RandomNumber.BindValueType(),
						RandomNumber.BindValueKind(),
						RandomNumber.BindIsDeterministic(),
						"Random Number: {0} (valueType: {1}, valueKind: {2}, isDeterministic: {3})"),
					new Button { Text = "Recalculate Random Number" }
						.Invoke(b => b.Clicked += (s, e) => EM.Recalculate(RandomNumber))
				}
			}
		};
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		try
		{
			cts = new();
			EM.StartWorkLoop(cts.Token);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "An error occurred while starting the calculation loop in OnNavigatedTo.");
		}
	}

	protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);
		try
		{
			await EM.StopCalculationLoopAsync();
			await EM.ClearAsync();
			cts?.Cancel();
			cts?.Dispose();
			cts = null;
			EM.Dispose();
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "An error occurred during cleanup in OnNavigatedFrom.");
		}
	}
}

public static class BindHelpers
{
	public static T Bind<T>(
		this T target,
		BindableProperty targetProperty,
		BindingBase binding) where T : BindableObject
	{
		target.SetBinding(targetProperty, binding);
		return target;
	}

	public static T Bind<T>(
		this T target,
		BindableProperty targetProperty,
		BindingBase binding1,
		BindingBase binding2,
		BindingBase binding3,
		BindingBase binding4,
		string stringFormat) where T : BindableObject
	{
		target.SetBinding(targetProperty, new MultiBinding
		{
			Bindings =
			{
				binding1,
				binding2,
				binding3,
				binding4,
			},
			StringFormat = stringFormat
		});
		return target;
	}
}
