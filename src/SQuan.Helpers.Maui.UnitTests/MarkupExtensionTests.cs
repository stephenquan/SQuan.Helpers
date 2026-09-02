// MarkupExtensionTests.cs

using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace SQuan.Helpers.Maui.UnitTests;

public partial class MarkupExtensionTests : BaseTest
{
	static readonly IServiceProvider emptyServiceProvider = new TestServiceProvider();

	[Fact]
	public void ProvideValue_UnsetBindingContext_InheritsTargetBindingContext()
	{
		var bindingContext = new object();
		var target = new ContentView { BindingContext = bindingContext };
		var extension = new TestMarkupExtension();
		var serviceProvider = new TestServiceProvider(new TestProvideValueTarget(target));

		var result = extension.ProvideValue(serviceProvider);

		Assert.Same(extension.Binding, result);
		Assert.Same(bindingContext, extension.BindingContext);
		Assert.Same(serviceProvider, extension.ServiceProvider);
	}

	[Fact]
	public void ProvideValue_ExplicitBindingContext_PreservesBindingContext()
	{
		var bindingContext = new object();
		var target = new ContentView { BindingContext = new object() };
		var extension = new TestMarkupExtension { BindingContext = bindingContext };
		var serviceProvider = new TestServiceProvider(new TestProvideValueTarget(target));

		extension.ProvideValue(serviceProvider);

		Assert.Same(bindingContext, extension.BindingContext);
	}

	[Theory]
	[InlineData(true, "true")]
	[InlineData(false, "false")]
	public void BoolToObjectExtension_ValueChanges_ReturnsConfiguredObject(bool value, string expected)
	{
		var extension = new BoolToObjectExtension
		{
			Value = value,
			TrueObject = "true",
			FalseObject = "false"
		};

		var binding = Assert.IsType<MultiBinding>(extension.ProvideBindingValue(emptyServiceProvider));
		var result = binding.Converter?.Convert([extension.Value, extension.TrueObject, extension.FalseObject], typeof(string), null, CultureInfo.InvariantCulture);

		Assert.Equal(BindingMode.OneWay, binding.Mode);
		Assert.Equal(3, binding.Bindings.Count);
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData(5, 4, CompareConverter.OperatorType.Greater, "true")]
	[InlineData(3, 4, CompareConverter.OperatorType.Greater, "false")]
	[InlineData(4, 4, CompareConverter.OperatorType.Equal, "true")]
	public void CompareExtension_ConfiguredComparison_ReturnsConfiguredObject(double value, double comparingValue, CompareConverter.OperatorType comparisonOperator, string expected)
	{
		var extension = new CompareExtension
		{
			Value = value,
			ComparingValue = comparingValue,
			ComparisonOperator = comparisonOperator,
			TrueObject = "true",
			FalseObject = "false"
		};

		var binding = Assert.IsType<MultiBinding>(extension.ProvideBindingValue(emptyServiceProvider));
		var result = binding.Converter?.Convert(
			[extension.Value, extension.ComparingValue, extension.ComparisonOperator, extension.TrueObject, extension.FalseObject],
			typeof(string),
			null,
			CultureInfo.InvariantCulture);

		Assert.Equal(BindingMode.OneWay, binding.Mode);
		Assert.Equal(5, binding.Bindings.Count);
		Assert.Equal(expected, result);
	}

	[Fact]
	public void RgbaToColorExtension_DefaultValues_ReturnsWhite()
	{
		var extension = new RgbaToColorExtension();

		var binding = Assert.IsType<MultiBinding>(extension.ProvideBindingValue(emptyServiceProvider));
		var result = binding.Converter?.Convert(
			[extension.Red, extension.Green, extension.Blue, extension.Alpha],
			typeof(Color),
			null,
			CultureInfo.InvariantCulture);

		Assert.Equal(BindingMode.OneWay, binding.Mode);
		Assert.Equal(4, binding.Bindings.Count);
		Assert.Equal(Colors.White, result);
		Assert.IsType<RgbaToColorConverter>(binding.Converter);
	}

	sealed partial class TestMarkupExtension : BaseBindableObjectMarkupExtension
	{
		public Binding Binding { get; } = new();

		public IServiceProvider? ServiceProvider { get; private set; }

		public override BindingBase ProvideBindingValue(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
			return Binding;
		}
	}

	sealed class TestProvideValueTarget(BindableObject targetObject) : IProvideValueTarget
	{
		public object TargetObject { get; } = targetObject;

		public object TargetProperty { get; } = BindableObject.BindingContextProperty;
	}

	sealed partial class TestServiceProvider(IProvideValueTarget? provideValueTarget = null) : IServiceProvider
	{
		public object? GetService(Type serviceType)
			=> serviceType == typeof(IProvideValueTarget) ? provideValueTarget : null;
	}
}
