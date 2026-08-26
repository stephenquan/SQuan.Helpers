// InternalBindablePropertyGenerator.cs

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SQuan.Helpers.Internals.SourceGenerators;

/// <summary>
/// Generates Bindable partial properties for properties annotated with
/// SQuan.Helpers.Internal.BindablePropertyAttribute.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class InternalBindablePropertyGenerator : IIncrementalGenerator
{
	/// <summary>
	/// Gets or sets the short name of the metadata type associated with the target attribute used for property binding.
	/// </summary>
	public string ShortTargetMetadataName { get; set; } = "InternalBindableProperty";

	/// <summary>
	/// Gets or sets the short name of the target attribute used for property binding.
	/// </summary>
	public string ShortTargetAttributeMetadataName { get; set; } = "InternalBindablePropertyAttribute";

	/// <summary>
	/// Gets or sets the name of the metadata type associated with the target attribute used for property binding.
	/// </summary>
	public string TargetAttributeMetadataName { get; set; } = "SQuan.Helpers.Internals.InternalBindablePropertyAttribute";

	/// <summary>
	/// Gets or sets the name of the target attribute used for property binding, including its fully qualified namespace.
	/// </summary>
	public string TargetAttributeFullyQualifiedName { get; set; } = "global::SQuan.Helpers.Internals.InternalBindablePropertyAttribute";

	/// <summary>
	/// Gets or sets the suffix to be used for generated source file names. The default value is ".InternalBindableProperties.g.cs".
	/// </summary>
	public string SafeFileNameSuffix { get; set; } = ".InternalBindableProperties.g.cs";

	static readonly string sGeneratedPrefix = "SQGEN_";

	static bool IsVerboseEnabled { get; } = false;

	static readonly SymbolDisplayFormat sTypeWithNullabilityFormat =
		SymbolDisplayFormat.FullyQualifiedFormat
			.WithMiscellaneousOptions(
				SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

	// Existing diagnostics
	DiagnosticDescriptor sAttributeNotFoundDescriptor => new(
		id: "SQGEN011",
		title: $"{ShortTargetAttributeMetadataName} not found",
		messageFormat: "Could not resolve attribute by metadata name: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Warning,
		isEnabledByDefault: true);

	DiagnosticDescriptor sCandidateCountDescriptor => new(
		id: "SQGEN111",
		title: $"{ShortTargetMetadataName} candidates",
		messageFormat: "Candidates: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Info,
		isEnabledByDefault: true);

	DiagnosticDescriptor sAnnotatedCountDescriptor => new(
		id: "SQGEN112",
		title: $"{ShortTargetMetadataName} annotated",
		messageFormat: "Annotated: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Info,
		isEnabledByDefault: true);

	DiagnosticDescriptor sDebugDumpDescriptor => new(
		id: "SQGEN113",
		title: $"{ShortTargetMetadataName} debug dump",
		messageFormat: "{0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Info,
		isEnabledByDefault: true);

	DiagnosticDescriptor sGeneratorFailureDescriptor => new(
		id: "SQGEN911",
		title: $"{ShortTargetMetadataName} generator failure",
		messageFormat: "Exception: {0}",
		category: "Usage",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	// New validation diagnostic: attribute applied to non-partial property
	DiagnosticDescriptor sBindablePropertyRequiresPartialDescriptor => new(
		id: "SQGEN912",
		title: $"{ShortTargetAttributeMetadataName} requires partial property",
		messageFormat: "[{ShortTargetAttributeMetadataName}] can only be applied to partial properties. Property '{0}' is not declared partial.",
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
			context.CompilationProvider.Select((compilation, _) =>
			{
				return compilation.GetTypeByMetadataName(TargetAttributeMetadataName);
			});

		// Validation pipeline: find properties with [BindableProperty] that are NOT partial, report diagnostic.
		IncrementalValuesProvider<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> invalidAttributedProperties =
			context.SyntaxProvider.CreateSyntaxProvider(
				predicate: (node, _) =>
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
					return HasBindablePropertyAttributeSyntax(p);
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

		// Attach semantic check for exact attribute type (your SQuan.Helpers.Internals.BindablePropertyAttribute)
		IncrementalValuesProvider<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> invalidAttributedPropertiesVerified =
			invalidAttributedProperties
				.Combine(attributeSymbolProvider)
				.Where(pair =>
				{
					// If we can't resolve the attribute type, we can't verify it is *your* attribute.
					// Still, you likely want to know, so we allow it only when verbose is enabled.
					if (pair.Right is null)
					{
						return IsVerboseEnabled;
					}

					return HasBindablePropertyAttribute(pair.Left.Symbol, pair.Right);
				})
				.Select(static (pair, _) =>
				{
					return pair.Left;
				});

		context.RegisterSourceOutput(invalidAttributedPropertiesVerified, (spc, item) =>
		{
			string propertyName = item.Syntax.Identifier.Text;
			spc.ReportDiagnostic(Diagnostic.Create(
				sBindablePropertyRequiresPartialDescriptor,
				item.Syntax.Identifier.GetLocation(),
				propertyName));
		});

		// Candidate partial properties (must be partial + has attributes)
		IncrementalValuesProvider<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> candidateProperties =
			context.SyntaxProvider.CreateSyntaxProvider(
				predicate: static (node, _) =>
				{
					return IsCandidate(node);
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

		// Filter to those actually annotated with your attribute
		IncrementalValuesProvider<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> annotatedProperties =
			candidateProperties
				.Combine(attributeSymbolProvider)
				.Where(pair =>
				{
					if (pair.Right is null)
					{
						return false;
					}

					return HasBindablePropertyAttribute(pair.Left.Symbol, pair.Right);
				})
				.Select(static (pair, _) =>
				{
					return pair.Left;
				});

		IncrementalValueProvider<ImmutableArray<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)>> candidatesCollected = candidateProperties.Collect();
		IncrementalValueProvider<ImmutableArray<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)>> annotatedCollected = annotatedProperties.Collect();

		context.RegisterSourceOutput(attributeSymbolProvider, (spc, attrSymbol) =>
		{
			if (!IsVerboseEnabled)
			{
				return;
			}

			if (attrSymbol is null)
			{
				spc.ReportDiagnostic(Diagnostic.Create(sAttributeNotFoundDescriptor, Location.None, TargetAttributeMetadataName));
			}
			else
			{
				string message =
					"Resolved attribute: " + attrSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) +
					" | Assembly: " + (attrSymbol.ContainingAssembly?.Identity.ToString() ?? "<null>");
				spc.ReportDiagnostic(Diagnostic.Create(sDebugDumpDescriptor, Location.None, message));
			}
		});

		context.RegisterSourceOutput(candidatesCollected, (spc, props) =>
		{
			if (!IsVerboseEnabled)
			{
				return;
			}

			spc.ReportDiagnostic(Diagnostic.Create(sCandidateCountDescriptor, Location.None, props.Length));
		});

		context.RegisterSourceOutput(annotatedCollected, (spc, props) =>
		{
			if (!IsVerboseEnabled)
			{
				return;
			}

			spc.ReportDiagnostic(Diagnostic.Create(sAnnotatedCountDescriptor, Location.None, props.Length));
		});

		// Output generation (no Cast<> needed anywhere)
		context.RegisterSourceOutput(annotatedCollected, (spc, properties) =>
		{
			try
			{
				if (properties.IsDefaultOrEmpty)
				{
					return;
				}

				foreach (IGrouping<ISymbol?, (PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> _group in
						 properties.GroupBy(p => p.Symbol.ContainingType, SymbolEqualityComparer.Default))
				{
					if (_group.Key is not INamedTypeSymbol containingType)
					{
						continue;
					}

					string src = GenerateForType(containingType, _group.ToImmutableArray());
					string hintName = GetSafeFileName(containingType) + SafeFileNameSuffix;
					//System.Diagnostics.Debugger.Launch();
					spc.AddSource(hintName, SourceText.From(src, Encoding.UTF8));
				}

				if (IsVerboseEnabled)
				{
					spc.AddSource("SQGEN_Heartbeat.g.cs", SourceText.From("// InternalBindableProperty generator heartbeat", Encoding.UTF8));
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

	bool HasBindablePropertyAttribute(IPropertySymbol property, INamedTypeSymbol attributeSymbol)
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
			if (string.Equals(fqn, TargetAttributeFullyQualifiedName, StringComparison.Ordinal))
			{
				return true;
			}
		}

		return false;
	}

	bool HasBindablePropertyAttributeSyntax(PropertyDeclarationSyntax property)
	{
		foreach (AttributeListSyntax list in property.AttributeLists)
		{
			foreach (AttributeSyntax attr in list.Attributes)
			{
				string name = attr.Name.ToString();

				// Covers: [InternalBindableProperty], [InternalBindablePropertyAttribute], and fully-qualified usage.
				if (name.Equals(ShortTargetMetadataName, StringComparison.Ordinal)
					|| name.Equals(ShortTargetAttributeMetadataName, StringComparison.Ordinal)
					|| name.EndsWith("." + ShortTargetMetadataName, StringComparison.Ordinal)
					|| name.EndsWith("." + ShortTargetAttributeMetadataName, StringComparison.Ordinal))
				{
					return true;
				}
			}
		}

		return false;
	}

	string GenerateForType(INamedTypeSymbol type, ImmutableArray<(PropertyDeclarationSyntax Syntax, IPropertySymbol Symbol)> properties)
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

		string tName = "object";

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

			tName = t.Name;

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

		foreach ((PropertyDeclarationSyntax syntax, IPropertySymbol s) in properties)
		{
			if (s is not IPropertySymbol p)
			{
				continue;
			}

			bool hasInitializer = syntax.Initializer is not null;
			var pTypeFullyQualifyFormat = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var pTypeWithNullabilityFormat = p.Type.ToDisplayString(sTypeWithNullabilityFormat);
			AttributeInfo? info = GetBindablePropertyAttributeInfo(p);
			string getAccessorModifier = GetAccessorModifier(p, isGet: true);
			string setAccessorModifier = GetAccessorModifier(p, isGet: false);

			AppendIndent(sb, indent);
			sb.AppendLine("/// <summary>");
			AppendIndent(sb, indent);
			sb.Append("/// Bindable property for <see cref=\"")
				.Append(p.Name)
				.AppendLine("\"/>.");
			AppendIndent(sb, indent);
			sb.AppendLine("/// </summary>");

			AppendIndent(sb, indent);
			sb.Append(GetAccessibility(p.DeclaredAccessibility))
			  .Append(' ')
			  .Append("static readonly BindableProperty ")
			  .Append(p.Name)
			  .Append("Property = BindableProperty.Create(nameof(")
			  .Append(p.Name)
			  .Append("), typeof(")
			  .Append(p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
			  .Append("), typeof(")
			  .Append(p.ContainingType?.Name ?? "null")
			  .Append(")");

			if (info is not null)
			{
				if (!info.InstanceMethods)
				{
					if (info.PropertyChangedMethodName is string _propertyChangedMethodName
						&& !string.IsNullOrWhiteSpace(_propertyChangedMethodName))
					{
						sb.AppendLine(",");
						AppendIndent(sb, indent + 1);
						sb.Append("propertyChanged: ")
						  .Append(_propertyChangedMethodName);
					}

					if (info.PropertyChangingMethodName is string _propertyChangingMethodName
						&& !string.IsNullOrWhiteSpace(_propertyChangingMethodName))
					{
						sb.AppendLine(",");
						AppendIndent(sb, indent + 1);
						sb.Append("propertyChanging: ")
						  .Append(_propertyChangingMethodName);
					}
				}
				else
				{
					sb.AppendLine(",");
					AppendIndent(sb, indent + 1);
					sb.AppendLine("propertyChanged: (b,o,n) =>");
					AppendIndent(sb, indent + 1);
					sb.AppendLine("{");
					AppendIndent(sb, indent + 2);
					sb.Append("((")
					  .Append(tName)
					  .Append(")b).On")
					  .Append(p.Name)
					  .Append("Changed((")
					  .Append(pTypeWithNullabilityFormat)
					  .AppendLine(")n);");
					AppendIndent(sb, indent + 2);
					sb.Append("((")
					  .Append(tName)
					  .Append(")b).On")
					  .Append(p.Name)
					  .Append("Changed((")
					  .Append(pTypeWithNullabilityFormat)
					  .Append(")o, (")
					  .Append(pTypeWithNullabilityFormat)
					  .AppendLine(")n);");
					AppendIndent(sb, indent + 1);
					sb.Append("}");
					sb.AppendLine(",");
					AppendIndent(sb, indent + 1);
					sb.AppendLine("propertyChanging: (b,o,n) =>");
					AppendIndent(sb, indent + 1);
					sb.AppendLine("{");
					AppendIndent(sb, indent + 2);
					sb.Append("((")
					  .Append(tName)
					  .Append(")b).On")
					  .Append(p.Name)
					  .Append("Changing((")
					  .Append(pTypeWithNullabilityFormat)
					  .AppendLine(")n);");
					AppendIndent(sb, indent + 2);
					sb.Append("((")
					  .Append(tName)
					  .Append(")b).On")
					  .Append(p.Name)
					  .Append("Changing((")
					  .Append(pTypeWithNullabilityFormat)
					  .Append(")o, (")
					  .Append(pTypeWithNullabilityFormat)
					  .AppendLine(")n);");
					AppendIndent(sb, indent + 1);
					sb.Append("}");
				}

				if (info.CoerceValueMethodName is string _coerceValueMethodName
					&& !string.IsNullOrWhiteSpace(_coerceValueMethodName))
				{
					sb.AppendLine(",");
					AppendIndent(sb, indent + 1);
					sb.Append("coerceValue: ")
					  .Append(_coerceValueMethodName);
				}
			}

			if (hasInitializer)
			{
				sb.AppendLine(",");
				AppendIndent(sb, indent + 1);
				sb.Append("defaultValueCreator: ")
				  .Append(sGeneratedPrefix)
				  .Append("CreateDefault")
				  .Append(p.Name);
			}
			sb.AppendLine(");");

			if (hasInitializer)
			{
				AppendIndent(sb, indent);
				sb.Append("bool ")
				  .Append(sGeneratedPrefix)
				  .Append("IsInitializing")
				  .Append(p.Name)
				  .AppendLine(" = false;");
			}

			AppendIndent(sb, indent);
			sb.Append(GetAccessibility(p.DeclaredAccessibility))
			  .Append(' ')
			  .Append("partial ")
			  .Append(pTypeWithNullabilityFormat)
			  .Append(" ")
			  .Append(p.Name)
			  .AppendLine();

			AppendIndent(sb, indent);
			sb.AppendLine("{");

			AppendIndent(sb, indent + 1);
			sb.Append(getAccessorModifier)
			  .Append("get => ");

			if (hasInitializer)
			{
				sb.Append(sGeneratedPrefix)
				  .Append("IsInitializing")
				  .Append(p.Name)
				  .Append(" ? (")
				  .Append(pTypeWithNullabilityFormat)
				  .Append(")field : ");
			}

			sb.Append("(")
				.Append(pTypeWithNullabilityFormat)
				.Append(")GetValue(")
				.Append(p.Name)
				.AppendLine("Property);");

			AppendIndent(sb, indent + 1);
			sb.Append(setAccessorModifier)
				.Append("set => SetValue(")
				.Append(p.Name)
				.AppendLine("Property, value);");

			AppendIndent(sb, indent);
			sb.AppendLine("}");

			if (hasInitializer)
			{
				AppendIndent(sb, indent);
				sb.Append("static object? ")
				  .Append(sGeneratedPrefix)
				  .Append("CreateDefault")
				  .Append(p.Name)
				  .AppendLine("(global::Microsoft.Maui.Controls.BindableObject b)");
				AppendIndent(sb, indent);
				sb.AppendLine("{");
				AppendIndent(sb, indent + 1);
				sb.Append("((")
				  .Append(tName)
				  .Append(")b).")
				  .Append(sGeneratedPrefix)
				  .Append("IsInitializing")
				  .Append(p.Name)
				  .AppendLine(" = true;");
				AppendIndent(sb, indent + 1);
				sb.Append("var result = ((")
				  .Append(tName)
				  .Append(")b).")
				  .Append(p.Name)
				  .AppendLine(";");
				AppendIndent(sb, indent + 1);
				sb.Append("((")
				  .Append(tName)
				  .Append(")b).")
				  .Append(sGeneratedPrefix)
				  .Append("IsInitializing")
				  .Append(p.Name)
				  .AppendLine(" = false;");
				AppendIndent(sb, indent + 1);
				sb.AppendLine("return result;");
				AppendIndent(sb, indent);
				sb.AppendLine("}");
			}

			if (info is not null && info.InstanceMethods)
			{
				AppendIndent(sb, indent);
				sb.Append("partial void On")
				  .Append(p.Name)
				  .Append("Changed(")
				  .Append(pTypeWithNullabilityFormat)
				  .AppendLine(" value);");
				AppendIndent(sb, indent);
				sb.Append("partial void On")
				  .Append(p.Name)
				  .Append("Changed(")
				  .Append(pTypeWithNullabilityFormat)
				  .Append(" oldValue, ")
				  .Append(pTypeWithNullabilityFormat)
				  .AppendLine(" newValue);");
				AppendIndent(sb, indent);
				sb.Append("partial void On")
				  .Append(p.Name)
				  .Append("Changing(")
				  .Append(pTypeWithNullabilityFormat)
				  .AppendLine(" value);");
				AppendIndent(sb, indent);
				sb.Append("partial void On")
				  .Append(p.Name)
				  .Append("Changing(")
				  .Append(pTypeWithNullabilityFormat)
				  .Append(" oldValue, ")
				  .Append(pTypeWithNullabilityFormat)
				  .AppendLine(" newValue);");
			}

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


	AttributeData? GetBindablePropertyAttributeData(IPropertySymbol property)
	{
		foreach (AttributeData a in property.GetAttributes())
		{
			INamedTypeSymbol? cls = a.AttributeClass;
			if (cls is null)
			{
				continue;
			}

			// Primary check (symbol identity)
			// NOTE: At generation time annotated properties already passed the symbol check,
			// but this method is called without access to that symbol, so we also keep the robust fallback.
			string fqn = cls.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			if (string.Equals(fqn, TargetAttributeFullyQualifiedName, StringComparison.Ordinal))
			{
				return a;
			}
		}

		return null;
	}

	AttributeInfo? GetBindablePropertyAttributeInfo(IPropertySymbol property)
	{
		AttributeData? a = GetBindablePropertyAttributeData(property);
		if (a is null)
		{
			return null;
		}

		AttributeInfo info = new();
		foreach (KeyValuePair<string, TypedConstant> kvp in a.NamedArguments)
		{
			object? _ = (kvp.Key, kvp.Value.Value) switch
			{
				("InstanceMethods", bool b) => info.InstanceMethods = b,
				("PropertyChangingMethodName", string s) when !string.IsNullOrWhiteSpace(s) => info.PropertyChangingMethodName = s,
				("PropertyChangedMethodName", string s) when !string.IsNullOrWhiteSpace(s) => info.PropertyChangedMethodName = s,
				("CoerceValueMethodName", string s) when !string.IsNullOrWhiteSpace(s) => info.CoerceValueMethodName = s,
				_ => null,
			};
		}

		return info;
	}

	class AttributeInfo
	{
		public bool InstanceMethods { get; set; } = true;
		public string? PropertyChangedMethodName { get; set; }
		public string? PropertyChangingMethodName { get; set; }
		public string? CoerceValueMethodName { get; set; }
	}

	static string GetAccessorModifier(IPropertySymbol property, bool isGet)
	{
		IMethodSymbol? method = isGet ? property.GetMethod : property.SetMethod;
		if (method is null)
		{
			return string.Empty;
		}

		Accessibility accessorAccessibility = method.DeclaredAccessibility;

		// If same as property accessibility, no need to repeat on accessor.
		if (accessorAccessibility == property.DeclaredAccessibility)
		{
			return string.Empty;
		}

		// NotApplicable can show up in odd cases (eg explicit interface impl); emit nothing.
		if (accessorAccessibility == Accessibility.NotApplicable)
		{
			return string.Empty;
		}

		return GetAccessibility(accessorAccessibility) + " ";
	}
}
