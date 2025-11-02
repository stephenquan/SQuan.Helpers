// GaugeViewTests.cs

using SQuan.Helpers.Maui.UnitTests.Mocks;

namespace SQuan.Helpers.UnitTests;

public class GuageViewTests
{
	[Theory]
	[InlineData(32, 270, 32)]
	[InlineData(-32, 270, 0)]
	[InlineData(271, 270, 270)]
	public void BalanceView_SetAngle_ClampedToMaximumAngle(double angle, double maximumAngle, double expectedAngle)
	{
		DispatcherProvider.SetCurrent(new MockDispatcherProvider());
		var control = new SQuan.Helpers.Sample.GaugeView()
		{
			MaximumAngle = maximumAngle
		};
		control.Angle = angle;
		Assert.Equal(expectedAngle, control.Angle);
	}
}
