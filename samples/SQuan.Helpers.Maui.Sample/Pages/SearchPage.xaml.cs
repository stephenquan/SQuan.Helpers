using System.Dynamic;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class SearchPage : ContentPage
{
	[BindableProperty] public partial string SearchText { get; set; } = "Statue of Liberty";
	[BindableProperty, NotifyPropertyChangedFor(nameof(IsNotSearching))] public partial bool IsSearching { get; internal set; } = false;
	public bool IsNotSearching => !IsSearching;
	public ObservableList<SearchResult> Results { get; } = [];
	CancellationTokenSource? cts;

	public SearchPage()
	{
		BindingContext = this;

		InitializeComponent();

		this.Disappearing += (s, e) =>
		{
			if (cts is not null)
			{
				if (!cts.IsCancellationRequested)
				{
					cts.Cancel();
				}
			}
		};
	}

	async void OnSearchClicked(object sender, EventArgs e)
	{
		IsSearching = true;
		cts = new CancellationTokenSource();
		try
		{
			dynamic query = new ExpandoObject();
			query.q = SearchText;
			query.f = "json";
			query.start = 1;
			query.num = 50;
			Results.Clear();
			Results.OnCollectionReset();
			Results.OnCountChanged();
			while (query.start != -1)
			{
				var client = new HttpClient();
				dynamic response = await client.PostApiAsync("https://www.arcgis.com/sharing/rest/search", (ExpandoObject)query, cts?.Token);
				System.Diagnostics.Trace.WriteLine($"Search: start:{response.start}, results:{response.results.Count}, nextStart:{response.nextStart}");
				int startIndex = Results.Count;
				List<SearchResult> changedItems = [];
				foreach (var item in response.results)
				{
					dynamic itemIndexer = new ObservableIndexer(item);
					var searchResult = new SearchResult
					{
						ItemId = itemIndexer.id ?? string.Empty,
						ItemType = itemIndexer.type ?? string.Empty,
						Modified = itemIndexer.modified ?? 0,
						Title = itemIndexer.title ?? string.Empty,
						Description = itemIndexer.description ?? string.Empty,
						Snippet = itemIndexer.snippet ?? string.Empty,
						Owner = itemIndexer.owner ?? string.Empty,
						Thumbnail = itemIndexer.thumbnail ?? string.Empty,
					};

					if (searchResult.ItemId is string itemId
						&& !string.IsNullOrEmpty(itemId)
						&& searchResult.Thumbnail is string thumbnail
						&& !string.IsNullOrEmpty(thumbnail))
					{
						searchResult.ThumbnailUrl = $"https://www.arcgis.com/sharing/rest/content/items/{itemId}/info/{thumbnail}?w=100";
						new Thread(async () =>
						{
							Thread.CurrentThread.Priority = ThreadPriority.Lowest;
							try
							{
								var httpClient = new HttpClient();
								byte[] bytes = await httpClient.GetByteArrayAsync(searchResult.ThumbnailUrl);
								if (bytes is not null && bytes.Length > 0)
								{
									this.Dispatcher.Dispatch(() =>
									{
										searchResult.ThumbnailImage = ImageSource.FromStream(() => new MemoryStream(bytes));
									});
								}
							}
							catch (Exception ex)
							{
								System.Diagnostics.Trace.WriteLine($"Exception: {ex.Message}");
							}
						}).Start();
					}
					if (!string.IsNullOrEmpty(searchResult.Thumbnail))
					{
						changedItems.Add(searchResult);
						Results.Add(searchResult);
					}
				}
				if (changedItems.Count > 0)
				{
					Results.OnCollectionAdd(changedItems, startIndex);
					Results.OnCountChanged();
				}
				query.start = response.nextStart;
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
		finally
		{
			IsSearching = false;
			cts.Dispose();
			cts = null;
		}
	}

	void OnCancelClicked(object sender, EventArgs e)
	{
		if (cts is not null)
		{
			if (!cts.IsCancellationRequested)
			{
				cts.Cancel();
			}
		}
	}
}
