// AspectPage.xaml.cs

using CommunityToolkit.Mvvm.Input;

namespace SQuan.Helpers.Sample;

public partial class AspectPage : ContentPage
{
	public AspectPage()
	{
		BindingContext = this;
		InitializeComponent();
	}

	[RelayCommand]
	void Play(Button btn)
	{
		btn.Text = btn.Text switch
		{
			"X" => "O",
			"O" => " ",
			_ => "X",
		};
	}
}
