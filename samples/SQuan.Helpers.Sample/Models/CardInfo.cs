using CommunityToolkit.Mvvm.ComponentModel;
namespace SQuan.Helpers.Sample;

public partial class CardInfo : ObservableObject
{
	[ObservableProperty] public partial string CardTitle { get; set; } = "Card Title";
	[ObservableProperty] public partial string CardDescription { get; set; } = "This is a sample card description. It can be used to provide additional information about the card.";
	[ObservableProperty] public partial bool IsSelected { get; set; } = false;
}
