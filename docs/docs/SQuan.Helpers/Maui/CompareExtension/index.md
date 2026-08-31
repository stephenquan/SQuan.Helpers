# Compare Markup Extension

## Overview

The `Compare` markup extension is a XAML-friendly wrapper around the CommunityToolkit.Maui CompareConverter. It simplifies the comparison of values and returns one of two objects, depending on whether the value of the comparison.

Unlike the underlying converter, all properties exposed by the markup extension are bindable. This allows values to be supplied through data bindings and ensures the conversion result automatically updates whenever the source value, ComparisonValue, ComparisonOperator, TrueObject, or FalseObject changes.

!!! warning "Experimental Feature"

    This feature is experimental and has not been fully tested. Its API and behavior may change in future releases, and you may encounter unexpected issues. Use with caution and thoroughly test it in your own application before relying on it in production.

## XAML Namespace

```xml
xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui"
```

## Fields

| Name                | Description                          |
| ------------------- | ------------------------------------ |
| ComparisonOperatorProperty | Bindable property for [ComparisonOperator](#comparisonoperator-property) |
| ComparisonValueProperty | Bindable property for [ComparisonValue](#comparisonvalue-property) |
| FalseObjectProperty | Bindable property for [FalseValue](#falseobject-property). |
| TrueObjectProperty | Bindable property for [TrueValue](#trueobject-property). |
| ValueProperty | Bindable property for [Value](#value-property). |

## Properties

| Name        | Description |
| ---         | ----------- |
| [ComparisonOperator](#comparisonoperator-property) | Gets or sets the comparison operator. |
| [ComparisonValue](#comparisonvalue-property) | Gets or sets the secondary value. |
| [FalseObject](#falseobject-property) | Gets or sets the false value. |
| [TrueObject](#trueobject-property) | Gets or sets the true value. |
| [Value](#value-property) | Gets or sets the primary value. |

## ComparisonOperator Property

Gets or sets the comparison type. Defaults to OperatorType.Equal.

```c#
public CompareConverter.OperatorType ComparisonOperator { get; set; } = CompareConverter.OperatorType.Equal;
```

## ComparisonValue Property

Gets or sets a primary value.

```c#
public IComparable Value { get; set; }
```

## FalseObject Property

Gets or sets the value that will be returned if the comparison result is false.

```c#
public object? FalseObject { get; set; }
```

## TrueObject Property

Gets or sets the value that will be returned if the comparison result is true.

```c#
public object? TrueObject { get; set; }
```

## Value Property

Gets or sets a primary value.

```c#
public IComparable Value { get; set; }
```

## Example

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui">
    <VerticalStackLayout>
        <Slider x:Name="slider" MinimumValue="0" MaximumVaue="100" Value="20" />
        <Slider x:Name="slider2" MinimumValue="0" MaximumVaue="100" Value="50" />
        <BoxView
            WidthRequest="30"
            HeightRequest="30"
            Color="{sqm:Compare Value={Binding Value,
                                               x:DataType=Slider,
                                               Source={Reference slider}},
                                ComparisonValue={Binding Value,
                                                         x:DataType=Slider,
                                                         Source={Reference slider2}},
                                ComparisonOperator=GreaterOrEqual,
                                TrueObject={Binding Source=Green},
                                FalseObject={Binding Source=Red}" />
    </VerticalStackLayout>
</ContentPage>
```
