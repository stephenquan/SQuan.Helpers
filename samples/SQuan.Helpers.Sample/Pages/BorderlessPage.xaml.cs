// BorderlessPage.xaml.cs

using CommunityToolkit.Maui;

namespace SQuan.Helpers.Sample;

public partial class BorderlessPage : ContentPage
{
	/// <summary>
	/// Gets or sets the border thickness of the input view control.
	/// </summary>
	[BindableProperty]
	public partial double BorderThickness { get; set; } = 1.0;

	public BorderlessPage()
	{
		BindingContext = this;
		InitializeComponent();
	}

	void OnThicknessClicked(object sender, EventArgs e)
	{
		if (sender is Button button && double.TryParse(button.Text, out double thickness))
		{
			BorderThickness = thickness;
		}
	}
}
