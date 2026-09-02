# AspectView Class

## Overview

AspectView is a view component that automatically maintains a fixed aspect ratio for its content, making it easy to display images, maps, videos, and other visual elements without distortion. It dynamically measures and arranges its child view to preserve the specified width-to-height ratio while adapting to the available layout space.

By handling aspect ratio calculations for you, AspectView simplifies responsive UI design in .NET MAUI and helps ensure consistent presentation across different screen sizes, orientations, and platforms.

## C# Namespace

```c#
using SQuan.Helpers.Maui;
```

## XAML Namespace

```xml
xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui"
```

## Fields

| Name                | Description                          |
| ------------------- | ------------------------------------ |
| AspectRatioProperty | Bindable property for [AspectRatio](#aspectratio-property). |
| ContentSizeProperty | Bindable property for [ContentSize](#contentsize-property). |

## Properties

| Name        | Description |
| ---         | ----------- |
| [AspectRatio](#aspectratio-property) | Gets or sets the desired aspect ratio of the content, expressed as width ÷ height. |
| [ContentSize](#contentsize-property) | Gets the calculated size of the content after the aspect ratio has been applied. |

## AspectRatio Property

Gets or sets the desired aspect ratio of the content, expressed as width ÷ height.


```c#
public double AspectRatio { get; set; }
```

## ContentSize Property

Gets the calculated size of the content after the aspect ratio has been applied.

```c#
public Size ContentSize { get; }
```

## Example

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui">
    <sqm:AspectView Padding="30" AspectRatio="1">
        <ContentView x:Name="GameBoard">
            <!-- square game board goes here -->
        </ContentView>
    </sqm:AspectView>
</ContentPage>
```
