# FuncToMultiConverter

## Overview

The `FuncToMultiConverter` is a helper that simplifies creating `IMultiValueConverter` instances by wrapping a supplied `Func<T1..Tn, TReturn>` (supports 1 to 9 input values).

!!! warning "Experimental Feature"

    This feature is experimental and has not been fully tested. Its API and behavior may change in future releases, and you may encounter unexpected issues. Use with caution and thoroughly test it in your own application before relying on it in production.

## C# Namespace

```c#
using SQuan.Helpers.Maui;
```

## Constructor

```c#
FuncToMultiConverter(Func<T1..TN,TReturn> convert)
```

You supply your Func to the constructor.

## Example

```c#
// A Red, Green, Blue, Alpha to Color converter.
public class RgbaToColorConverter : FuncToMultiConverter<int, int, int, int, Color>
{
    public RgbaToColorConverter() : base(Color.FromRgba) { }
}
```
