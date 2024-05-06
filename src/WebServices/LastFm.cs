using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace fmbrainz.WebServices
{
    internal class LastFm
    {
        private static readonly HttpClient client = new HttpClient();

        // TODO: Move to configuration file
        private static string? token = "x";
        private static string secret = "x";

        private static string baseUrl = "http://ws.audioscrobbler.com/2.0/?";

        private static async Task<dynamic> CallApi(string method, Dictionary<string, string> parameters)
        {
            parameters.Add("method", method);
            parameters.Add("api_key", token);
            parameters.Add("format", "json");

            string requestUrl = QueryHelpers.AddQueryString(baseUrl, parameters);

            Console.WriteLine(requestUrl);
            dynamic response = await HttpService.GetResponse<dynamic>(requestUrl, token);
            return response;
        }

        public static async Task<dynamic> GetArtistInfo(string artistName)
        {
            return await CallApi("artist.getinfo", new Dictionary<string, string> { { "artist", artistName } });
        }

        public static async Task<dynamic> GetUserInfo(string username)
        {
            return await CallApi("user.getinfo", new Dictionary<string, string> { { "user", username } });
        }

        public static async Task<dynamic> GetUserTracks(string username)
        {
            return await CallApi("user.getrecenttracks", new Dictionary<string, string> { { "user", username } });
        }
    }
}