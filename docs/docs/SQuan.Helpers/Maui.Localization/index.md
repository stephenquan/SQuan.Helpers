# SQuan.Helpers.Maui.Localization

## Overview

SQuan.Helpers.Maui.Localization is a lightweight localization framework for .NET MAUI that helps applications become culture-aware and respond dynamically to language changes while the application is running.

At the heart of the library is the [LocalizationManager](LocalizationManager/index.md), which provides a simple, consistent approach for managing translations and notifying the user interface when the active culture changes. This enables applications to update translated text without requiring a restart, creating a seamless multilingual experience for users.

The library promotes a straightforward but extensible pattern that .NET MAUI developers can adopt quickly, reducing the complexity typically associated with localization while remaining flexible enough to support more advanced scenarios and custom translation providers.

## Add NuGet package

Use the NuGet Package Manager in Visual Studio to install the [SQuan.Helpers.Maui.Localization](https://www.nuget.org/packages/SQuan.Helpers.Maui.Localization) package:

1. Select Project > Manage NuGet Packages
2. On the NuGet Package Manager page, next to Package source, select nuget.org
3. Go to the Browse tab and search for [SQuan.Helpers.Maui.Localization](https://www.nuget.org/packages/SQuan.Helpers.Maui.Localization). In the list, select [SQuan.Helpers.Maui.Localization](https://www.nuget.org/packages/SQuan.Helpers.Maui.Localization), and then select Install.

## C# Namespace

```c#
using SQuan.Helpers.Maui.Localization;
```

## XAML Namespace

```xml
xmlns:i18n="clr-namespace:SQuan.Helpers.Maui.Localization;assembly=SQuan.Helpers.Maui.Localization"
```

## Getting started

For your .NET MAUI project create the neutral resource file in the format `<FullTypeName>.resx` (e.g. AppStrings.resx).

| Resource Name           | Resource Value                        |
| ----------------------- | ------------------------------------- |
| TITLE_HOME              | Home                                  |
| LABEL_HELLO_WORLD       | Hello, World!                         |
| LABEL_WELCOME           | Welcome to .NET Multi-platform App UI |
| BUTTON_CLICK_ME         | Click me                              |
| BUTTON_CLICKED_1_TIME   | Clicked 1 time                        |
| BUTTON_CLICKED_N_TIMES  | Clicked {0} times!                    |

The for each language, create localized resource files in the format `<FullTypeName><.Locale>.resx` (e.g. to add translations for French, German and Chinese, you will need to create AppStrings.fr.resx, AppStrings.de.resx and AppStrings.zh.resx respectively). In Visual Studio, this is a manual process, which means, you will need to supply the translation strings for each locale you wish to support. There are some tools such as ResXManager that can automate this process using 3rd-party APIs some of which may require an API key to access.

## Set LocalizationProvider

LocalizationManager will need you to set the LocalizationProvider to perform string lookups. You can do this by using your ResourceManager's GetString function. For example:

```c# hl_lines="22-23"
// MauiProgram.cs

using SQuan.Helpers.Maui.Localization;
using HelloWorldDemo.Resources.Strings;

namespace HelloWorldDemo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

        LocalizationManager.Current.LocalizationProvider
            = AppStrings.ResourceManager.GetString;

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

```

## XAML Localize markup extension

The `Localize` markup extension convenient wires up translatable text to your .NET MAUI application. For example:

```xml hl_lines="6-7 11-13"
<!-- MainPage.xaml -->
<ContentPage
    x:Class="HelloWorldDemo.MainPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:i18n="clr-namespace:SQuan.Helpers.Maui.Localization;assembly=SQuan.Helpers.Maui.Localization"
    Title="{i18n:Localize TITLE_HOME}">
    <ScrollView>
        <VerticalStackLayout>
           <Image Source="dotnet_bot.png" />
           <Label Text="{i18n:Localize LABEL_HELLO_WORLD}" />
           <Label Text="{i18n:Localize LABEL_WELCOME}" />
           <Button x:Name="CounterBtn" Text="{i18n:Localize BUTTON_CLICK_ME}" />
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

### Fluent C# Localize extension method

Use `Localize` extension method in C# to create culture-aware bindings to your string resources. For example:

```c# hl_lines="22 24"
// MainPage.xaml.cs

using SQuan.Helpers.Maui.Localization;
using HelloWorldDemo.Resources.Strings;

namespace HelloWorldDemo;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    void OnCounterClicked(object? sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Localize(Button.TextProperty, _ => AppStrings.BUTTON_CLICKED_1_TIME);
        else
            CounterBtn.Localize(Button.TextProperty, _ => AppStrings.BUTTON_CLICKED_N_TIMES, count);

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
```

### Runtime culture switching

Use `CultureUICulture` from `LocalizationManager` to change culture at runtime. Because this is an observable property, changing its value will trigger localization updates throughout the application.

```
// Switch to French.
LocalizationManager.Current.CurrentUICulture = new CultureInfo("fr-FR");
```
