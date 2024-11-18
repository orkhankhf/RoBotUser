using Entities.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
