# SQuan.Helpers.Maui.Mvvm

## Overview

SQuan.Helpers.Maui.Mvvm is a source generator designed to simplify working with BindableProperty in .NET MAUI applications.

Inspired by the developer-friendly experience of [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm), this library brings a similar attribute-driven and code-generation approach to .NET MAUI bindable properties. Rather than manually writing verbose BindableProperty declarations, developers can define properties declaratively and allow the source generator to produce the required boilerplate code.

The library is aimed at developers building .NET MAUI applications with [CommunityToolkit.Maui](https://www.nuget.org/packages/CommunityToolkit.Maui) and follows modern .NET development practices. It reduces repetitive code, improves maintainability, and makes bindable property creation more approachable, particularly for developers who prefer working with instance methods and strongly typed C# code over traditional static callback patterns.

## Add NuGet package

Use the NuGet Package Manager in Visual Studio to install the [SQuan.Helpers.Maui.Mvvm](https://www.nuget.org/packages/SQuan.Helpers.Maui.Mvvm) package:

1. Select Project > Manage NuGet Packages
2. On the NuGet Package Manager page, next to Package source, select nuget.org
3. Go to the Browse tab and search for [SQuan.Helpers.Maui.Mvvm](https://www.nuget.org/packages/SQuan.Helpers.Maui.Mvvm). In the list, select [SQuan.Helpers.Maui.Mvvm](https://www.nuget.org/packages/SQuan.Helpers.Maui.Mvvm), and then select Install.

## C# Namespace

```c#
using SQuan.Helpers.Maui.Mvvm;
```

## BindableProperty attribute

The `BindableProperty` type is an attribute that allows generating bindable properties from annotated fields. Its purpose is to greatly reduce the amount of boilerplate that is needed to define bindable properties.

!!! warning "Deprecated"

    It is recommended to use the `BindableProperty` attribute from `CommunityToolkit.Maui` together with `BindablePropertyInstanceMethods` instead of the `BindableProperty` implementation provided by this library. Future maintenance and enhancements will focus on `BindablePropertyInstanceMethods`, while the internal `BindableProperty` 
    
### How it works

The `BindableProperty` attribute can be used to annotate a field in a partial type, like so:

```c#
using BindablePropertyAttribute = SQuan.Helpers.Maui.Mvvm.BindablePropertyAttribute;

public partial class CardView
{
    [BindableProperty]
    public partial string CardTitle { get; set; }
}
```

And it will generate a bindable property similar to this:

```c#
partial class CardView
{
    public static readonly BindableProperty CardTitleProperty
        = BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(CardView));

	public partial string CardTitle
	{
		 get => (string)GetValue(CardTitleProperty);
		 set => SetValue(CardTitleProperty, value);
	}
}
```

## BindablePropertyInstanceMethods attribute

The `BindablePropertyInstanceMethods` type is an attribute that allows generating of  for PropertyChanged, PropertyChanging, and/or CoerceValue static callouts from annotated fields that have also been annotated as `BindableProperty` from the CommunityToolkit.Maui. Its purpose is to greatly reduce the complexity that's needed to callout to instance methods.

### How it works

The `BindablePropertyInstanceMethods` attribute can be used with the `CommunityToolkit.Maui.BindableProperty` attribute to annotate a field in a partial type. The bindable property must also provide the `PropertyChangedMethodName`, `PropertyChangingMethodName` and/or `CoerceValueMethodName` parameters, like so:

```c#
using BindablePropertyAttribute = CommunityToolkit.Maui.BindablePropertyAttribute;
using BindablePropertyInstanceMethodsAttribute = SQuan.Helpers.Maui.Mvvm.BindablePropertyInstanceMethodsAttribute;

public partial class CardView
{
    [BindableProperty(PropertyChangedMethodName=nameof(OnCardTitleChanged))]
    [BindablePropertyInstanceMethods]
    public partial string CardTitle { get; set; }

    partial void OnCardTitleChanged(string oldValue, string newValue)
    {
        // ...
    }
}
```

And it will generate static to instance bridging callouts similar to this:

```c#
public partial class CardView
{
    #region From CommunityToolkit.Maui.BindablePropertyAttribute
    public static readonly BindableProperty CardTitleProperty
        = BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(CardView),
            propertyChanged: nameof(OnCardTitleChanged));

	public partial string CardTitle
	{
		 get => (string)GetValue(CardTitleProperty);
		 set => SetValue(CardTitleProperty, value);
	}
    #endregion

    #region From BindablePropertyInstanceMethodAttribute
    static void OnCardTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CardView)bindable).OnCardTitleChanged((string)oldValue, (string)newValue);
    }

    partial void OnCardTitleChanged(string oldValue, string newValue);
    #endregion
}
```
