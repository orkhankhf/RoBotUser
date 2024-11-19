using Entities.RequestModels;
using System.Text.Json;

namespace Common.Helpers
{
    public static class RequestHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string BaseUrl = AppSettings.Api.BaseUrl;

        public static async Task<ApiResponse<T>> GetAsync<T>(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl+url);

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse);

            return result;
        }
    }
}
