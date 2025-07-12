using System.ComponentModel;
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
		Properties.Data.Welcome = "Welcome to .NET Multi-platform App UI";

		if (Properties.Data is INotifyPropertyChanged dataChanged)
		{
			dataChanged.PropertyChanged += (s, e) =>
			{
				OnPropertyChanged(nameof(Json));
			};
		}

		BindingContext = this;

		InitializeComponent();

		((ObservableIndexer)(vsl.BindingContext)).RemoveNulls = false;

		//CounterBtn.SetBinding(Button.TextProperty, new Binding("[Count]", BindingMode.OneWay, stringFormat: "Clicked {0} times"));
		CounterBtn.SetBinding(Button.TextProperty, BindingBase.Create(static (ObservableIndexer i) => i["Count"], BindingMode.OneWay, stringFormat: "Clicked {0} times"));
	}

	void OnCounterClicked(object sender, EventArgs e)
	{
		Properties.Data.Count++;
		Properties.Data.Hello += "!";
		Properties.Data.Welcome += "!";
	}
}
