// App.xaml.cs

namespace SQuan.Helpers.Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = new Window(new AppShell());
		window.MinimumWidth = 300;
		window.MinimumHeight = 300;
		return window;
	}
}
