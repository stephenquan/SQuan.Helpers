using CommunityToolkit.Mvvm.ComponentModel;

namespace SQuan.Helpers.Maui.Sample;
public partial class SearchResult : ObservableObject
{
	[ObservableProperty] public partial string ItemId { get; set; } = string.Empty;
	[ObservableProperty] public partial string Title { get; set; } = string.Empty;
	[ObservableProperty] public partial string ItemType { get; set; } = string.Empty;
	[ObservableProperty] public partial long Modified { get; set; } = 0;
	[ObservableProperty] public partial string Owner { get; set; } = string.Empty;
	[ObservableProperty] public partial string Description { get; set; } = string.Empty;
	[ObservableProperty] public partial string Snippet { get; set; } = string.Empty;
	[ObservableProperty] public partial string Thumbnail { get; set; } = string.Empty;
	[ObservableProperty] public partial string ThumbnailUrl { get; set; } = string.Empty;
	[ObservableProperty] public partial ImageSource? ThumbnailImage { get; set; }
}
