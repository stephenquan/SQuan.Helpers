using System.Dynamic;
using System.Text.Json;

namespace SQuan.Helpers.Maui.Sample;

public partial class CountDynamicPage : ContentPage
{
	public dynamic Properties { get; }
	public string Json => JsonSerializer.Serialize(Properties, new JsonSerializerOptions() { WriteIndented = true });

	public CountDynamicPage()
	{
		Properties = new ExpandoObject();
		Properties.Data = new ExpandoObject();
		Properties.Data.Count = 0;
		Properties.Data.Hello = "Hello, World!";
		Properties.Data.Welcome = "Welcome to \n.NET Multi-platform App UI";

		BindingContext = this;
		InitializeComponent();
		//CounterBtn.SetBinding(Button.TextProperty, new Binding("[Count]", BindingMode.OneWay, stringFormat: "Clicked {0} times"));
		CounterBtn.SetBinding(Button.TextProperty, BindingBase.Create(static (ObservableIndexer i) => i["Count"], BindingMode.OneWay, stringFormat: "Clicked {0} times"));
	}

	void OnCounterClicked(object sender, EventArgs e)
	{
		Properties.Data.Count++;
		Properties.Data.Hello += "!";
		Properties.Data.Welcome += "!";
		OnPropertyChanged(nameof(Json));
	}
}
