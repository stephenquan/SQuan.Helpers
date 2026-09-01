# FuncToConverter

## Overview

The `FuncToConverter` is a base class that simplifies the creation of IMultiValueConverters. It allows you to supply a `Func<T,TReturn>` to the constructor and it builds a IValueConverter based on the supplied Func. You specify up to 4 input types and 1 output type for your Func.

!!! warning "Experimental Feature"

    This feature is experimental and has not been fully tested. Its API and behavior may change in future releases, and you may encounter unexpected issues. Use with caution and thoroughly test it in your own application before relying on it in production.

## C# Namespace

```c#
using SQuan.Helpers.Maui;
```

## Constructor

```c#
FuncToConverter(Func<T,TReturn> convert)
```

You supply your Func to the constructor.

## Example

```c#
// Creates a currency to color converter with negative currency being red and positive currency being green.
public class BalanceToColorConverter : FuncToConverter<double, Color>
{
    public BalanceToColorConverter() : base(v => v >= 0d ? Colors.Green : Colors.Red) { }
}
```
