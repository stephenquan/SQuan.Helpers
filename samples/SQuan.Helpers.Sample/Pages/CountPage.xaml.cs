// CountPage.xaml.cs

using CommunityToolkit.Maui.Markup;
using BindablePropertyAttribute = CommunityToolkit.Maui.BindablePropertyAttribute;
using BindablePropertyInstanceMethodsAttribute = SQuan.Helpers.Maui.Mvvm.BindablePropertyInstanceMethodsAttribute;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

namespace SQuan.Helpers.Sample;

public partial class CountPage : ContentPage
{
	[BindableProperty(PropertyChangedMethodName = nameof(OnCountChanged))]
	[BindablePropertyInstanceMethods]
	public partial int Count { get; set; } = 0;

	partial void OnCountChanged(int oldValue, int newValue)
	{
		System.Diagnostics.Trace.WriteLine($"Count changed from {oldValue} to {newValue}");
	}

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
