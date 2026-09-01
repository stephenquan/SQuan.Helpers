# FuncToMultiConverter

## Overview

The `FuncToMultiConverter` is a base class that simplifies the creation of IMultiValueConverters. It allows you to supply a `Func<T1...Tn,TReturn>` to the constructor and it builds a IMultiValueConverter based on the supplied Func. You specify up to 4 input types and 1 output type for your Func.

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
// Creates a Red, Green, Blue markup extension for quickly converting R, G, B values to a Color.
public class RgbaToColorConverter : FuncToMultiConverter<int, int, int, int, Color>
{
    public RgbaToColorConverter() : base(Color.FromRgba) { }
}
```
