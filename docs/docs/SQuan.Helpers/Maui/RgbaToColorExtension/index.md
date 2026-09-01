# RgbColorExtension

## Overview

The `RgbColor` XAML markup extension that builds a Color from its red, green, blue and alpha component values.

!!! warning "Experimental Feature"

    This feature is experimental and has not been fully tested. Its API and behavior may change in future releases, and you may encounter unexpected issues. Use with caution and thoroughly test it in your own application before relying on it in production.

## XAML Namespace

```xml
xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui"
```

## Fields

| Name                | Description                          |
| ------------------- | ------------------------------------ |
| AlphaProperty | Bindable property for [Alpha](#alpha-property) |
| BlueProperty | Bindable property for [Blue](#blue-property) |
| GreenProperty | Bindable property for [Green](#green-property). |
| RedProperty | Bindable property for [Red](#red-property). |

## Properties

| Name        | Description |
| ---         | ----------- |
| [Alpha](#alpha-property) | Gets or sets the comparison operator. |
| [Blue](#blue-property) | Gets or sets the secondary value. |
| [Green](#green-property) | Gets or sets the false value. |
| [Red](#red-property) | Gets or sets the true value. |

## Alpha Property

Gets or sets the Alpha component. Defaults to 255.

```c#
public int Alpha { get; set; } = 255;
```

## Blue Property

Gets or sets the Blue component. Defaults to 255.

```c#
public int Blue { get; set; } = 255;
```

## Green Property

Gets or sets the Green component. Defaults to 255.

```c#
public int Green { get; set; } = 255;
```

## Red Property

Gets or sets the Red component. Defaults to 55.

```c#
public int Red { get; set; } = 255;
```

## Example

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui">
    <VerticalStackLayout>
        <Slider x:Name="redSlider" MinimumValue="0" MaximumVaue="255" Value="255" />
        <Slider x:Name="greenSlider" MinimumValue="0" MaximumVaue="255" Value="255" />
        <Slider x:Name="blueSlider" MinimumValue="0" MaximumVaue="255" Value="255" />
        <Slider x:Name="alphaSlider" MinimumValue="0" MaximumVaue="255" Value="255" />
        <BoxView
            WidthRequest="30"
            HeightRequest="30"
            Color="{sqm:RgbaToColor Red={Binding Value, x:DataType=Slider, Source={Reference redSlider}},
                                    Green={Binding Value, x:DataType=Slider, Source={Reference greenSlider}},
                                    Blue={Binding Value, x:DataType=Slider, Source={Reference blueSlider}},
                                    Alpha={Binding Value, x:DataType=Slider, Source={Reference alphaSlider}}}" />
    </VerticalStackLayout>
</ContentPage>
```
