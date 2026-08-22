// InternalObservablePropertyGenerator.cs

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SQuan.Helpers.Internals.SourceGenerators;

/// <summary>
/// Generates observable partial properties for properties annotated with
/// SQuan.Helpers.Internal.ObservablePropertyAttribute.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class InternalObservablePropertyGenerator : IIncrementalGenerator
{
	const string kTargetAttributeMetadataName = "SQuan.Helpers.Internals.InternalObservablePropertyAttribute";
	const string kTargetAttributeFullyQualifiedName = "global::SQuan.Helpers.Internals.InternalObservablePropertyAttribute";

	static bool IsVerboseEnabled { get; } = false;

	static readonly SymbolDisplayFormat sTypeWithNullabilityFormat =
		SymbolDisplayFormat.FullyQualifiedFormat
			.WithMiscellaneousOptions(
				SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

	// Existing diagnostics
	static readonly DiagnosticDescriptor sAttributeNotFoundDescriptor = new(
		id: "SQGEN001",
		title: "ObservablePropertyAttribute not found",
		messageFormat: "Could not resolve attribute by metadata name: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Warning,
		isEnabledByDefault: true);

	static readonly DiagnosticDescriptor sCandidateCountDescriptor = new(
		id: "SQGEN101",
		title: "ObservableProperty candidates",
		messageFormat: "Candidates: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Info,
		isEnabledByDefault: true);

	static readonly DiagnosticDescriptor sAnnotatedCountDescriptor = new(
		id: "SQGEN102",
		title: "ObservableProperty annotated",
		messageFormat: "Annotated: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Info,
		isEnabledByDefault: true);

	static readonly DiagnosticDescriptor sDebugDumpDescriptor = new(
		id: "SQGEN103",
		title: "ObservableProperty debug dump",
		messageFormat: "{0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Info,
		isEnabledByDefault: true);

	static readonly DiagnosticDescriptor sGeneratorFailureDescriptor = new(
		id: "SQGEN901",
		title: "ObservableProperty generator failure",
		messageFormat: "Exception: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	// New validation diagnostic: attribute applied to non-partial property
	static readonly DiagnosticDescriptor sObservablePropertyRequiresPartialDescriptor = new(
		id: "SQGEN902",
		title: "ObservablePropertyAttribute requires partial property",
		messageFormat: "[ObservableProperty] can only be applied to partial properties. Property '{0}' is not declared partial.",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Error, // change to Warning if desired
		isEnabledByDefault: true);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValueProvider<INamedTypeSymbol?> attributeSymbolProvider =
			context.CompilationProvider.Select(static (compilation, _) =>
			{
				return compilation.GetTypeByMetadataName(kTargetAttributeMetadataName);
			});

		// Validation pipeline: find properties with [ObservableProperty] that are NOT partial, report diagnostic.
		IncrementalValuesProvider<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> invalidAttributedProperties =
			context.SyntaxProvider.CreateSyntaxProvider(
					predicate: static (node, _) =>
					{
						if (node is not PropertyDeclarationSyntax p)
						{
							return false;
						}

						if (p.AttributeLists.Count == 0)
						{
							return false;
						}

						// Only interested in non-partial properties
						if (p.Modifiers.Any(SyntaxKind.PartialKeyword))
						{
							return false;
						}

						// Cheap syntax name check to avoid semantic work where not needed
						return HasObservablePropertyAttributeSyntax(p);
					},
					transform: static (ctx, _) =>
					{
						var syntax = (PropertyDeclarationSyntax)ctx.Node;
						var symbol = ctx.SemanticModel.GetDeclaredSymbol(syntax) as IPropertySymbol;
						return (Syntax: syntax, Symbol: symbol);
					})
				.Where(static x =>
				{
					return x.Symbol is not null;
				})!
				.Select(static (x, _) =>
				{
					return (x.Syntax, x.Symbol!);
				});

		// Attach semantic check for exact attribute type (your SQuan.Helpers.Internals.ObservablePropertyAttribute)
		IncrementalValuesProvider<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> invalidAttributedPropertiesVerified =
			invalidAttributedProperties
				.Combine(attributeSymbolProvider)
				.Where(static pair =>
				{
					// If we can't resolve the attribute type, we can't verify it is *your* attribute.
					// Still, you likely want to know, so we allow it only when verbose is enabled.
					if (pair.Right is null)
					{
						return IsVerboseEnabled;
					}

					return HasObservablePropertyAttribute(pair.Left.Symbol, pair.Right);
				})
				.Select(static (pair, _) =>
				{
					return pair.Left;
				});

		context.RegisterSourceOutput(invalidAttributedPropertiesVerified, static (spc, item) =>
		{
			string propertyName = item.Syntax.Identifier.Text;
			spc.ReportDiagnostic(Diagnostic.Create(
				sObservablePropertyRequiresPartialDescriptor,
				item.Syntax.Identifier.GetLocation(),
				propertyName));
		});

		// Candidate partial properties (must be partial + has attributes)
		IncrementalValuesProvider<IPropertySymbol> candidateProperties =
			context.SyntaxProvider.CreateSyntaxProvider(
					predicate: static (node, _) =>
					{
						return IsCandidate(node);
					},
					transform: static (ctx, _) =>
					{
						return GetPropertySymbol(ctx);
					})
				.Where(static symbol =>
				{
					return symbol is not null;
				})!
				.Select(static (symbol, _) =>
				{
					return symbol!;
				});

		// Filter to those actually annotated with your attribute
		IncrementalValuesProvider<IPropertySymbol> annotatedProperties =
			candidateProperties
				.Combine(attributeSymbolProvider)
				.Where(static pair =>
				{
					if (pair.Right is null)
					{
						return false;
					}

					return HasObservablePropertyAttribute(pair.Left, pair.Right);
				})
				.Select(static (pair, _) =>
				{
					return pair.Left;
				});

		IncrementalValueProvider<ImmutableArray<IPropertySymbol>> candidatesCollected = candidateProperties.Collect();
		IncrementalValueProvider<ImmutableArray<IPropertySymbol>> annotatedCollected = annotatedProperties.Collect();

		context.RegisterSourceOutput(attributeSymbolProvider, static (spc, attrSymbol) =>
		{
			if (!IsVerboseEnabled)
			{
				return;
			}

			if (attrSymbol is null)
			{
				spc.ReportDiagnostic(Diagnostic.Create(sAttributeNotFoundDescriptor, Location.None, kTargetAttributeMetadataName));
			}
			else
			{
				string message =
					"Resolved attribute: " + attrSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) +
					" | Assembly: " + (attrSymbol.ContainingAssembly?.Identity.ToString() ?? "<null>");
				spc.ReportDiagnostic(Diagnostic.Create(sDebugDumpDescriptor, Location.None, message));
			}
		});

		context.RegisterSourceOutput(candidatesCollected, static (spc, props) =>
		{
			if (!IsVerboseEnabled)
			{
				return;
			}

			spc.ReportDiagnostic(Diagnostic.Create(sCandidateCountDescriptor, Location.None, props.Length));
		});

		context.RegisterSourceOutput(annotatedCollected, static (spc, props) =>
		{
			if (!IsVerboseEnabled)
			{
				return;
			}

			spc.ReportDiagnostic(Diagnostic.Create(sAnnotatedCountDescriptor, Location.None, props.Length));
		});

		// Output generation (no Cast<> needed anywhere)
		context.RegisterSourceOutput(annotatedCollected, static (spc, properties) =>
		{
			try
			{
				if (properties.IsDefaultOrEmpty)
				{
					return;
				}

				foreach (IGrouping<ISymbol?, IPropertySymbol> _group in
						 properties.GroupBy(p => p.ContainingType, SymbolEqualityComparer.Default))
				{
					if (_group.Key is not INamedTypeSymbol containingType)
					{
						continue;
					}

					string src = GenerateForType(containingType, _group.ToImmutableArray());
					string hintName = GetSafeFileName(containingType) + ".InternalObservableProperties.g.cs";
					//System.Diagnostics.Debugger.Launch();
					spc.AddSource(hintName, SourceText.From(src, Encoding.UTF8));
				}

				if (IsVerboseEnabled)
				{
					spc.AddSource("SQGEN_Heartbeat.g.cs", SourceText.From("// generator heartbeat", Encoding.UTF8));
				}
			}
			catch (Exception ex)
			{
				spc.ReportDiagnostic(Diagnostic.Create(sGeneratorFailureDescriptor, Location.None, ex.ToString()));
			}
		});
	}

	static bool IsCandidate(SyntaxNode node)
	{
		if (node is not PropertyDeclarationSyntax p)
		{
			return false;
		}

		if (!p.Modifiers.Any(SyntaxKind.PartialKeyword))
		{
			return false;
		}

		if (p.AttributeLists.Count == 0)
		{
			return false;
		}

		return true;
	}

	static IPropertySymbol? GetPropertySymbol(GeneratorSyntaxContext context)
	{
		if (context.Node is not PropertyDeclarationSyntax p)
		{
			return null;
		}

		if (p.AccessorList is null)
		{
			return null;
		}

		bool hasGet = false;
		bool hasSet = false;

		foreach (AccessorDeclarationSyntax a in p.AccessorList.Accessors)
		{
			if (a.Body is not null || a.ExpressionBody is not null)
			{
				return null;
			}

			if (a.IsKind(SyntaxKind.GetAccessorDeclaration))
			{
				hasGet = true;
			}
			else if (a.IsKind(SyntaxKind.SetAccessorDeclaration))
			{
				hasSet = true;
			}
			else if (a.IsKind(SyntaxKind.InitAccessorDeclaration))
			{
				return null;
			}
		}

		if (!hasGet || !hasSet)
		{
			return null;
		}

		if (context.SemanticModel.GetDeclaredSymbol(p) is not IPropertySymbol symbol)
		{
			return null;
		}

		if (symbol.IsStatic)
		{
			return null;
		}

		return symbol;
	}

	static bool HasObservablePropertyAttribute(IPropertySymbol property, INamedTypeSymbol attributeSymbol)
	{
		foreach (AttributeData a in property.GetAttributes())
		{
			INamedTypeSymbol? cls = a.AttributeClass;
			if (cls is null)
			{
				continue;
			}

			if (SymbolEqualityComparer.Default.Equals(cls, attributeSymbol))
			{
				return true;
			}

			// Robust fallback (multi-targeting / symbol identity oddities)
			string fqn = cls.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			if (string.Equals(fqn, kTargetAttributeFullyQualifiedName, StringComparison.Ordinal))
			{
				return true;
			}
		}

		return false;
	}

	static bool HasObservablePropertyAttributeSyntax(PropertyDeclarationSyntax property)
	{
		foreach (AttributeListSyntax list in property.AttributeLists)
		{
			foreach (AttributeSyntax attr in list.Attributes)
			{
				string name = attr.Name.ToString();

				// Covers: [ObservableProperty], [ObservablePropertyAttribute], and fully-qualified usage.
				if (name is "ObservableProperty" or "ObservablePropertyAttribute"
					|| name.EndsWith(".ObservableProperty", StringComparison.Ordinal)
					|| name.EndsWith(".ObservablePropertyAttribute", StringComparison.Ordinal))
				{
					return true;
				}
			}
		}

		return false;
	}

	static string GenerateForType(INamedTypeSymbol type, ImmutableArray<IPropertySymbol> properties)
	{
		var sb = new StringBuilder();
		sb.AppendLine("// <auto-generated/>");
		sb.AppendLine("#nullable enable");
		sb.AppendLine("#pragma warning disable");
		sb.AppendLine();

		string? ns = type.ContainingNamespace?.IsGlobalNamespace == false
			? type.ContainingNamespace.ToDisplayString()
			: null;

		if (ns is not null)
		{
			sb.Append("namespace ").Append(ns).AppendLine(";");
			sb.AppendLine();
		}

		var chain = new Stack<INamedTypeSymbol>();
		for (INamedTypeSymbol? t = type; t is not null; t = t.ContainingType)
		{
			chain.Push(t);
		}

		int indent = 0;

		while (chain.Count > 0)
		{
			INamedTypeSymbol t = chain.Pop();

			AppendIndent(sb, indent);

			sb.Append(GetAccessibility(t.DeclaredAccessibility))
			  .Append(' ')
			  .Append("partial ")
			  .Append(GetTypeKind(t))
			  .Append(' ')
			  .Append(t.Name);

			if (t.TypeParameters.Length > 0)
			{
				sb.Append('<')
				  .Append(string.Join(", ", t.TypeParameters.Select(tp => tp.Name)))
				  .Append('>');
			}

			sb.AppendLine();
			AppendIndent(sb, indent);
			sb.AppendLine("{");
			indent++;
		}

		foreach (ISymbol? s in properties.Distinct(SymbolEqualityComparer.Default))
		{
			if (s is not IPropertySymbol p)
			{
				continue;
			}

			AppendIndent(sb, indent);
			sb.Append(GetAccessibility(p.DeclaredAccessibility))
			  .Append(' ')
			  .Append("partial ")
			  //.Append(p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
			  .Append(p.Type.ToDisplayString(sTypeWithNullabilityFormat)).Append(' ')
			  .Append(p.Name)
			  .AppendLine();

			AppendIndent(sb, indent);
			sb.AppendLine("{");

			AppendIndent(sb, indent + 1);
			sb.AppendLine("get => field;");

			AppendIndent(sb, indent + 1);
			sb.AppendLine("set => SetProperty(ref field, value);");

			AppendIndent(sb, indent);
			sb.AppendLine("}");
			sb.AppendLine();
		}

		while (indent > 0)
		{
			indent--;
			AppendIndent(sb, indent);
			sb.AppendLine("}");
		}

		sb.AppendLine("#pragma warning restore");
		return sb.ToString();
	}

	static string GetTypeKind(INamedTypeSymbol t)
	{
		if (t.TypeKind == TypeKind.Struct)
		{
			return "struct";
		}

		return "class";
	}

	static string GetAccessibility(Accessibility a)
	{
		return a switch
		{
			Accessibility.Public => "public",
			Accessibility.Internal => "internal",
			Accessibility.Private => "private",
			Accessibility.Protected => "protected",
			Accessibility.ProtectedAndInternal => "private protected",
			Accessibility.ProtectedOrInternal => "protected internal",
			_ => "internal"
		};
	}

	static void AppendIndent(StringBuilder sb, int indent)
	{
		sb.Append(' ', indent * 4);
	}

	static string GetSafeFileName(INamedTypeSymbol type)
	{
		var names = new Stack<string>();
		for (INamedTypeSymbol? t = type; t is not null; t = t.ContainingType)
		{
			names.Push(t.Name);
		}

		return string.Join(".", names).Replace('<', '_').Replace('>', '_');
	}
}
