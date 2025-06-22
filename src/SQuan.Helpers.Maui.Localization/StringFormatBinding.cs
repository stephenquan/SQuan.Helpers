namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides methods for creating bindings that support localized content with optional formatting.
/// </summary>
public static class StringFormatBinding
{
	/// <summary>
	/// Creates a binding for localized content using the specified key and arguments.
	/// </summary>
	/// <param name="format">The localization key used to retrieve the localized content.</param>
	/// <param name="args">An array of optional arguments to format the localized content. Can be null or empty if no arguments are required.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(string format, params object?[]? args)
		=> Create(BindingBase.Create(static (string s) => s, BindingMode.OneWay, source: format), args);

	/// <summary>
	/// Creates a <see cref="MultiBinding"/> that combines the specified format binding and additional bindings into a
	/// single binding with string formatting capabilities.
	/// </summary>
	/// <param name="formatBinding">The primary binding that provides thei format string. This parameter cannot be <see langword="null"/>.</param>
	/// <param name="args">An array of additional bindings to be included in the <see cref="MultiBinding"/>. This parameter can be <see
	/// langword="null"/> or empty.</param>
	/// <returns>A <see cref="MultiBinding"/> configured with the specified bindings, operating in <see cref="BindingMode.OneWay"/>
	/// mode, and using a <see cref="StringFormatConverter"/> to apply string formatting.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="formatBinding"/> is <see langword="null"/>.</exception>
	public static BindingBase Create(BindingBase? formatBinding, params object?[]? args)
	{
		if (formatBinding is null)
		{
			throw new ArgumentNullException(nameof(formatBinding), "Key binding cannot be null.");
		}

		if (args is null || args.Length == 0)
		{
			return formatBinding;
		}

		List<BindingBase> bindings = [];
		bindings.Add(formatBinding);
		if (args is not null && args.Length > 0)
		{
			foreach (var arg in args)
			{
				bindings.Add((arg is BindingBase argBinding) ? argBinding : BindingBase.Create(static (object? o) => o, BindingMode.OneWay, source: arg));
			}
		}

		return new MultiBinding
		{
			Bindings = bindings,
			Mode = BindingMode.OneWay,
			Converter = new StringFormatConverter(),
		};
	}
}
