using CommunityToolkit.Mvvm.ComponentModel;

namespace SQuan.Helpers.Maui.Sample;

public partial class Sample : ObservableObject
{
	[ObservableProperty] public partial string Name { get; set; }
	[ObservableProperty] public partial string PageRoute { get; set; }
	[ObservableProperty] public partial Type PageType { get; set; }

	public Sample(string name, string pageRoute, Type pageType)
	{
		Name = name;
		PageRoute = pageRoute;
		PageType = pageType;
	}
}
