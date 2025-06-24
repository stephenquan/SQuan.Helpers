namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality for creating and managing localized data bindings in applications.
/// </summary>
public static class LocalizeBinding
{
	/// <summary>
	/// Creates a binding for localized content using the specified key and arguments.
	/// </summary>
	/// <param name="key">The localization key used to retrieve the localized content.</param>
	/// <param name="args">An array of optional arguments to format the localized content. Can be null or empty if no arguments are required.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(string key, params object?[]? args)
		=> Create(BindingBase.Create(static (string s) => s, BindingMode.OneWay, source: key), args);

	/// <summary>
	/// Creates a new <see cref="MultiBinding"/> that combines a key binding and a culture binding to enable localized
	/// value resolution.
	/// </summary>
	/// <param name="keyBinding">A <see cref="BindingBase"/> representing the key used for localization.  This parameter cannot be <see
	/// langword="null"/>.</param>
	/// <param name="args">An optional array of arguments to be passed to the converter for formatting the localized value.</param>
	/// <returns>A <see cref="MultiBinding"/> configured to resolve localized values based on the provided key binding and the
	/// current UI culture.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="keyBinding"/> is <see langword="null"/>.</exception>
	public static BindingBase Create(BindingBase? keyBinding, params object?[]? args)
	{
		if (keyBinding is null)
		{
			throw new ArgumentNullException(nameof(keyBinding), "Key binding cannot be null.");
		}

		BindingBase cultureBinding = BindingBase.Create(
			static (LocalizationManager lm) => lm.CurrentUICulture,
			BindingMode.OneWay,
			source: LocalizationManager.Current);

		BindingBase strBinding = new MultiBinding
		{
			Bindings = new List<BindingBase> { keyBinding, cultureBinding },
			Mode = BindingMode.OneWay,
			Converter = new LocalizeMultiConverter()
		};

		if (args is null || args.Length == 0)
		{
			return strBinding;
		}

		return StringFormatBinding.Create(strBinding, args);
	}
}
