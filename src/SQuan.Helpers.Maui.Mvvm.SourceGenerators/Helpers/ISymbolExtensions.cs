using Microsoft.CodeAnalysis;

namespace SQuan.Helpers.Maui.Mvvm.SourceGenerators.Helpers;

static class ISymbolExtensions
{
	public static AttributeData GetAttribute(this ISymbol symbol, ISymbol attributeSymbol)
	{
		return symbol.GetAttributes().Single(ad => ad.AttributeClass is not null ? ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default) : false);
	}

	public static string ToPropertyName(this ISymbol symbol, AttributeData attribute)
	{
		var namedArgument = attribute.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

		if (!namedArgument.IsNull)
		{
			return namedArgument.Value is not null ? namedArgument.Value.ToString() : string.Empty;
		}

		var constructorArgument = attribute.ConstructorArguments.FirstOrDefault();
		if (!constructorArgument.IsNull && constructorArgument.Kind == TypedConstantKind.Primitive)
		{
			return constructorArgument.Value is not null ? constructorArgument.Value.ToString() : string.Empty;
		}

		var fieldName = symbol.Name;

		fieldName = fieldName.TrimStart('_');
		if (fieldName.Length == 0)
		{
			return string.Empty;
		}

		if (fieldName.Length == 1)
		{
			return fieldName.ToUpper();
		}

		return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
	}
}
