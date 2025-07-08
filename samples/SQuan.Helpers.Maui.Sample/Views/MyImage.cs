using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class MyImage : Image
{
	[BindableProperty] public partial string? SourceUrl { get; set; }

	bool changing = false;
	long changeTime = 0;

	public MyImage()
	{
		this.PropertyChanged += (s, e) =>
		{
			switch (e.PropertyName)
			{
				case nameof(SourceUrl):
					changeTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
					if (!changing)
					{
						changing = true;
						this.Dispatcher.Dispatch(async () =>
						{
							this.Source = null;
							while (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() < changeTime + 500)
							{
								await Task.Delay(50);
							}
							changing = false;
							if (!string.IsNullOrEmpty(SourceUrl))
							{
								this.Source = ImageSource.FromUri(new Uri(SourceUrl));
							}
							changing = false;
						});
					}
					break;
			}
		};
	}
}
