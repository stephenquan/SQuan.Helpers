namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality for creating and managing localized data bindings in applications.
/// </summary>
public static class LocalizeBinding
{
	/// <summary>
	/// Creates a binding for localized content using the specified key and arguments.
	/// </summary>
	/// <remarks>The binding is configured to use the current UI culture from the <see cref="LocalizationManager"/> 
	/// and applies a <see cref="LocalizeConverter"/> to process the localization key and arguments.</remarks>
	/// <param name="key">The localization key used to retrieve the localized content.</param>
	/// <param name="args">An array of optional arguments to format the localized content. Can be null or empty if no arguments are required.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(string key, params object?[]? args)
	{
		if (args is not null && args.Any(arg => arg is BindingBase))
		{
			return Create(BindingBase.Create(static (string k) => k, BindingMode.OneWay, source: key), args);
		}

		return BindingBase.Create(
			static (LocalizationManager lm) => lm.CurrentUICulture,
			BindingMode.OneWay,
			source: LocalizationManager.Current,
			converter: new LocalizeConverter(),
			converterParameter: new object?[] { key, args });
	}

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

		List<BindingBase> bindings = [
			keyBinding,
			cultureBinding
		];

		if (args is not null)
		{
			foreach (var arg in args)
			{
				if (arg is BindingBase argBinding)
				{
					bindings.Add(argBinding);
				}
				else
				{
					bindings.Add(BindingBase.Create(static (object o) => o, BindingMode.OneWay, source: arg));
				}
			}
		}

		return new MultiBinding
		{
			Bindings = bindings,
			Mode = BindingMode.OneWay,
			Converter = new LocalizeMultiConverter(),
		};
	}
}
