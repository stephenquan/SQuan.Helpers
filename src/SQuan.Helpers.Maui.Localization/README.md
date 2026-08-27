# SQuan.Helpers.Maui.Localization

This library provides a localization manager, a XAML markup extension, and Fluent C# extension methods to make it easier to build .NET MAUI applications that react to localization changes at runtime.

## Configure the LocalizationManager.Current.LocalizationProvider

LocalizationManger will need your to set the LocalizationProvider to perform string lookups. You can do this by using your ResourceManager's GetString function, for example:

```c#
LocalizationManager.Current.LocalizationProvider = AppStrings.ResourceManager.GetString;
```

## Include the XAML namespace

In order to use the Localize markup extension in XAML, the following `xmlns` needs to be added into your page or view:

```xaml
xmlns:i18n="clr-namespace:SQuan.Helpers.Maui.Localization;assembly=SQuan.Helpers.Maui.Localization"
```

## Include the C# namespace

In order to use the Localize extension method in C#, the following using statement needs to be added into your file:

```c#
using SQuan.Helpers.Maui.Localization;
```

## Get/Set culture values through the LocalizationManager

The LocalizationManager provides wrappers for CurrentCulture and CurrentUICulture. These wrappers have property change and other event notification to ensure changes in culture are broadcasted to localized strings.

```c#
var de_DE = new CultureInfo("de-DE");
LocalizationManager.Current.CurrentUICulture = de_DE; // Set localized strings to German.
LocalizationManager.Current.CurrentCulture = de_DE; // Set date, time, currency to Germany.
```

## XAML Localize markup extension example

You can use the Localize markup extension in XAML to assign localize string resources to your text properties, e.g.

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:i18n="clr-namespace:SQuan.Helpers.Maui.Localization;assembly=SQuan.Helpers.Maui.Localization">
    <ScrollView>
        <VerticalStackLayout>
           <Label Text="{i18n:Localize LABEL_HELLO_WORLD}" />
           <Label Text="{i18n:LocalizeLABEL_WELCOME}" />
           <Button x:Name="CounterBtn" Text="{i18n:Localize BUTTON_CLICK_ME}" />
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

## Using the Localize Fluent C# extension method

Use `Localize` to create a culture-aware binding to your localized string resource:

```c#
// Localize using the string key.
CounterBtn.Localize(
    Button.TextProperty,
    "BUTTON_CLICKED_N_TIMES",
    Count);

// Localize using a binding to a string key (useful for collections).
CounterBtn.Localize(
    Button.TextProperty,
    new Binding(".", source: "BUTTON_CLICKED_N_TIMES"),
    Count);

// Localize using a localization provider function.
CounterBtn.Localize(
    Button.TextProperty,
    _ => AppStrings.BUTTON_CLICKED_N_TIMES,
    Count);
```

## Further information

For more information please visit:

 - Documentation: https://github.com/stephenquan/SQuan.Helpers/wiki/Localization
 - GitHub repository: https://github.com/stephenquan/SQuan.Helpers
