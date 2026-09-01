# LocalizationManager Class

## Overview

LocalizationManager is a singleton service that centralizes localization and culture management for an application. It exposes observable CurrentCulture and CurrentUICulture properties and supports a pluggable LocalizationProvider for translating strings from any localization source. By implementing INotifyPropertyChanged, the manager notifies the application whenever the active culture changes, allowing UI elements and data bindings to update automatically at runtime without requiring an application restart.

In addition to culture management, LocalizationManager provides an extended GetString() API that offers fine-grained control over localization behavior. Resource strings can be retrieved using a specific UI culture, while numbers, dates, and currency values can be formatted using a different culture or fall back to the system defaults. This flexibility makes it easy to build culture-aware applications that can dynamically switch languages while preserving the appropriate regional formatting conventions.

## C# Namespace

```c#
using SQuan.Helpers.Maui.Localization;
```

## Properties

| Name                 | Description                                                   |
| -------------------- | ------------------------------------------------------------- |
| [Current](#current-property) | Provides access to the LocalizationManager singleton |
| [CurrentCulture](#currentculture-property) | Gets or sets the culture used for numbers, currency and dates |
| [CurrentUICulture](#currentuiculture-property) | Gets or sets the culture used for translating strings |
| [LocalizationProvider](#localizationprovider-property) | Gets or sets the culture-aware string lookup function |

## Methods

| Name      | Description                                                         |
| --------- | ------------------------------------------------------------------- |
| [GetString](#getstring-method) | Retrieves localized strings through the active LocalizationProvider |

## Current Property

`LocalizationManager.Current` returns the singleton `LocalizationManager` instance, providing a central point of access for localization services, culture settings, and localized string retrieval throughout the application.

## CurrentCulture Property

`LocalizationManager.Current.CurrentCulture` provides access to the culture used for formatting numbers, currency values, dates, and other culture-sensitive data. It acts as a wrapper around CultureInfo.CurrentCulture, allowing applications to both read and change the active formatting culture at runtime. When this value is updated, LocalizationManager raises the appropriate notifications so that culture-aware UI elements and data bindings can automatically refresh throughout the application.

    public CultureInfo CurrentCulture { get; set; }

```c#
// Follow numeric, currency and date formatting used in France.
LocalizationManager.Current.CurrentUICulture = new CultureInfo("fr-FR");
```

## CurrentUICulture Property

`LocalizationManager.Current.CurrentUICulture` gets or sets the culture used for retrieving localized strings and other user-facing resources. As a wrapper around `CultureInfo.CurrentUICulture`, it allows applications to change the active UI language at runtime. Changing this value triggers localization notifications, enabling culture-aware components to automatically update throughout the application.

```c#
public CultureInfo CultureUICulture { get; set; }
```

Example

```c#
// Translate all text to French.
LocalizationManager.Current.CurrentUICulture = new CultureInfo("fr-FR");
```

## LocalizationProvider Property

`LocalizationManager.Current.LocalizationProvider` gets or sets the delegate used to resolve localized strings. The provider receives a resource key and CultureInfo and returns the corresponding translated string. This allows applications to integrate with ResourceManager.GetString, RESX resources, databases, remote services, or custom localization systems. Changing this value updates the localization source used by LocalizationManager for all subsequent string lookups.

```c#
public Func<string, CultureInfo?, string?>? LocalizationProvider { get; set; }
```

Example

```c#
// Use a ResourceManager to provide localized strings.
LocalizationManager.Current.LocalizationProvider = AppStrings.ResourceManager.GetString;
```

## GetString Method

GetString() retrieves a localized string using the configured LocalizationProvider. The method supports composite string formatting by accepting format arguments and can optionally use the supplied currentUICulture and currentCulture values when resolving and formatting the result. If no cultures are provided, the application-wide CurrentUICulture and CurrentCulture values are used, allowing localized strings and culture-sensitive formatting to be resolved consistently throughout the application.

```c#
string? GetString(string key, params object[] args);
string? GetString(string key, CultureInfo? currentUICulture = null, params object[] args);
string? GetString(string key, CultureInfo? currentUICulture = null, CultureInfo? currentCulture = null, params object[] args);
```

Example

```c#
// Translate "Clicked {0} times!"
string? buttonText = LocalizationManager.Current.GetString("BUTTON_CLICKED_N_TIMES", Count);
```
