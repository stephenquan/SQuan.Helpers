using System.Collections.ObjectModel;
using System.Dynamic;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class SearchPage : ContentPage
{
	[BindableProperty] public partial string SearchText { get; set; } = "Surveys";
	public ObservableCollection<object> Results { get; } = [];

	public SearchPage()
	{
		BindingContext = this;

		InitializeComponent();
	}

	async void OnSearchClicked(object sender, EventArgs e)
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
			dynamic response = await HttpClientHelper.GetAsync(client, "https://www.arcgis.com/sharing/rest/search", FormDataHelper.EncodeFormData(query));
			System.Diagnostics.Trace.WriteLine($"Found {response.results.Count} results, next start: {response.nextStart}");
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
}
