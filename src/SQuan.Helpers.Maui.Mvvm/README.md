# SQuan.Helpers.Maui.Mvvm

The `SQuan.Helpers.Maui.Mvvm` package is designed to complement the [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) library by providing .NET MAUI-specific source generators. It helps simplify the creation of observable and bindable properties on .NET MAUI bindable objects.

## Installation

To use `[ObservableProperty]` or `[BindableProperty]` on partial properties, the C# language version must be set to `preview`.

This is necessary because the generated code for `[ObservableProperty]` and `[BindableProperty]` on partial properties relies on preview language features.

Be sure to add `<LangVersion>preview</LangVersion>` (or higher) to your `.csproj` file.

## CardView Example

You can use [BindableProperty] to reduce the code needed to extend `ContentView` to create a custom control. The following example shows the `CardTitle` bindable property in the code-behind file to the `CardView` class:

```c#
using CommunityToolkit.Maui.Markup;
using SQuan.Helpers.Maui.Mvvm;

public partial class CardView : ContentView
{
    [BindableProperty] public partial string CardTitle { get; set; } = string.Empty;
    [BindableProperty] public partial string CardDescription { get; set; } = string.Empty;
    // ...

    public CardView()
    {
        InitializeComponent();
    }
}
```

## Count Example

You can use `[ObservableProperty]` to reduce the code needed to add properties to a `ContentPage`. The following example turns Count into an observable property in the code-behind file of the `MainPage` class:

```c#
using CommunityToolkit.Maui.Markup;
using SQuan.Helpers.Maui.Mvvm;
using RelayCommandAttribute = CommunityToolkit.Mvvm.Input.RelayCommandAttribute;

public partial class MainPage : ContentPage
{
    [ObservableProperty] public partial int Count { get; set; } = 0;

    public MainPage()
    {
        BindingContext = this;
        InitializeComponent();
        CounterBtn.Bind(
            Button.TextProperty,
            static (MainPage ctx) => ctx.Count,
            stringFormat: "Clicked {0} times");
    }

    [RelayCommand]
    void IncrementCounter()
    {
        Count++;
        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
```

## Further information

For more information please visit:

 - Documentation: https://github.com/stephenquan/SQuan.Helpers/wiki/MVVM
 - GitHub repository: https://github.com/stephenquan/SQuan.Helpers
