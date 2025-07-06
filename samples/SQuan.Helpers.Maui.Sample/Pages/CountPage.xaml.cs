using CommunityToolkit.Maui.Markup;
using SQuan.Helpers.Maui.Mvvm;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

namespace SQuan.Helpers.Maui.Sample;

public partial class CountPage : ContentPage
{
	// Our syntax is similar to CommunityToolkit.Mvvm
	[ObservableProperty] public partial int Count { get; set; } = 0;

	public CountPage()
	{
		BindingContext = this;
		InitializeComponent();
		CounterBtn.Bind(
			Button.TextProperty,
			static (CountPage ctx) => ctx.Count,
			stringFormat: "Clicked {0} times");
	}

	[RelayCommand]
	void IncrementCounter()
	{
		Count++;
		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}
