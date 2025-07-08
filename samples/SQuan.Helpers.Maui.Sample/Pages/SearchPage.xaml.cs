using System.Collections.ObjectModel;
using System.Dynamic;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;


public partial class SearchPage : ContentPage
{
	public static Type DictionaryType { get; } = typeof(IDictionary<string, object?>);

	[BindableProperty] public partial string SearchText { get; set; } = "Statue of Liberty";
	[BindableProperty, NotifyPropertyChangedFor(nameof(IsNotSearching))] public partial bool IsSearching { get; internal set; } = false;
	public bool IsNotSearching => !IsSearching;
	public ObservableCollection<object> Results { get; } = [];

	public SearchPage()
	{
		BindingContext = this;

		InitializeComponent();
	}

	async void OnSearchClicked(object sender, EventArgs e)
	{
		IsSearching = true;
		try
		{
			dynamic query = new ExpandoObject();
			query.q = SearchText;
			query.f = "json";
			query.start = 1;
			query.num = 50;
			Results.Clear();
			while (true)
			{
				var client = new HttpClient();
				dynamic response = await client.PostApiAsync("https://www.arcgis.com/sharing/rest/search", (ExpandoObject)query);
				System.Diagnostics.Trace.WriteLine($"Search: start:{response.start}, results:{response.results.Count}, nextStart:{response.nextStart}");
				foreach (var result in response.results)
				{
					Results.Add(result);
				}
				if (response.nextStart == -1)
				{
					break;
				}
				query.start = response.nextStart;
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
		finally
		{
			IsSearching = false;
		}
	}
}
