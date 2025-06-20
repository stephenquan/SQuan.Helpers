# SQuan.Helpers.Maui.Localization

The `SQuan.Helpers.Maui.Localization` provides a localization resource manager, a XAML markup extension and a C# extension method to make localization easier.

## Installation

In order to use the toolkit correctly the UseSQuanHelpersMauiLocalization method must be called with your string resource on the MauiAppBuilder class when bootstrapping an application the MauiProgram.cs file. The following example shows how to perform this.

```c#
var builder = MauiApp.CreateBuilder();
builder
    .UseMauiApp<App>()
    .UseSQuanHelperMauiLocalization<AppStrings>()
```

## Include the XAML namespace

In order to use the Localize markup extension in XAML, the following `xmlns` needs to be added into your page or view:

```xaml
xmlns:i18n="clr-namespace:SQuan.Helpers.Maui.Localization;assembly=SQuan.Helpers.Maui.Localization"
```

## Include the C# namespace

In order to use the Localize extension method in C#, the following `using` statement needs to be added into your file:

```c#
using SQuan.Helpers.Maui.Localization;
```

## Get/Set culture values through the LocalizationManager

The LocalizationManager provides wrappers for InstalledUICulture,CurrentCulture and CurrentUICulture. These wrappers have property change and other event notification to ensure changes in culture are broadcasted to localized strings.

```c#
var de_DE = new CultureInfo("de-DE");
LocalizationManager.Current.CurrentCulture = de_DE; // Set date, time, currency to Germany.
LocalizationManager.Current.CurrentUICulture = de_DE; // Set strings to German.
```

## XAML Localize markup extension example

You can use the Localize markup extension in XAML to assign localize string resources to your text properties, e.g.

```xaml
<Label
    SemanticProperties.HeadingLevel="Level1"
    Style="{StaticResource Headline}"
    Text="{i18n:Localize LBL_HELLO}" />
```

## C# Localize extension method example

You can use the Localize extension method in C# to assign localize string resources to your text properties with parameters, e.g.

```c#
Count++;
CounterBtn.Localize(Button.TextProperty, "BTN_CLICKED_N_TIMES", Count);
SemanticScreenReader.Announce(CounterBtn.Text);
```

## C# LocalizeBinding.Create example

You can use LocalizeBinding.Create method in C# to create multi bindings. You can use this with the CommunityToolkit.Maui.Markup bindable extensions to rapidly create bindings for your controls, e.g.

```c#
using SQuan.Helpers.Maui.Localization;
using CommunityToolkit.Maui.Markup;

CounterBtn.Bind(
    Button.TextProperty,
    binding1: LocalizeBinding.Create("BTN_CLICKED_N_TIMES"),
    binding2: BindingBase.Create(static (LocalizePage m) => m.Count, BindingMode.OneWay, source: this),
    convert: ((string? str, int count) v) => !string.IsNullOrEmpty(v.str) ? string.Format(v.str, v.count) : string.Empty);
```

## Further information

For more information please visit:

 - Documentation: https://github.com/stephenquan/SQuan.Helpers.Maui/wiki/Localization
 - GitHub repository: https://github.com/stephenquan/SQuan.Helpers.Maui
