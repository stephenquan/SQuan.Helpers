using System.Text.Json;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides utility methods for working with form data, including encoding key-value pairs into URL-encoded form
/// content.
/// </summary>
/// <remarks>This class is designed to assist in preparing form data for HTTP requests where URL-encoded content
/// is required. It includes methods for serializing and encoding data in a format suitable for transmission.</remarks>
public static class FormDataHelper
{
	/// <summary>
	/// Encodes a collection of key-value pairs into a URL-encoded form content.
	/// </summary>
	/// <remarks>This method serializes each value in the <paramref name="formData"/> dictionary to a JSON string 
	/// before encoding it. The resulting content can be used in HTTP requests where URL-encoded form data  is
	/// required.</remarks>
	/// <param name="formData">A dictionary containing the form data to encode. The keys represent parameter names,  and the values are serialized
	/// into JSON format before encoding.</param>
	/// <returns>A <see cref="FormUrlEncodedContent"/> object containing the URL-encoded form data.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="formData"/> is <see langword="null"/>.</exception>
	public static FormUrlEncodedContent EncodeFormData(IDictionary<string, object?> formData)
	{
		if (formData is null)
		{
			throw new ArgumentNullException(nameof(formData));
		}

		List<KeyValuePair<string, string>> _formData = new();
		foreach (var kvp in formData)
		{
			string value = kvp.Value switch
			{
				string str => str,
				_ => JsonSerializer.Serialize(kvp.Value)
			};
			_formData.Add(new KeyValuePair<string, string>(kvp.Key, value));
		}
		return new FormUrlEncodedContent(_formData);
	}
}
