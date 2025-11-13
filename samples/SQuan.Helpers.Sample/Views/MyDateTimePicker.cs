using CommunityToolkit.Maui.Markup;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Sample;

public partial class MyDateTimePicker : ContentView
{
	[BindableProperty] public partial DateTime? Value { get; set; } = null;

	int changing = 0;

	public MyDateTimePicker()
	{
		var datePicker = new Microsoft.Maui.Controls.DatePicker()
		{
			VerticalOptions = LayoutOptions.Center,
		};


		var timePicker = new Microsoft.Maui.Controls.TimePicker()
		{
#if WINDOWS
			Margin = new Thickness(0, 0, 0, -13),
#endif
			VerticalOptions = LayoutOptions.Center,
		};

		var clearButton = new Button()
		{
			Text = "X",
			MinimumHeightRequest = 22,
			MinimumWidthRequest = 22,
			Command = new Command(() =>
			{
				Value = (DateTime?)null;
				//OnPropertyChanged(nameof(Value));
				//OnPropertyChanged(nameof(TimePart));
			}),
		}.CenterVertical();


		this.PropertyChanged += (s, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(Value):
					if (changing == 0)
					{
						changing++;
						if (Value is null)
						{
							datePicker.Date = DateTime.Today;
							timePicker.Time = TimeSpan.Zero;
						}
						else
						{
							datePicker.Date = Value.Value - Value.Value.TimeOfDay;
							timePicker.Time = Value.Value.TimeOfDay;
						}
						changing--;
					}
					break;

			}
		};

		datePicker.DateSelected += (s, e) =>
		{
			if (changing == 0)
			{
				changing++;
				if (Value is null)
				{
					Value = datePicker.Date;
					timePicker?.Time = datePicker.Date?.TimeOfDay;
				}
				else if (datePicker.Date is null)
				{
					Value = null;
					timePicker.Time = TimeSpan.Zero;
				}
				else
				{
					if (datePicker.Date is DateTime dateTime)
					{
						if (timePicker.Time is TimeSpan timeSpan)
						{
							Value = dateTime - dateTime.TimeOfDay + timeSpan;
						}
						else
						{
							Value = dateTime - dateTime.TimeOfDay;
						}
					}
				}
				changing--;
			}
		};

		timePicker.TimeSelected += (s, e) =>
		{
			if (changing == 0)
			{
				changing++;
				if (Value is null)
				{
					datePicker.Date = DateTime.Today;
					Value = datePicker.Date + timePicker.Time;
				}
				else if (timePicker.Time is TimeSpan timeSpan)
				{
					Value = Value - Value.Value.TimeOfDay + timeSpan;
				}
				changing--;
			}
		};

		this.Content = new HorizontalStackLayout()
		{
			Spacing = 10,
			Children =
			{
				datePicker,
				timePicker,
				clearButton,
			}
		};

	}
}
