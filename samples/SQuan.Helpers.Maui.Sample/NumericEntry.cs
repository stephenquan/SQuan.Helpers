using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class NumericEntry : Entry
{
	[BindableProperty] public partial double? Value { get; set; } = null;

	int Lock { get; set; } = 0;

	public NumericEntry()
	{
		PropertyChanged += (sender, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(Text):
					if (Lock == 0)
					{
						Lock++;
						Value = (!string.IsNullOrEmpty(Text) && double.TryParse(Text, out double parsedValue)) ? parsedValue : null;
						Lock--;
					}
					break;
				case nameof(Value):
					if (Lock == 0)
					{
						Lock++;
						Text = (Value is null) ? string.Empty : Value.ToString();
						Lock--;
					}
					break;
			}
		};
	}
}
