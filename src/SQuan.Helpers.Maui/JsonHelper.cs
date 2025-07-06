using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides utility methods for parsing and converting JSON data into .NET objects.
/// </summary>
/// <remarks>The <see cref="JsonHelper"/> class includes static methods for working with JSON data, such as
/// parsing JSON strings into dynamic objects, populating dictionaries with JSON key-value pairs, and converting JSON
/// elements into .NET types. These methods are designed to handle various JSON structures, including nested objects and
/// arrays, and are useful for scenarios where the JSON structure is dynamic or unknown at compile time.</remarks>
public static class JsonHelper
{
	/// <summary>
	/// Parses a JSON string and converts it into a dynamic object.
	/// </summary>
	/// <remarks>The returned dynamic object allows access to JSON properties using dot notation. This method is
	/// useful for scenarios where the structure of the JSON is not known at compile time.</remarks>
	/// <param name="json">The JSON string to parse. Cannot be null or empty.</param>
	/// <returns>A dynamic object representing the parsed JSON data.</returns>
	public static dynamic Parse(string json)
	{
		var obj = new ExpandoObject();
		ConvertFromJson(obj, json);
		return obj;
	}

	/// <summary>
	/// Populates the specified dictionary with key-value pairs extracted from the provided JSON string.
	/// </summary>
	/// <remarks>This method parses the JSON string and adds its contents to the provided dictionary.  Existing
	/// entries in the dictionary may be overwritten if their keys match keys in the JSON.</remarks>
	/// <param name="dict">The dictionary to populate with data. Keys and values from the JSON string will be added to this dictionary.</param>
	/// <param name="json">A JSON-formatted string containing the data to be converted into key-value pairs.</param>
	public static void ConvertFromJson(IDictionary<string, object?> dict, string json)
	{
		JsonDocument? doc = JsonDocument.Parse(json);
		if (doc is not null)
		{
			ConvertFromJsonDocument(dict, doc);
		}
	}


	/// <summary>
	/// Populates the specified dictionary with key-value pairs extracted from a JSON document.
	/// </summary>
	/// <remarks>This method processes the root element of the provided JSON document only if it is a JSON object.
	/// If the root element is not a JSON object or if the document is <see langword="null"/>, the dictionary remains
	/// unchanged.</remarks>
	/// <param name="dict">The dictionary to populate with data. Keys are derived from the JSON object, and values are the corresponding JSON
	/// values. Existing entries in the dictionary may be overwritten if the keys match.</param>
	/// <param name="doc">The JSON document to extract data from. Must represent a JSON object at its root.</param>
	public static void ConvertFromJsonDocument(IDictionary<string, object?> dict, JsonDocument doc)
	{
		if (doc is not null && doc.RootElement.ValueKind == JsonValueKind.Object)
		{
			ConvertFromJsonObject(dict, doc.RootElement);
		}
	}

	/// <summary>
	/// Populates a dictionary with key-value pairs extracted from a JSON object.
	/// </summary>
	/// <remarks>This method recursively processes nested JSON objects and arrays. For JSON properties of type <see
	/// cref="JsonValueKind.Object"/>, a nested dictionary is created and populated. For JSON properties of type <see
	/// cref="JsonValueKind.Array"/>, a list is created and populated.  Supported JSON value types include strings,
	/// numbers, booleans, objects, and arrays. Null values are represented as <see langword="null"/> in the
	/// dictionary.</remarks>
	/// <param name="dict">The dictionary to populate with data from the JSON object. Keys are derived from the property names in the JSON
	/// object, and values are converted based on the JSON property's type.</param>
	/// <param name="jsonObject">The JSON object to convert. Each property in the JSON object is processed and added to the dictionary.</param>
	public static void ConvertFromJsonObject(IDictionary<string, object?> dict, JsonElement jsonObject)
	{
		foreach (var jsonProperty in jsonObject.EnumerateObject())
		{
			switch (jsonProperty.Value.ValueKind)
			{
				case JsonValueKind.String:
					dict[jsonProperty.Name] = ConvertFromJsonString(jsonProperty.Value) ?? null;
					break;

				case JsonValueKind.Number:
					dict[jsonProperty.Name] = ConvertFromJsonNumber(jsonProperty.Value) ?? null;
					break;

				case JsonValueKind.True:
				case JsonValueKind.False:
					dict[jsonProperty.Name] = jsonProperty.Value.GetBoolean();
					break;

				case JsonValueKind.Object:
					var nestedObj = new ExpandoObject();
					ConvertFromJsonObject(nestedObj, jsonProperty.Value);
					dict[jsonProperty.Name] = nestedObj;
					break;

				case JsonValueKind.Array:
					var nestedArray = new List<object?>();
					ConvertFromJsonArray(nestedArray, jsonProperty.Value);
					dict[jsonProperty.Name] = nestedArray;
					break;
			}
		}
	}

	/// <summary>
	/// Converts a JSON array into a list of objects, recursively processing nested arrays and objects.
	/// </summary>
	/// <remarks>This method processes each element in the provided JSON array and converts it into an appropriate
	/// .NET object. Supported JSON value types include strings, numbers, objects, and arrays. Nested arrays and objects
	/// are recursively converted. Null values are added to the list for unsupported or null JSON values.</remarks>
	/// <param name="array">The list to populate with the converted objects. This list will be modified during the method execution.</param>
	/// <param name="jsonArray">The JSON array to convert. Must be a valid <see cref="JsonElement"/> representing an array.</param>
	public static void ConvertFromJsonArray(List<object?> array, JsonElement jsonArray)
	{
		for (int i = 0; i < jsonArray.GetArrayLength(); i++)
		{
			var jsonValue = jsonArray[i];
			switch (jsonValue.ValueKind)
			{
				case JsonValueKind.String:
					array.Add(ConvertFromJsonString(jsonValue) ?? null);
					break;

				case JsonValueKind.Number:
					array.Add(ConvertFromJsonNumber(jsonValue) ?? null);
					break;

				case JsonValueKind.Object:
					var newArrayItem = new ExpandoObject();
					ConvertFromJsonObject(newArrayItem, jsonValue);
					array.Add(newArrayItem);
					break;

				case JsonValueKind.Array:
					var nestedArray = new List<object?>();
					ConvertFromJsonArray(nestedArray, jsonValue);
					array.Add(nestedArray);
					break;
			}
		}
	}

	/// <summary>
	/// Converts a JSON number represented by a <see cref="JsonElement"/> into its corresponding .NET numeric type.
	/// </summary>
	/// <remarks>This method attempts to convert the JSON number to the most appropriate .NET numeric type based on
	/// its precision. If the number fits within the range of an <see cref="int"/>, it will be returned as an <see
	/// cref="int"/>. If it exceeds the range of an <see cref="int"/> but fits within a <see cref="long"/>, it will be
	/// returned as a <see cref="long"/>. For floating-point numbers, the method will return a <see cref="float"/> or <see
	/// cref="double"/> depending on the precision.</remarks>
	/// <param name="number">A <see cref="JsonElement"/> containing the JSON number to convert. The element must represent a valid numeric
	/// value.</param>
	/// <returns>The converted numeric value as an <see cref="object"/>. The return type will be one of the following: <see
	/// cref="int"/>, <see cref="long"/>, <see cref="float"/>, or <see cref="double"/>, depending on the precision of the
	/// JSON number. Returns <see langword="null"/> if the JSON number cannot be converted.</returns>
	public static object? ConvertFromJsonNumber(JsonElement number)
	{
		if (number.TryGetInt32(out int intValue))
		{
			return intValue;
		}
		if (number.TryGetInt64(out long longValue))
		{
			return longValue;
		}
		else if (number.TryGetSingle(out float floatValue))
		{
			return floatValue;
		}
		else if (number.TryGetDouble(out double doubleValue))
		{
			return doubleValue;
		}
		return null;
	}

	/// <summary>
	/// Converts a <see cref="JsonElement"/> representing a JSON string into a .NET string.
	/// </summary>
	/// <remarks>This method returns <see langword="null"/> if the <paramref name="jsonElement"/> does not represent
	/// a JSON string.</remarks>
	/// <param name="jsonElement">The <see cref="JsonElement"/> to convert. Must have a <see cref="JsonValueKind"/> of <see
	/// cref="JsonValueKind.String"/>.</param>
	/// <returns>The string value contained in the <paramref name="jsonElement"/> if its <see cref="JsonValueKind"/> is <see
	/// cref="JsonValueKind.String"/>;  otherwise, <see langword="null"/>.</returns>
	public static string? ConvertFromJsonString(JsonElement jsonElement)
	{
		if (jsonElement.ValueKind == JsonValueKind.String)
		{
			return jsonElement.GetString();
		}
		return null;
	}

}
