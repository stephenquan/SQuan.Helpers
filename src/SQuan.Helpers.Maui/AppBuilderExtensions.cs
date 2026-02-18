// AppBuilderExtensions.cs

using System.Reflection;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides extension methods for configuring Lucide Icons font in a .NET MAUI application.
/// </summary>
public static class AppBuilderExtensions
{
	/// <summary>
	/// Registers Lucide Icons font for use in the MAUI application.
	/// </summary>
	/// <param name="builder">The <see cref="MauiAppBuilder"/> to configure.</param>
	/// <returns>The configured <see cref="MauiAppBuilder"/> instance.</returns>
	public static MauiAppBuilder UseSQuanHelpersMaui(this MauiAppBuilder builder)
	{
		Assembly assembly = typeof(LucideIcons).Assembly;
		builder
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup()
			.ConfigureFonts(fonts =>
			{
				fonts.AddEmbeddedResourceFont(assembly, LucideIcons.FontFile, LucideIcons.FontFamily);
			});
		return builder;
	}
}
