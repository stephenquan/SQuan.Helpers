using System.Collections.ObjectModel;

namespace SQuan.Helpers.Maui.Sample;

public partial class SamplesHelper
{
	static SamplesHelper? instance;

	public static SamplesHelper Current
	{
		get
		{
			if (instance is null)
			{
				instance = new SamplesHelper();
			}
			return instance;
		}
	}

	public ObservableCollection<Sample> Samples { get; } = new();

	public static Sample RegisterSample(string name, string routePage, Type routeType)
	{
		Routing.RegisterRoute(routePage, routeType);

		var sample = new Sample(name, routePage, routeType);
		Current.Samples.Add(sample);
		return sample;
	}
}
