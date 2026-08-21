// LocalizeResolver.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Resolves a string value for the specified key and culture.
/// </summary>
/// <param name="key">The string key to resolve.</param>
/// <param name="culture">The culture to use for resolution.</param>
/// <returns>The resolved string.</returns>
[Obsolete("Use LocalizationManager.Current.LocalizationProvider instead.")]
public delegate string LocalizeResolver(string key, CultureInfo culture);
