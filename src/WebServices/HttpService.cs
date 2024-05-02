using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace fmbrainz.WebServices
{
    internal class HttpService
    {
        private static readonly HttpClient client = new()
        {
            DefaultRequestHeaders =
            {
                UserAgent = { ProductInfoHeaderValue.Parse("anonymous") }
            }
        };
        public static async Task<T> GetResponse<T>(string url, string? token = null,
            Dictionary<string, string>? parameters = null)
        {
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("api_key", token);
            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
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
        public static void PrintJsonElements(string jsonString)
        {
            var jsonToken = jsonString.TrimStart().StartsWith("[") ? (JToken)JArray.Parse(jsonString) : JObject.Parse(jsonString);
            PrintJsonElementsRecursive(jsonToken);
        }

        private static void PrintJsonElementsRecursive(JToken token, string prefix = "")
        {
            if (token is JValue value)
            {
                Console.WriteLine($"{prefix}: {value.Value}");
            }
            else if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    PrintJsonElementsRecursive(property.Value, $"{prefix}{(string.IsNullOrEmpty(prefix) ? "" : ".")}{property.Name}");
                }
            }
            else if (token is JArray array)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    PrintJsonElementsRecursive(array[i], $"{prefix}[{i}]");
                }
            }
        }
    }
}