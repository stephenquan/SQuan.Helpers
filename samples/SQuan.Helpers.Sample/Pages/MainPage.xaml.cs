using CommunityToolkit.Mvvm.Input;
using SQuan.Helpers.Maui.Localization;

namespace SQuan.Helpers.Sample;

public partial class MainPage : ContentPage
{
	public LocalizationManager LM { get; } = LocalizationManager.Current;

	public MainPage()
	{
		BindingContext = this;
		InitializeComponent();
	}

	[RelayCommand]
	async Task Navigate(string route)
	{
		await Shell.Current.GoToAsync(route);
	}
}
