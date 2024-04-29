using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace fmbrainz
{
    internal class HttpService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<T> GetResponse<T>(string token, string url,
            Dictionary<string, string>? parameters = null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("api_key", token);

            if (parameters != null)
            {
                url = QueryHelpers.AddQueryString(url, parameters);
            }

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseContent);

            return data;
        }
    }
}