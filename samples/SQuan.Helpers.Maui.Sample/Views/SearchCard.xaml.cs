using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class SearchCard : ContentView
{
	[BindableProperty] public partial string? ItemId { get; set; }
	[BindableProperty] public partial string? Title { get; set; }
	[BindableProperty] public partial string? Snippet { get; set; }
	[BindableProperty] public partial string? Description { get; set; }
	[BindableProperty] public partial string? ItemType { get; set; }
	[BindableProperty] public partial string? Owner { get; set; }
	[BindableProperty] public partial long? Modified { get; set; }
	[BindableProperty] public partial string? Thumbnail { get; set; }

	public string? ThumbnailUrl
	{
		get
		{
			if (string.IsNullOrEmpty(ItemId) || string.IsNullOrEmpty(Thumbnail))
			{
				return null;
			}

			return $"https://www.arcgis.com/sharing/rest/content/items/{ItemId}/info/{Thumbnail}?w=100";
		}
	}

	public DateTime? ModifiedDate
	{
		get
		{
			if (Modified is long modified)
			{
				return DateTimeOffset.FromUnixTimeMilliseconds(modified).DateTime;
			}

			return null;
		}
	}

	public SearchCard()
	{
		InitializeComponent();

		this.PropertyChanged += (s, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(ItemId):
				case nameof(Thumbnail):
					OnPropertyChanged(nameof(ThumbnailUrl));
					break;

				case nameof(Modified):
					OnPropertyChanged(nameof(ModifiedDate));
					break;
			}
		};
	}
}
