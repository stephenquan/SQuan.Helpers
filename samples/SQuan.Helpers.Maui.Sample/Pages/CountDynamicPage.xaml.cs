using System.Dynamic;
using System.Text.Json;

namespace SQuan.Helpers.Maui.Sample;

public partial class CountDynamicPage : ContentPage
{
	public dynamic Properties { get; } = new BindableDynamic(new ExpandoObject());
	public string Json => JsonSerializer.Serialize(Properties, new JsonSerializerOptions() { WriteIndented = true });

	public CountDynamicPage()
	{
		Properties.Count = 0;
		Properties.Hello = "Hello, World!";
		Properties.Welcome = "Welcome to \n.NET Multi-platform App UI";
		BindingContext = this;
		InitializeComponent();
		CounterBtn.SetBinding(Button.TextProperty, new Binding("[Count]", BindingMode.OneWay, stringFormat: "Clicked {0} times"));
	}

	void OnCounterClicked(object sender, EventArgs e)
	{
		Properties.Count++;
		Properties.Hello += "!";
		Properties.Welcome += "!";
		OnPropertyChanged(nameof(Json));
	}
}
