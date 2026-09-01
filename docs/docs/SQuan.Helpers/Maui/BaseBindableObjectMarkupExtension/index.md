# BaseBindableObjectMarkupExtension

## Overview

`BaseBindableObjectMarkupExtension` is an abstract base class that simplifies the creation of XAML markup extensions that expose bindable properties.

The class implements both `IMarkupExtension` and the required `IProvideValue` plumbing, allowing you to focus on defining bindable properties and creating the binding logic.

A typical implementation uses the `CommunityToolkit.Maui` `BindableProperty` source generator to declare properties and overrides `ProvideBindingValue` to return a `BindingBase` instance, often a `MultiBinding` with a custom converter.

Because this class is abstract, derived classes must implement `ProvideBindingValue`.

!!! warning "Experimental Feature"

    This feature is experimental and has not been fully tested. Its API and behavior may change in future releases, and you may encounter unexpected issues. Use with caution and thoroughly test it in your own application before relying on it in production.

## C# Namespace

```c#
using SQuan.Helpers.Maui;
```

## XAML Namespace

```xml
xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui"
```

## ProvideBindingValue

Derived classes must override ProvideBindingValue and return a BindingBase instance.

This method is responsible for:

 - Creating any required Binding or MultiBinding objects.
- Configuring binding modes.
- Providing value converters.
- Returning the final binding that will be applied by the markup extension.

## Example

```c#
// The following example creates an RgbaToColorExtension that converts RGBA component values into a MAUI Color.
[RequireService([typeof(IReferenceProvider), typeof(IProvideValueTarget)])]
public partial class RgbaToColorExtension : BaseBindableObjectMarkupExtension
{
    [BindableProperty] public partial int Red { get; set; } = 255;
    [BindableProperty] public partial int Green { get; set; } = 255;
    [BindableProperty] public partial int Blue { get; set; } = 255;
    [BindableProperty] public partial int Alpha { get; set; } = 255;
    public override BindingBase ProvideBindingValue(IServiceProvider serviceProvider)
        => new MultiBinding
        {
            Bindings =
            {
                BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Red, BindingMode.OneWay, source: this),
                BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Green, BindingMode.OneWay, source: this),
                BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Blue, BindingMode.OneWay, source: this),
                BindingBase.Create(static (RgbaToColorExtension ctx) => ctx.Alpha, BindingMode.OneWay, source: this)
            },
            Mode = BindingMode.OneWay,
            Converter = new FuncToMultiConverter<int, int, int, int, Color>(Color.FromRgba)
        };
}
```
