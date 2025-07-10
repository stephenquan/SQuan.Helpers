using System.Dynamic;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class SearchPage : ContentPage
{
	[BindableProperty] public partial string SearchText { get; set; } = "Statue of Liberty";
	[BindableProperty, NotifyPropertyChangedFor(nameof(IsNotSearching))] public partial bool IsSearching { get; internal set; } = false;
	public bool IsNotSearching => !IsSearching;
	public ObservableList<object> Results { get; } = [];
	CancellationTokenSource? cts;

	public SearchPage()
	{
		BindingContext = this;

		InitializeComponent();
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
			while (query.start != -1)
			{
				var client = new HttpClient();
				dynamic response = await client.PostApiAsync("https://www.arcgis.com/sharing/rest/search", (ExpandoObject)query, cts?.Token);
				System.Diagnostics.Trace.WriteLine($"Search: start:{response.start}, results:{response.results.Count}, nextStart:{response.nextStart}");
				Results.AddRange(response.results);
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
