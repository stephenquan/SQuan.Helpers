# RgbaToColorConverter

## Overview

The `RgbaToColorConverter` is a multi-value converter that converts an 4-tuple containing red, green, blue, alpha values into a Color object.

!!! warning "Experimental Feature"

    This feature is experimental and has not been fully tested. Its API and behavior may change in future releases, and you may encounter unexpected issues. Use with caution and thoroughly test it in your own application before relying on it in production.

## C# Namespace

```c#
using SQuan.Helpers.Maui;
```

## Example

```c#
boxView.SetBinding(
    BoxView.ColorProperty,
    new MultiBinding
    {
        Bindings =
        {
            BindingBase.Create(static (Slider s) => s.Value, BindingMode.OneWay, source: redSlider),
            BindingBase.Create(static (Slider s) => s.Value, BindingMode.OneWay, source: greenlider),
            BindingBase.Create(static (Slider s) => s.Value, BindingMode.OneWay, source: blueSlider),
            BindingBase.Create(static (Slider s) => s.Value, BindingMode.OneWay, source: alphaSlider),
        },
        Mode = BindingMode.OneWay,
        Converter = new RgbaToColorConverter()
    }
);
```
