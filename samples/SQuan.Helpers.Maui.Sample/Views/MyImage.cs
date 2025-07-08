using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.Sample;

public partial class MyImage : Image
{
	[BindableProperty] public partial string? SourceUrl { get; set; }

	bool changing = false; // indicates if the image source is currently being changed
	long changeTime = 0; // milliseconds since epoch
	long changeInterval = 500; // milliseconds

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
							while (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() < changeTime + changeInterval)
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
