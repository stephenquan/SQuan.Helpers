// BPExtrasGenerator.cs

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SQuan.Helpers.Maui.Mvvm.SourceGenerators.Helpers;

namespace SQuan.Helpers.Maui.Mvvm.SourceGenerators;

/// <summary>
/// Generates static wrapper methods for instance or partial methods defined in a class with properties decorated with
/// both CommunityToolkit.Maui's [BindableProperty] and SQuan.Helpers.Maui.Mvvm [BPExtras] attributes, enabling simpler integration
/// with the BindableProperty's PropertyChanged and PropertyChanging callbacks without needing to define static methods manually.
/// </summary>
[Generator]
public class BPExtrasGenerator : IIncrementalGenerator
{
	/// <summary>
	/// Initializes the incremental source generator by configuring the syntax provider to identify properties with
	/// specific attributes and registering the source output for code generation.
	/// </summary>
	/// <param name="context">The context for generator initialization, providing access to the syntax provider and mechanisms
	/// for registering source outputs.</param>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var properties = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (node, _)
					=> node is PropertyDeclarationSyntax propertyDeclaration
						&& propertyDeclaration.AttributeLists.Count > 0
						&& propertyDeclaration.Modifiers.Count > 0
						&& propertyDeclaration.Modifiers.IndexOf(SyntaxKind.PartialKeyword) != -1,
				transform: static (ctx, _) =>
				{
					var propertySyntax = (PropertyDeclarationSyntax)ctx.Node;
					var propertySymbol = ctx.SemanticModel.GetDeclaredSymbol(propertySyntax) as IPropertySymbol;
					if (propertySymbol is null)
					{
						return null;
					}

					bool hasBindablePropertyExtrasAttribute = false;
					bool hasBindablePropertyAttribute = false;
					foreach (var attr in propertySymbol.GetAttributes())
					{
						if (attr.AttributeClass?.Name == "CommunityToolkit.Maui.BindablePropertyAttribute" ||
							attr.AttributeClass?.ToDisplayString() == "CommunityToolkit.Maui.BindablePropertyAttribute")
						{
							hasBindablePropertyAttribute = true;
							continue;
						}

						if (attr.AttributeClass?.Name == "SQuan.Helpers.Maui.Mvvm.BPExtrasAttribute" ||
							attr.AttributeClass?.ToDisplayString() == "SQuan.Helpers.Maui.Mvvm.BPExtrasAttribute")
						{
							hasBindablePropertyExtrasAttribute = true;
							continue;
						}
					}

					return (hasBindablePropertyAttribute && hasBindablePropertyExtrasAttribute) ? propertySymbol : null;
				})
			.Where(static symbol => symbol is not null);

		context.RegisterSourceOutput(properties, (spc, propertySymbol) =>
		{
			var propertyAttributes = propertySymbol!.GetAttributes();
			var classSymbol = propertySymbol!.ContainingType;
			var className = classSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var bareClassName = classSymbol.Name;
			var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
			var propertyName = propertySymbol.Name;
			var typeName = propertySymbol.Type.ToDisplayString();

			bool hasPropertyChangedMethod = false;
			string propertyChangedMethodName = string.Empty;
			bool hasPropertyChangingMethod = false;
			string propertyChangingMethodName = string.Empty;

			foreach (var attr in propertyAttributes)
			{
				switch (attr.AttributeClass?.ToDisplayString())
				{
					case "CommunityToolkit.Maui.BindablePropertyAttribute":
						if (attr.GetAttributeValueByName("PropertyChangedMethodName").Value is string changedMethodName && !string.IsNullOrEmpty(changedMethodName))
						{
							hasPropertyChangedMethod = true;
							propertyChangedMethodName = changedMethodName;
						}
						if (attr.GetAttributeValueByName("PropertyChangingMethodName").Value is string changingMethodName && !string.IsNullOrEmpty(changingMethodName))
						{
							hasPropertyChangingMethod = true;
							propertyChangingMethodName = changingMethodName;
						}
						break;
				}
			}

			string staticWrappers = string.Empty;
			bool hasStaticWrappers = false;

			if (hasPropertyChangedMethod)
			{
				staticWrappers +=
					$$"""
						static void {{propertyChangedMethodName}}(BindableObject b, object o, object n)
						{
							(({{className}})b).{{propertyChangedMethodName}}(({{typeName}})o, ({{typeName}})n);
							(({{className}})b).{{propertyChangedMethodName}}(({{typeName}})n);
						}
						partial void {{propertyChangedMethodName}}({{typeName}} oldValue, {{typeName}} newValue);
						partial void {{propertyChangedMethodName}}({{typeName}} value);
					""";
				hasStaticWrappers = true;
			}

			if (hasPropertyChangingMethod)
			{
				staticWrappers +=
					$$"""
						static void {{propertyChangingMethodName}}(Microsoft.Maui.Controls.BindableObject b, object o, object n)
						{
							(({{className}})b).{{propertyChangingMethodName}}(({{typeName}})o, ({{typeName}})n);
							(({{className}})b).{{propertyChangingMethodName}}(({{typeName}})n);
						}
						partial void {{propertyChangingMethodName}}({{typeName}} oldValue, {{typeName}} newValue);
						partial void {{propertyChangingMethodName}}({{typeName}} value);
					""";
				hasStaticWrappers = true;
			}

			if (hasStaticWrappers)
			{
				var source =
					$$"""
					using System.ComponentModel;

					// <auto-generated/>
					#pragma warning disable
					#nullable enable

					namespace {{namespaceName}};

					partial class {{className}}
					{
						{{staticWrappers}}
					}
					""";
				spc.AddSource($"{bareClassName}_{propertyName}_BPExtras.g.cs", SourceText.From(source, Encoding.UTF8));
			}
		});
	}
}
