// InputExtrasTests.cs

namespace SQuan.Helpers.Maui.UnitTests;

public partial class InputExtrasTests : BaseTest
{
	[Fact]
	public void GetBorderThickness_UnsetProperty_ReturnsDefaultValue()
	{
		var entry = new Entry();

		var result = InputExtras.GetBorderThickness(entry);

		Assert.Equal(1.0, result);
	}

	[Theory]
	[InlineData(0.0)]
	[InlineData(1.5)]
	[InlineData(10.0)]
	public void SetBorderThickness_ValidValue_ReturnsAssignedValue(double value)
	{
		var entry = new Entry();

		InputExtras.SetBorderThickness(entry, value);

		Assert.Equal(value, InputExtras.GetBorderThickness(entry));
		Assert.Single(entry.Behaviors);
	}

	[Fact]
	public void GetInputMode_UnsetProperty_ReturnsDefaultValue()
	{
		var entry = new Entry();

		var result = InputExtras.GetInputMode(entry);

		Assert.Equal(InputMode.None, result);
	}

	[Theory]
	[InlineData(InputMode.None)]
	[InlineData(InputMode.Integer)]
	[InlineData(InputMode.Decimal)]
	[InlineData(InputMode.Pattern)]
	public void SetInputMode_ValidValue_ReturnsAssignedValue(InputMode value)
	{
		var entry = new Entry();

		InputExtras.SetInputMode(entry, value);

		Assert.Equal(value, InputExtras.GetInputMode(entry));
		Assert.Single(entry.Behaviors);
	}

	[Fact]
	public void GetInputPattern_UnsetProperty_ReturnsEmptyString()
	{
		var entry = new Entry();

		var result = InputExtras.GetInputPattern(entry);

		Assert.Equal(string.Empty, result);
	}

	[Theory]
	[InlineData("")]
	[InlineData("^[a-z]+$")]
	[InlineData(@"^\d{3}-\d{2}-\d{4}$")]
	public void SetInputPattern_ValidValue_ReturnsAssignedValue(string value)
	{
		var entry = new Entry();

		InputExtras.SetInputPattern(entry, value);

		Assert.Equal(value, InputExtras.GetInputPattern(entry));
		Assert.Single(entry.Behaviors);
	}
}
