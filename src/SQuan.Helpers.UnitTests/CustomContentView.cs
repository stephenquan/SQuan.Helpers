// CustomContentView.cs

using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.UnitTests;

public partial class CustomContentView : ContentView
{
	[BindableProperty] public partial int Magic { get; set; } = 42;
	public int MagicChangedCount { get; private set; } = 0;
	partial void OnMagicChanged(int oldValue, int newValue)
	{
		MagicChangedCount++;
	}
}
