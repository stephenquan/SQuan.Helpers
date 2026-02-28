// CustomContentView.cs

using System.Globalization;
using SQuan.Helpers.Maui.Mvvm;
using BindablePropertyAttribute = CommunityToolkit.Maui.BindablePropertyAttribute;

namespace SQuan.Helpers.Maui.UnitTests;

public partial class CustomContentView : ContentView
{
	[BindableProperty(PropertyChangedMethodName = nameof(OnMagicChanged))]
	[BPExtras]
	public partial int Magic { get; set; } = 42;
	partial void OnMagicChanged(int value) => MagicChangedCount++;

	[BindableProperty(PropertyChangedMethodName = nameof(OnCultureChanged))]
	[BPExtras]
	public partial CultureInfo? Culture { get; set; }
	partial void OnCultureChanged(CultureInfo? value) => CultureChangedCount++;

	[BindableProperty]
	public partial int MagicChangedCount { get; private set; } = 0;

	[BindableProperty]
	public partial int CultureChangedCount { get; private set; } = 0;
}
