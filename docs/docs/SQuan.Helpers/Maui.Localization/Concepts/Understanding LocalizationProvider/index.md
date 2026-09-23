# Understanding LocalizationProvider

Most applications configure localization by assigning a `ResourceManager` to [LocalizationManager.Current.LocalizationProvider](../../LocalizationManager/index.md#localizationprovider-property):

```csharp
LocalizationManager.Current.LocalizationProvider =
    AppStrings.ResourceManager.GetString;
```

For many applications, this is all that is required.

However, the [LocalizationProvider](../../LocalizationManager/index.md#localizationprovider-property) property was intentionally designed as:

```csharp
Func<string, CultureInfo?, string?>
```

rather than a `ResourceManager`.

This abstraction decouples the localization system from any specific storage mechanism. While a `ResourceManager` is a natural implementation, any source that can resolve a key to a localized string can be used.

This design makes it possible to:

- Use multiple resource managers.
- Support modular applications where each assembly owns its own resources.
- Integrate database-backed localization.
- Use dictionary-based localization for testing.
- Connect to third-party localization services.
- Implement custom lookup and fallback behavior.

## LocalizationProvider as an Abstraction

From the perspective of the localization system, only one thing matters: a function that accepts:

```csharp
(string key, CultureInfo? culture)
```

and returns:

```csharp
string?
```

Everything else is an implementation detail.

Using `ResourceManager.GetString()` is simply one implementation of that contract:

```csharp
LocalizationManager.Current.LocalizationProvider =
    AppStrings.ResourceManager.GetString;
```

Because the API accepts a delegate, applications are free to introduce additional behavior before deciding how a key should be resolved.

## Supporting Multiple Resource Managers

In larger applications, localized strings are often distributed across multiple assemblies. For example, an application might define its core user interface strings in `App.Core`, while data-related messages and validations are maintained in `App.Data`.

Rather than consolidating all strings into a single resource file, a [LocalizationProvider](../../LocalizationManager/index.md#localizationprovider-property) can perform lookups across multiple `ResourceManager` instances.

## Looking Up Prefixed Keys

The provider can inspect incoming keys and route the lookup to the appropriate localization source.

For example:

```text
APPCORE_LoginButton
APPDATA_RecordCount
```

A lookup implementation could:

1. Extract the prefix.
2. Find the registered localizer.
3. Remove the prefix.
4. Delegate the lookup.

Conceptually:

```csharp
APPCORE_LoginButton
```

becomes:

```csharp
LoginButton
```

which is then passed to:

```csharp
App.Core.CoreStrings.ResourceManager.GetString
```

This allows localization resources to remain owned by their respective modules while presenting a single localization provider to the rest of the application.

## Example: Using Multiple Resource Managers

```c#
using System.Globalization;

var registeredResourceManagers =
    new Dictionary<string, Func<string, CultureInfo?, string?>>(
        StringComparer.OrdinalIgnoreCase)
    {
        ["APPCORE"] = App.Core.CoreStrings.ResourceManager.GetString,
        ["APPDATA"] = App.Data.DataStrings.ResourceManager.GetString,
    };

LocalizationManager.Current.LocalizationProvider = (key, culture) =>
{
    if (key.IndexOf('_', StringComparison.Ordinal) is int separatorIndex
        && separatorIndex > 0)
    {
        var prefix = key[..separatorIndex];
        var resourceKey = key[(separatorIndex + 1)..];

        if (registeredResourceManagers.TryGetValue(prefix, out var localizer))
        {
            return localizer(resourceKey, culture);
        }
    }

    return null;
};
```

## Using a Custom Data Source

[LocalizationProvider](../../LocalizationManager/index.md#localizationprovider-property) is not limited to .NET resource files. Because it is simply a delegate, it can retrieve localized strings from any source.

One simple approach is to store translations in a `Dictionary<string, string?>`. The dictionary key can contain both the logical string identifier and an optional culture suffix.

For example:

| Key | Value |
|------|---------|
| `itext:/form/question1:label` | What is your name? |
| `itext:/form/question1:label:fr` | Quel est votre nom ? |
| `itext:/form/question1:label:de` | Wie heißen Sie? |

When a key begins with the `itext:` prefix, the provider can first attempt to locate a language-specific translation and then fall back to the language-neutral version.

## Example: Using a Custom Data Source

```c#
var strings = new Dictionary<string, string?>
{
    ["/form/question1:label"] = "What is your name?",
    ["/form/question1:label:fr"] = "Quel est votre nom ?",
    ["/form/question1:label:de"] = "Wie heißen Sie?"
};

LocalizationManager.Current.LocalizationProvider = (key, culture) =>
{
    if (!key.StartsWith("itext:", StringComparison.OrdinalIgnoreCase))
    {
        return null;
    }

    var lookupKey = key["itext:".Length..];

    if (culture is not null
        && strings.TryGetValue(
            $"{lookupKey}:{culture.TwoLetterISOLanguageName}",
            out var localized))
    {
        return localized;
    }

    return strings.TryGetValue(lookupKey, out var neutral)
        ? neutral
        : null;
};
```

In this example, a request for `itext:/form/question1:label` using the French culture (`fr-FR`) first attempts to resolve:

```
/form/question1:label:fr
```

If no French translation exists, the provider falls back to:

```
/form/question1:label
```

This pattern allows applications to support language-specific translations while maintaining a language-neutral default. The same approach can be adapted to databases, JSON files, remote services, or any other localization store.
