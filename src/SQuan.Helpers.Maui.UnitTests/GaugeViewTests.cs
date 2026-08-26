// GaugeViewTests.cs

namespace SQuan.Helpers.Maui.UnitTests;

public partial class GaugeViewTests : BaseTest
{
	[Theory]
	[InlineData(32, 270, 32)]
	[InlineData(-32, 270, 0)]
	[InlineData(271, 270, 270)]
	public void GaugeView_SetAngle_ClampedToMaximumAngle(double angle, double maximumAngle, double expectedAngle)
	{
		var control = new SQuan.Helpers.Sample.GaugeView()
		{
			MaximumAngle = maximumAngle
		};
		control.Angle = angle;
		Assert.Equal(expectedAngle, control.Angle);
	}
}
