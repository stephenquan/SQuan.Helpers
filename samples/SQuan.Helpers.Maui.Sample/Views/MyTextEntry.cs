using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;
public partial class MyTextEntry : Entry
{
	[BindableProperty] public partial string? Value { get; set; }

	int changing = 0;

	public MyTextEntry()
	{
		PropertyChanged += (sender, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(Text):
					if (changing == 0)
					{
						changing++;
						Value = !string.IsNullOrEmpty(Text) ? Text : null;
						changing--;
					}
					break;
				case nameof(Value):
					if (changing == 0)
					{
						changing++;
						Text = Value;
						changing--;
					}
					break;
			}
		};
	}
}

