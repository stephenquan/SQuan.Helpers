using System.Text.Json;

namespace SQuan.Helpers.Maui;

/// <summary>
/// Provides extension methods for the <see cref="HttpClient"/> class to simplify common HTTP operations.
/// </summary>
/// <remarks>This class includes methods for sending HTTP requests and processing responses, such as posting form
/// data and parsing JSON responses. These methods are designed to streamline typical use cases for <see
/// cref="HttpClient"/>, reducing boilerplate code and improving readability.</remarks>
public static class HttpClientHelper
{
	/// <summary>
	/// Sends a POST request to the specified URL with the provided form data.
	/// </summary>
	/// <remarks>This method sends a POST request with form-encoded data. Ensure that the <paramref name="url"/> is
	/// valid and the <paramref name="formData"/> contains the required fields expected by the API.</remarks>
	/// <param name="httpClient">The <see cref="HttpClient"/> instance used to send the request.</param>
	/// <param name="url">The URL to which the POST request is sent. Cannot be null or empty.</param>
	/// <param name="formData">A dictionary containing the form data to include in the request body. Keys represent form field names, and values
	/// represent their corresponding values. Cannot be null.</param>
	/// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A task representing the asynchronous operation. The result contains the response data, which may vary depending on
	/// the API.</returns>
	public static async Task<dynamic> PostApiAsync(this HttpClient httpClient, string url, IDictionary<string, object?> formData, CancellationToken? cancellationToken = null)
	{
		return await PostApiAsync(httpClient, url, Encode(formData), cancellationToken);
	}

	/// <summary>
	/// Sends a POST request to the specified URL with the provided form-encoded data and returns the parsed response.
	/// </summary>
	/// <remarks>This method throws an exception if the HTTP response indicates a failure or if the response content
	/// is empty. The response is expected to be in JSON format and will be parsed into a dynamic object.</remarks>
	/// <param name="httpClient">The <see cref="HttpClient"/> instance used to send the request. Cannot be <see langword="null"/>.</param>
	/// <param name="url">The URL to which the POST request is sent. Cannot be <see langword="null"/> or empty.</param>
	/// <param name="encodedFormData">The form-encoded data to include in the POST request body. Cannot be <see langword="null"/>.</param>
	/// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to observe while waiting for the request to complete. If <see
	/// langword="null"/>, the request will not be cancellable.</param>
	/// <returns>A dynamic object representing the parsed JSON response from the server.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/>, <paramref name="url"/>, or <paramref name="encodedFormData"/> is <see
	/// langword="null"/>.</exception>
	/// <exception cref="HttpRequestException">Thrown if the HTTP response indicates a failure or if the response content is empty.</exception>
	public static async Task<dynamic> PostApiAsync(this HttpClient httpClient, string url, FormUrlEncodedContent encodedFormData, CancellationToken? cancellationToken = null)
	{
		if (httpClient is null)
		{
			throw new ArgumentNullException(nameof(httpClient));
		}

		using HttpResponseMessage message = cancellationToken is CancellationToken ct
			? await httpClient.PostAsync(url, encodedFormData, ct)
			: await httpClient.PostAsync(url, encodedFormData);

		if (!message.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"Request to {url} failed with status code {message.StatusCode}");
		}

		var json = await message.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(json))
		{
			throw new HttpRequestException($"Response from {url} was empty.");
		}

		return JsonHelper.Parse(json);
	}

	/// <summary>
	/// Sends an asynchronous HTTP GET request to the specified URL with the provided query parameters.
	/// </summary>
	/// <remarks>This method constructs a GET request by encoding the provided query parameters and appending them
	/// to the specified URL. Ensure that the <paramref name="url"/> is properly formatted and accessible.</remarks>
	/// <param name="httpClient">The <see cref="HttpClient"/> instance used to send the request.</param>
	/// <param name="url">The URL to which the GET request is sent. Must be a valid, non-empty URI.</param>
	/// <param name="queryParameters">A dictionary containing query parameters to include in the request. Keys represent parameter names, and values
	/// represent parameter values. Null values are allowed and will be encoded appropriately.</param>
	/// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to cancel the operation. If not provided, the operation cannot be
	/// canceled.</param>
	/// <returns>A task representing the asynchronous operation. The result contains the dynamic response from the API.</returns>
	public static async Task<dynamic> GetApiAsync(this HttpClient httpClient, string url, IDictionary<string, object?> queryParameters, CancellationToken? cancellationToken = null)
	{
		return await GetApiAsync(httpClient, url, Encode(queryParameters), cancellationToken);
	}

	/// <summary>
	/// Sends an HTTP GET request to the specified URL with the provided query parameters and returns the parsed response.
	/// </summary>
	/// <remarks>This method constructs the full URL by appending the query parameters to the base URL. It sends an
	/// HTTP GET request and expects a JSON response. If the response is empty or the request fails, an exception is
	/// thrown.</remarks>
	/// <param name="httpClient">The <see cref="HttpClient"/> instance used to send the request. Cannot be <see langword="null"/>.</param>
	/// <param name="url">The base URL to which the GET request is sent. Must be a valid, non-empty URL string.</param>
	/// <param name="queryParameters">The query parameters to append to the URL. Cannot be <see langword="null"/>.</param>
	/// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to cancel the operation. If <see langword="null"/>, no cancellation
	/// token is used.</param>
	/// <returns>The parsed response as a dynamic object. The format of the response depends on the server's JSON output.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/> or <paramref name="queryParameters"/> is <see langword="null"/>.</exception>
	/// <exception cref="HttpRequestException">Thrown if the HTTP request fails or the response is empty.</exception>
	public static async Task<dynamic> GetApiAsync(this HttpClient httpClient, string url, FormUrlEncodedContent queryParameters, CancellationToken? cancellationToken = null)
	{
		if (httpClient is null)
		{
			throw new ArgumentNullException(nameof(httpClient));
		}

		if (queryParameters is null)
		{
			throw new ArgumentNullException(nameof(queryParameters));
		}

		var queryString = await queryParameters.ReadAsStringAsync();
		var fullUrl = $"{url}?{queryString}";

		using HttpResponseMessage message = cancellationToken is CancellationToken ct
			? await httpClient.GetAsync(fullUrl, ct)
			: await httpClient.GetAsync(fullUrl);

		if (!message.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"Request to {fullUrl} failed with status code {message.StatusCode}");
		}

		var json = await message.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(json))
		{
			throw new HttpRequestException($"Response from {fullUrl} was empty.");
		}

		return JsonHelper.Parse(json);
	}

	/// <summary>
	/// Encodes a collection of key-value pairs into a URL-encoded form content.
	/// </summary>
	/// <remarks>This method serializes each value in the <paramref name="data"/> dictionary to a JSON string 
	/// before encoding it. The resulting content can be used in HTTP requests where URL-encoded form data  is
	/// required.</remarks>
	/// <param name="data">A dictionary containing the form data to encode. The keys represent parameter names,  and the values are serialized
	/// into JSON format before encoding.</param>
	/// <returns>A <see cref="FormUrlEncodedContent"/> object containing the URL-encoded form data.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <see langword="null"/>.</exception>
	public static FormUrlEncodedContent Encode(IDictionary<string, object?> data)
	{
		if (data is null)
		{
			throw new ArgumentNullException(nameof(data));
		}

		List<KeyValuePair<string, string>> _formData = new();
		foreach (var kvp in data)
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
