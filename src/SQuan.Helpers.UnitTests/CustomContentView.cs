// CustomContentView.cs

using System.Globalization;
using SQuan.Helpers.Maui.Mvvm;

namespace SQuan.Helpers.Maui.UnitTests;

public partial class CustomContentView : ContentView
{
	[BindableProperty] public partial int Magic { get; set; } = 42;
	[BindableProperty] public partial CultureInfo? Culture { get; set; }
	public int MagicChangedCount { get; private set; } = 0;
	public int CultureChangedCount { get; private set; } = 0;
	partial void OnMagicChanged(int value) => MagicChangedCount++;
	partial void OnCultureChanged(CultureInfo? value) => CultureChangedCount++;
}
