# BoolToObject Markup Extension

## Overview

The `BoolToObject` markup extension is a XAML-friendly wrapper around the CommunityToolkit.Maui BoolToObjectConverter. It simplifies converting a bool value into one of two objects, depending on whether the value is true or false.

Unlike the underlying converter, all properties exposed by the markup extension are bindable. This allows values to be supplied through data bindings and ensures the conversion result automatically updates whenever the source value, TrueObject, or FalseObject changes.

!!! warning "Experimental Feature"

    This feature is experimental and has not been fully tested. Its API and behavior may change in future releases, and you may encounter unexpected issues. Use with caution and thoroughly test it in your own application before relying on it in production.

## XAML Namespace

```xml
xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui"
```

## Fields

| Name                | Description                          |
| ------------------- | ------------------------------------ |
| FalseObjectProperty | Bindable property for [FalseObject](#falseobject-property). |
| TrueObjectProperty | Bindable property for [TrueObject](#trueobject-property). |
| ValueProperty | Bindable property for [Value](#value-property). |

## Properties

| Name        | Description |
| ---         | ----------- |
| [FalseObject](#falseobject-property) | Gets or sets the false value. |
| [TrueObject](#trueobject-property) | Gets or sets the true value. |
| [Value](#value-property) | Gets or sets the controlling boolean value. |

## FalseObject Property

Gets or sets the value that will be returned if the controlling boolean value is false.

```c#
public object? FalseObject { get; set; }
```

## TrueObject Property

Gets or sets the value that will be returned if the controlling boolean value is true.

```c#
public object? TrueObject { get; set; }
```


## Value Property

Gets or sets a controlling boolean value.

```c#
public bool Value { get; set; }
```

## Example

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui">
    <VerticalStackLayout>
        <CheckBox x:Name="chk" />
        <BoxView
            WidthRequest="30"
            HeightRequest="30"
            Color="{sqm:BoolToObject Value={Binding IsChecked,
                                                    x:DataType=CheckBox,
                                                    Source={Reference chk}},
                                     TrueObject={Binding Source=Green},
                                     FalseObject={Binding Source=Red}" />
    </VerticalStackLayout>
</ContentPage>
```
