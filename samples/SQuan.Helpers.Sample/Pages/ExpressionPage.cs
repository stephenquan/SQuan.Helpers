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

	public ExpressionNode X1 { get; }
	public ExpressionNode X2 { get; }
	public ExpressionNode Sum { get; }
	public ExpressionNode Product { get; }
	public ExpressionNode Hypotenuse { get; }
	public ExpressionNode SerialCode { get; }
	public ExpressionNode BadExpression { get; }
	public ExpressionNode RandomNumber { get; }

	public ExpressionPage()
	{
		EM = new();
		EM.SetInvokeOnUIThread(Dispatcher);
		X1 = EM.SetValue<double>("/survey/x1", 3);
		X2 = EM.SetValue<double>("/survey/x2", 4);
		Sum = EM.SetExpression<double>("/survey/sum", "/survey/x1 + /survey/x2");
		Product = EM.SetExpression<double>("/survey/product", "/survey/x1 * /survey/x2");
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
					new Entry { }.Bind(Entry.TextProperty, "EM[/survey/x1]", BindingMode.TwoWay, source: this),
					new Label { Text = "Enter X2" },
					new Entry { }.Bind(Entry.TextProperty, "Value", BindingMode.TwoWay, source: X2),
					new Label { }.Bind(Label.TextProperty,
						new Binding("[/survey/sum]", BindingMode.OneWay, source: EM),
						new Binding("ValueType", BindingMode.OneWay, source: Sum),
						new Binding("ValueKind", BindingMode.OneWay, source: Sum),
						"Sum: {0} (valueType: {1}, valueKind: {2})"),
					new Label { }.Bind(Label.TextProperty,
						BindingBase.Create(static (ExpressionNode n) => n.Value, BindingMode.OneWay, source: Product),
						BindingBase.Create(static (ExpressionNode n) => n.ValueType, BindingMode.OneWay, source: Product),
						BindingBase.Create(static (ExpressionNode n) => n.ValueKind, BindingMode.OneWay, source: Product),
						"Product: {0} (valueType: {1}, valueKind: {2})"),
					new Label { }.Bind(Label.TextProperty,
						BindingBase.Create(static (ExpressionNode n) => n.Value, BindingMode.OneWay, source: Hypotenuse),
						BindingBase.Create(static (ExpressionNode n) => n.ValueType, BindingMode.OneWay, source: Hypotenuse),
						BindingBase.Create(static (ExpressionNode n) => n.ValueKind, BindingMode.OneWay, source: Hypotenuse),
						"Hypotenuse: {0} (valueType: {1}, valueKind: {2})"),
					new Label { }.Bind(Label.TextProperty,
						BindingBase.Create(static (ExpressionNode n) => n.Value, BindingMode.OneWay, source: SerialCode),
						BindingBase.Create(static (ExpressionNode n) => n.ValueType, BindingMode.OneWay, source: SerialCode),
						BindingBase.Create(static (ExpressionNode n) => n.ValueKind, BindingMode.OneWay, source: SerialCode),
						"Serial Code: {0} (valueType: {1}, valueKind: {2})"),
					new Label { }.Bind(Label.TextProperty,
						BindingBase.Create(static (ExpressionNode n) => n.Value, BindingMode.OneWay, source: BadExpression),
						BindingBase.Create(static (ExpressionNode n) => n.ValueType, BindingMode.OneWay, source: BadExpression),
						BindingBase.Create(static (ExpressionNode n) => n.ValueKind, BindingMode.OneWay, source: BadExpression),
						"Bad Expression: {0} (valueType: {1}, valueKind: {2})"),
					new Label { }.Bind(Label.TextProperty,
						BindingBase.Create(static (ExpressionNode n) => n.Value, BindingMode.OneWay, source: RandomNumber),
						BindingBase.Create(static (ExpressionNode n) => n.ValueType, BindingMode.OneWay, source: RandomNumber),
						BindingBase.Create(static (ExpressionNode n) => n.ValueKind, BindingMode.OneWay, source: RandomNumber),
						"Random Number: {0} (valueType: {1}, valueKind: {2})"),
					new Button { Text = "Recalculate Random Number" }
						.Invoke(b => b.Clicked += (s, e) => EM.RecalculateByNodeRef("/survey/random"))
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
			EM.StartCalculationLoop(cts.Token);
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
		BindingBase binding1,
		BindingBase binding2,
		BindingBase binding3,
		string stringFormat) where T : BindableObject
	{
		target.SetBinding(targetProperty, new MultiBinding
		{
			Bindings =
			{
				binding1,
				binding2,
				binding3
			},
			StringFormat = stringFormat
		});
		return target;
	}
}
