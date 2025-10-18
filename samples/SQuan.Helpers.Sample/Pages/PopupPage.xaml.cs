using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Sample;

public partial class SamplePopup<T> : Popup<T>
{
	[BindableProperty]
	public partial T Secret { get; set; } = (T)Convert.ChangeType(31415, typeof(T));

	public SamplePopup()
	{
		Content = new Button
		{
			Text = "Click me",
			Command = new Command(async () => await this.CloseAsync(Secret)),
		};
	}
}

public partial class PopupPage : ContentPage
{
	public PopupPage()
	{
		InitializeComponent();
	}

	async void OnIntPopup(object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		var pi = new SamplePopup<int>();
		IPopupResult<int> ri = await this.ShowPopupAsync<int>(pi);
		btn.Text = ri.Result.ToString();
	}

	async void OnDblPopup(object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		var pi = new SamplePopup<double>();
		IPopupResult<double> ri = await this.ShowPopupAsync<double>(pi);
		btn.Text = ri.Result.ToString();
	}
}
