using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Sample;

public partial class MyNumericEntry : Entry
{
	[BindableProperty] public partial double? Value { get; set; } = null;

	int changing = 0;

	public MyNumericEntry()
	{
		PropertyChanged += (sender, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(Text):
					if (changing == 0)
					{
						changing++;
						Value = (!string.IsNullOrEmpty(Text) && double.TryParse(Text, out double parsedValue)) ? parsedValue : null;
						changing--;
					}
					break;
				case nameof(Value):
					if (changing == 0)
					{
						changing++;
						Text = (Value is null) ? string.Empty : Value.ToString();
						changing--;
					}
					break;
			}
		};
	}
}
