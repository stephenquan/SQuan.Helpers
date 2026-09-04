# SQuan.Helpers.Maui

## Overview

SQuan.Helpers.Maui is a collection of reusable utilities designed to simplify .NET MAUI development. It provides a variety of components, behaviours, converters, markup extensions, and extension methods that help reduce boilerplate code and make it easier to build modern, maintainable MAUI applications.

The library focuses on common development scenarios and developer productivity, offering lightweight building blocks that can be used independently or together. Whether you're creating custom UI components, simplifying data binding, enhancing XAML, or adding reusable behaviours, SQuan.Helpers.Maui helps streamline the development experience and allow you to focus on building your application rather than reinventing common patterns.

## Add NuGet package

Use the NuGet Package Manager in Visual Studio to install the [SQuan.Helpers.Maui](https://www.nuget.org/packages/SQuan.Helpers.Maui) package:

1. Select Project > Manage NuGet Packages
2. On the NuGet Package Manager page, next to Package source, select nuget.org
3. Go to the Browse tab and search for [SQuan.Helpers.Maui](https://www.nuget.org/packages/SQuan.Helpers.Maui). In the list, select [SQuan.Helpers.Maui](https://www.nuget.org/packages/SQuan.Helpers.Maui), and then select Install.

## C# Namespace

```c#
using SQuan.Helpers.Maui;
```

## XAML Namespace

```xml
xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui"
```

## Classes

| Name | Description |
| ---- | -- |
| [AspectView](AspectView/index.md) | Keeps content scaled to a fixed aspect ratio. |
| [InputExtras](InputExtras/index.md) | Contains attached properties for InputViews. |

## Converters

| Name | Description |
| ---- | -- |
| [FuncToMultiConverter](FuncToMultiConverter/index.md) | Creates an IMultiValueConverter from a supplied Func delegate. |
| [RgbaToColorConverter](RgbaToColorConverter/index.md) | Converts RGBA components to a Color. |

## Markup Extensions

| Name | Description |
| ---- | -- |
| [BaseBindableObjectMarkupExtension](BaseBindableObjectMarkupExtension/index.md) | An abstract class to simplify building markup extensions. |
| [BoolToObjectExtension](BoolToObjectExtension/index.md) | A markup extension for converting a boolean to one of two objects. |
| [CompareExtension](CompareExtension/index.md) | A markup extension for returning one of two objects based on a comparison. |
| [RgbaToColorExtension](RgbaToColorExtension/index.md) | A markup extension for converting RGBA components to a Color. |
