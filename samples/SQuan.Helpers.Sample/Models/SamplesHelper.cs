using System.Collections.ObjectModel;

namespace SQuan.Helpers.Sample;

public partial class SamplesHelper
{
	static SamplesHelper? instance;

	public static SamplesHelper Current
		=> instance ??= new SamplesHelper();

	public ObservableCollection<Sample> Samples { get; } = [];

	public static Sample RegisterSample(string name, string routePage, Type routeType)
	{
		Routing.RegisterRoute(routePage, routeType);
		var sample = new Sample(name, routePage, routeType);
		Current.Samples.Add(sample);
		return sample;
	}
}
