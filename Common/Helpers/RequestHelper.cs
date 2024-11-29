using Entities.RequestModels;
using System.Text;
using System.Text.Json;

namespace Common.Helpers
{
    public static class RequestHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string BaseUrl = AppSettings.Api.BaseUrl;

        // Helper method to add headers
        private static void AddUserTokenHeader(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(UserSettings.Token))
            {
                request.Headers.Add("userToken", UserSettings.Token);
            }
        }

        //GET method
        public static async Task<ApiResponse<T>> GetAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + url);

            AddUserTokenHeader(request); // Add the user token header

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse);

            return result;
        }

        public static async Task<ApiResponse<T>> GetFilteredAsync<T>(string url, object queryParams)
        {
            if (queryParams != null)
            {
                var queryParamsDictionary = queryParams.GetType()
                    .GetProperties()
                    .Where(prop => prop.GetValue(queryParams, null) != null) // Exclude null properties
                    .SelectMany(prop =>
                    {
                        var value = prop.GetValue(queryParams, null);

                        if (value is IEnumerable<int> collection) // Handle lists of integers (e.g., Cities, Categories)
                        {
                            return collection.Select(item => new KeyValuePair<string, string>(prop.Name, item.ToString()));
                        }

                        return new[] { new KeyValuePair<string, string>(prop.Name, value.ToString()) }; // Handle single values
                    });

                var query = string.Join("&", queryParamsDictionary.Select(kv =>
                    $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

                url += $"?{query}";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + url);
            AddUserTokenHeader(request);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error calling {url}: {response.StatusCode} {response.ReasonPhrase}");
            }

            var result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse);
            return result;
        }

        //POST method
        public static async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + url)
            {
                Content = content
            };

            AddUserTokenHeader(request); // Add the user token header

            HttpResponseMessage response = await _httpClient.SendAsync(request);


            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<TResponse>
                {
                    Success = false,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode
                };
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<TResponse>>(jsonResponse);

            return result;
        }

        // PUT method
        public static async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, BaseUrl + url)
            {
                Content = content
            };

            AddUserTokenHeader(request); // Add the user token header

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<TResponse>
                {
                    Success = false,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode
                };
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<TResponse>>(jsonResponse);

            return result;
        }

        // DELETE method
        public static async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, BaseUrl + url);
            AddUserTokenHeader(request); // Add the user token header

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<TResponse>
                {
                    Success = false,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode
                };
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<TResponse>>(jsonResponse);

            return result;
        }
    }
}