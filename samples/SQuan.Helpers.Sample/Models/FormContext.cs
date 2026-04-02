// FormContext.cs

using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using SQuan.Helpers.Maui;
using SQuan.Helpers.Maui.Localization;

namespace SQuan.Helpers.Sample;

public partial class FormContext : ObservableObject
{
	public ExpressionManager EM { get; }

	public FormContext(ExpressionManager em)
	{
		this.EM = em;
		em.SetValue<double>("/survey/x1", 3);
		em.SetValue<double>("/survey/x2", 4);
		em.SetExpression<double>("/survey/sum", "/survey/x1 + /survey/x2");
		em.SetExpression<double>("/survey/product", "/survey/x1 * /survey/x2");
	}

	static Dictionary<string, string> itextDictionary = new Dictionary<string, string>
	{
		// neutral (default)
		["/survey/x1:label"] = "Enter a value for X1",
		["/survey/x2:label"] = "Enter a value for X2",
		["/survey/sum:label"] = "The sum of X1 and X2",
		["/survey/product:label"] = "The product of X1 and X2",
		// French
		["/survey/x1:label:fr"] = "Entrez une valeur pour X1",
		["/survey/x2:label:fr"] = "Entrez une valeur pour X2",
		["/survey/sum:label:fr"] = "La somme de X1 et X2",
		["/survey/product:label:fr"] = "Le produit de X1 et X2",
		// Chinese (Simplified)
		["/survey/x1:label:zh"] = "输入 X1 的值",
		["/survey/x2:label:zh"] = "输入 X2 的值",
		["/survey/sum:label:zh"] = "X1 和 X2 的和",
		["/survey/product:label:zh"] = "X1 和 X2 的乘积",
		// Arabic
		["/survey/x1:label:ar"] = "أدخل قيمة لـ X1",
		["/survey/x2:label:ar"] = "أدخل قيمة لـ X2",
		["/survey/sum:label:ar"] = "مجموع X1 و X2",
		["/survey/product:label:ar"] = "حاصل ضرب X1 و X2",
	};

	public string GetIText(string key, CultureInfo culture)
	{
		var cultureSuffix = culture.TwoLetterISOLanguageName;
		if (itextDictionary.TryGetValue($"{key}:{cultureSuffix}", out var value))
		{
			return value;
		}
		if (itextDictionary.TryGetValue(key, out value))
		{
			return value;
		}
		return key;
	}

	public LocalizeResolver ITextResolver => GetIText;
}
