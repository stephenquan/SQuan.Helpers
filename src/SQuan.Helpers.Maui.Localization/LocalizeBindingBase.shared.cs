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
	public static BindingBase Create(string key, params object?[] args)
		=> Create(BindingBase.Create<string, string>(static s => s, BindingMode.OneWay, source: key), args);

	/// <summary>
	/// Creates a binding for localized content using the specified key binding and arguments.
	/// </summary>
	/// <param name="keyBinding">A <see cref="BindingBase"/> representing the key used for localization.</param>
	/// <param name="args">An array of optional arguments to format the localized content.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create(BindingBase keyBinding, params object?[] args)
		=> new MultiBinding
		{
			Bindings =
			{
				keyBinding,
				BindingBase.Create<LocalizationManager, CultureInfo>(static lm => lm.CurrentUICulture, BindingMode.OneWay, source: LocalizationManager.Current),
				BindingBase.Create<LocalizationManager, CultureInfo>(static lm => lm.CurrentCulture, BindingMode.OneWay, source: LocalizationManager.Current),
				new MultiBinding
				{
					Bindings = CoerceToBindings(args),
					Mode = BindingMode.OneWay,
					Converter = new PassThruMultiValueConverter(),
				}
			},
			Mode = BindingMode.OneWay,
			Converter = new LocalizeMultiValueConverter()
		};

	/// <summary>
	/// Creates a binding for localized content using a localization provider function and optional arguments.
	/// </summary>
	/// <typeparam name="TProvider">The type of the localization provider.</typeparam>
	/// <param name="localizationProvider">A function that provides localized content based on the current culture.</param>
	/// <param name="args">An array of optional arguments to format the localized content.</param>
	/// <returns>A <see cref="BindingBase"/> instance configured for one-way binding to the localized content.</returns>
	public static BindingBase Create<TProvider>(Func<CultureInfo, TProvider> localizationProvider, params object?[] args)
		=> new MultiBinding
		{
			Bindings =
			{
				new Binding(".", BindingMode.OneWay, source: localizationProvider),
				BindingBase.Create(static (LocalizationManager lm) => lm.CurrentUICulture, BindingMode.OneWay, source: LocalizationManager.Current),
				BindingBase.Create(static (LocalizationManager lm) => lm.CurrentCulture, BindingMode.OneWay, source: LocalizationManager.Current),
				new MultiBinding
				{
					Bindings = CoerceToBindings(args),
					Mode = BindingMode.OneWay,
					Converter = new PassThruMultiValueConverter(),
				}
			},
			Mode = BindingMode.OneWay,
			Converter = new LocalizeFuncMultiValueConverter<TProvider>()
		};

	/// <summary>
	/// Coerces an array of objects into a list of <see cref="BindingBase"/> instances.
	/// If an object is already a <see cref="BindingBase"/>, it is added directly to the list; otherwise, a new <see cref="BindingBase"/> is created for the object.
	/// </summary>
	/// <param name="args">An array of objects to be coerced into <see cref="BindingBase"/> instances.</param>
	/// <returns>A list of <see cref="BindingBase"/> instances.</returns>
	public static List<BindingBase> CoerceToBindings(params object?[] args)
		=> args
			.Select(static arg => arg as BindingBase ?? BindingBase.Create(static (object? o) => o, BindingMode.OneWay, source: arg))
			.ToList();
}
