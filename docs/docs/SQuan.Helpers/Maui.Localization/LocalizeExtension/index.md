# Localize markup extension

## Overview

The `Localize` markup extension provides a convenient way to display localized text in XAML. It resolves a resource key through the current LocalizationProvider, automatically responds to CurrentUICulture changes, and supports string formatting via the X0 to X9 parameters using the current culture.

## XAML Namespace

```xml
xmlns:i18n="clr-namespace:SQuan.Helpers.Maui.Localization;assembly=SQuan.Helpers.Maui.Localization"
```

## Properties

| Name          | Description |
| ------------- | ----------- |
| Key (default) | The key to a string resource |
| X0            | The {0} formatting argument |
| X1            | The {1} formatting argument |
| X2            | The {2} formatting argument |
| X3            | The {3} formatting argument |
| X4            | The {4} formatting argument |
| X5            | The {5} formatting argument |
| X6            | The {6} formatting argument |
| X7            | The {7} formatting argument |
| X8            | The {8} formatting argument |
| X9            | The {9} formatting argument |

## Example

```xml hl_lines="6-7 11-15"
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
           <Button x:Name="CounterBtn"
                   Text="{i18n:Localize BUTTON_CLICKED_N_TIMES,
                                        X0={Binding Count}}" />
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```
