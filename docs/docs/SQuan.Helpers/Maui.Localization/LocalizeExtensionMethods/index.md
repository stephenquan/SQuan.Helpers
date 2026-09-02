# LocalizeExtensionMethods

## Overview

The `LocalizeExtensionMethods` class is a static class that provides Localize extension method overloads for `BindableObject`. These extensions make it easier to create applications that support multiple languages by enabling localized and culture-aware content through a fluent C# API.

## C# Namespace

```c#
using SQuan.Helpers.Maui.Localization;
```

## Methods

| Name          | Description |
| ------------- | ----------- |
| [Localize](#localize-extension-methods) | Applies a culture-aware binding to the specified bindable object. |

## Localize Extension Methods

The `Localize` method offers a number of overloads providing different convenience around the setup of a creating a `Binding` to translable text.

```c#
BindableObject Localize<TProvider>(
    this BindableObject bindable,
    BindableProperty targetProperty,
    Func<CultureInfo, TProvider> localizationProvider,
    params object?[] args);

 BindableObject Localize(
    this BindableObject bindable,
    BindableProperty targetProperty,
    BindingBase keyBinding,
    params object?[] args);

BindableObject Localize(
    this BindableObject bindable,
    BindableProperty targetProperty,
    string key,
    params object?[] args);
```

## Example


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
