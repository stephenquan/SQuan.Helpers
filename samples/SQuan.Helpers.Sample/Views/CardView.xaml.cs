// CardView.xaml.cs

using CommunityToolkit.Maui.Markup;
using BindablePropertyAttribute = CommunityToolkit.Maui.BindablePropertyAttribute;

namespace SQuan.Helpers.Sample;

public partial class CardView : ContentView
{
	[BindableProperty] public partial string CardTitle { get; set; } = string.Empty;
	[BindableProperty] public partial string CardDescription { get; set; } = string.Empty;
	[BindableProperty] public partial ImageSource? IconImageSource { get; set; } = null;
	[BindableProperty] public partial Color IconBackgroundColor { get; set; } = Colors.Transparent;
	[BindableProperty] public partial Color BorderColor { get; set; } = Colors.Transparent;
	[BindableProperty] public partial Color CardColor { get; set; } = Colors.Transparent;
	[BindableProperty] public partial bool IsSelected { get; set; } = false;

	public CardView()
	{
		InitializeComponent();

		// Implement a ControlTemplate selector
		this.Bind(
			ContentView.ControlTemplateProperty,
			static (CardView ctx) => ctx.IsSelected,
			source: this,
			convert: (bool isSelected) => Resources[isSelected ? "CardViewDefault" : "CardViewCompressed"] as ControlTemplate);
	}
}
