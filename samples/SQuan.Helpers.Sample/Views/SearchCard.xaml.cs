using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Sample;

public partial class SearchCard : ContentView
{
	[BindableProperty] public partial string? ItemId { get; set; }
	[BindableProperty] public partial string? Title { get; set; }
	[BindableProperty] public partial string? Snippet { get; set; }
	[BindableProperty] public partial string? Description { get; set; }
	[BindableProperty] public partial string? ItemType { get; set; }
	[BindableProperty] public partial string? Owner { get; set; }
	[BindableProperty] public partial long? Modified { get; set; }
	[BindableProperty] public partial ImageSource? ThumbnailImage { get; set; }

	public SearchCard()
	{
		InitializeComponent();
	}
}
