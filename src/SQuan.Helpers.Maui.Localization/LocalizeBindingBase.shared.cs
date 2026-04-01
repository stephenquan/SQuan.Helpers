// LocalizeBindingBase.shared.cs

using System.Globalization;

namespace SQuan.Helpers.Maui.Localization;

/// <summary>
/// Provides functionality for creating and managing localized data bindings in applications.
/// </summary>
public static class LocalizeBindingBase
{
	/// <summary>
	/// Creates a binding for localized content using the specified key and arguments.
	/// </summary>
	/// <param name="key">The localization key used to retrieve the localized content.</param>
	/// <param name="args">An array of optional arguments to format the localized content.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(string key, object?[] args)
		=> Create(BindingBase.Create<string, string>(static s => s, BindingMode.OneWay, source: key), args);

	/// <summary>
	/// Creates a binding for localized content using the specified key binding and arguments.
	/// </summary>
	/// <param name="keyBinding">A <see cref="BindingBase"/> representing the key used for localization.</param>
	/// <param name="argBindings">An array of optional arguments to format the localized content.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(BindingBase keyBinding, IList<BindingBase> argBindings)
		=> new MultiBinding
		{
			Bindings =
			{
				keyBinding,
				BindingBase.Create<LocalizationManager, CultureInfo>(static lm => lm.CurrentCulture, BindingMode.OneWay, source: LocalizationManager.Current),
				new Binding("Culture", BindingMode.OneWay, source: new RelativeBindingSource(RelativeBindingSourceMode.FindAncestor, ancestorType: typeof(LocalizeScope))),
				BindingBase.Create<LocalizationManager, CultureInfo>(static lm => lm.CurrentUICulture, BindingMode.OneWay, source: LocalizationManager.Current),
				new Binding("UICulture", BindingMode.OneWay, source: new RelativeBindingSource(RelativeBindingSourceMode.FindAncestor, ancestorType: typeof(LocalizeScope))),
				new Binding("Resolver", BindingMode.OneWay, source: LocalizationManager.Current),
				new Binding("StringResourceResolver", BindingMode.OneWay, source: LocalizationManager.Current),
				new Binding("Resolver", BindingMode.OneWay, source: new RelativeBindingSource(RelativeBindingSourceMode.FindAncestor, ancestorType: typeof(LocalizeScope))),
				new MultiBinding
				{
					Bindings = argBindings,
					Mode = BindingMode.OneWay,
					Converter = new PassThruMultiValueConverter(),
				}
			},
			Mode = BindingMode.OneWay,
			Converter = new LocalizeMultiValueConverter()
		};

	/// <summary>
	/// Creates a binding for localized content using the specified key binding and a single argument binding.
	/// </summary>
	/// <param name="keyBinding">A <see cref="BindingBase"/> representing the key used for localization.</param>
	/// <param name="argBinding">A <see cref="BindingBase"/> representing a single argument used to format the localized content.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(BindingBase keyBinding, BindingBase argBinding)
		=> Create(keyBinding, new List<BindingBase> { argBinding });

	/// <summary>
	/// Creates a binding for localized content using the specified key binding and arguments.
	/// </summary>
	/// <param name="keyBinding">A <see cref="BindingBase"/> representing the key used for localization.</param>
	/// <param name="args">An array of optional arguments to format the localized content.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(BindingBase keyBinding, object?[] args)
	{
		List<BindingBase> bindings = new List<BindingBase>(args.Length);
		for (int i = 0; i < args.Length; i++)
		{
			bindings[i] = new Binding(".", BindingMode.OneWay, source: args[0]);
		}
		return Create(keyBinding, bindings);
	}

}
