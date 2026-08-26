// GaugeView.cs

using BindablePropertyAttribute = CommunityToolkit.Maui.BindablePropertyAttribute;
using BindablePropertyInstanceMethodsAttribute = SQuan.Helpers.Maui.Mvvm.BindablePropertyInstanceMethodsAttribute;

namespace SQuan.Helpers.Sample;

/// <summary>
/// A sample gauge view demonstrating the use of bindable properties.
/// </summary>
public partial class GaugeView : ContentView
{
	/// <summary>
	/// Gets or sets the angle of the gauge.
	/// </summary>
	[BindableProperty(CoerceValueMethodName = nameof(CoerceAngle))]
	[BindablePropertyInstanceMethods]
	public partial double Angle { get; set; } = 0;

	/// <summary>
	/// Gets or sets the minimum angle of the gauge.
	/// </summary>
	[BindableProperty]
	public partial double MaximumAngle { get; set; } = 270;

	double CoerceAngle(double value)
	{
		if (value < 0)
		{
			return 0;
		}

		if (value > MaximumAngle)
		{
			return MaximumAngle;
		}

		return value;
	}
}
