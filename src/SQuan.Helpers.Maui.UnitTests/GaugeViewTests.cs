// GaugeViewTests.cs

namespace SQuan.Helpers.Maui.UnitTests;

public partial class GaugeViewTests : BaseTest
{
	[Theory]
	[InlineData(32d, 270d, 32d)]
	[InlineData(-32d, 270d, 0d)]
	[InlineData(271d, 270d, 270d)]
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
