# InputExtras Attached Properties

## Overview

The `InputExtras` is a class containing attached properties for InputViews (i.e. Entry and Editor). It is backed by a platform behavior implementation to deliver additional capabilities.

## C# Namespace

```c#
using SQuan.Helpers.Maui;
```

## XAML Namespace

```xml
xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui"
```

## Attached Properties

| Name            | Description                                              |
| --------------- | -------------------------------------------------------- |
| [BorderThickness](#borderthickness-attached-property) | Gets or sets the InputView's border thickness.           |
| [InputMode](#inputmode-attached-property) | Gets or sets the InputView's input filter mode.          |
| [InputPattern](#inputpattern-attached-property) | Gets or sets the InputView's input filter regex pattern. |

## BorderThickness Attached Property

The `InputExtras.BorderThickness` attached property, when set to zero (0), will make the InputViews borderless. When set to non-zero (e.g. 1), will restore the InputViews' border. For thickness > 1 there is inconsistent behavior, for example, on Windows, Mac and iOS, the border may become thicker. On Android, however, all non-zero thickness will appear to have a normal border.

## InputMode Attached Property

The `InputExtras.InputMode` attached property can be set to
 - `InputMode.None` (no filtering applied)
 - `InputMode.Integer` (supports entering of integers)
 - `InputMode.Decimal` (supports entering of decimals)
 - `InputMode.Pattern` (allows a custom-defined pattern)

The implementation makes use of platform implementation to ensure that the characters are optimally blocked. On Android platform, you may observe additional TextChanged events as a correction is applied to reverse invalid input. On other platforms, the invalid input is filtered earlier.

## InputPattern Attached Property

The `InputExtras.InputPattern` attached property is checked only when the `InputExtras.InputMode` is set to `InputMode.Pattern`. It allows one to supply a regular expression that describes the valid input. Note you must pattern-match intermediate valid input, not just the final input. So license plate AAA999 validation must be `"^([A-Z]{0,2}|[A-Z]{3}[0-9]{0,3})$"` not `"^[A-Z]{3}[0-9]{3}$`.

## Example

```xml
<ContentPage
    xmlns:sqm="clr-namespace:SQuan.Helpers.Maui;assembly=SQuan.Helpers.Maui">
    <VerticalStackLayout>
        <Entry
            sqm:InputExtras.BorderThickness="1"
            sqm:InputExtras.InputMode="Integer"
            Placeholder="Enter an integer."
            Keyboard="Numeric" />

        <Entry
            sqm:InputExtras.BorderThickness="1"
            sqm:InputExtras.InputMode="Decimal"
            Placeholder="Enter a decimal."
            Keyboard="Numeric" />

        <Entry
            sqm:InputExtras.BorderThickness="1"
            sqm:InputExtras.InputMode="Pattern"
            sqm:InputExtras.InputPattern="^([A-Za-z]{0,2}|[A-Za-z]{3}[0-9]{0,3})$"
            Placeholder="Enter a license plate (AAA999)."
            Keyboard="Default" />
    </VerticalStackLayout>
</ContentPage>
```