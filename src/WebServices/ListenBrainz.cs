﻿using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace fmbrainz.WebServices
{
    internal class ListenBrainz
    {
        private static string lbUrl = "https://api.listenbrainz.org";
        private static string mbUrl = "https://musicbrainz.org/ws/2";
        private static readonly HttpClient client = new()
        {
            DefaultRequestHeaders =
            {
                UserAgent = { ProductInfoHeaderValue.Parse("fmbrainz/1.0") }
            }
        };

        private static async Task<T> LookupMetadata<T>(string mediaType, string name, string artistName, string incs)
        {
            string url ="";
            switch (mediaType)
            {
                case "track":
                    url =
                        $"{lbUrl}/1/metadata/lookup/?recording_name={Uri.EscapeDataString(name)}&artist_name={Uri.EscapeDataString(artistName)}";
                    break;
                case "album":
                    url = 
                        $"{mbUrl}/release/?query=release:{Uri.EscapeDataString(name)}%20AND%20artist:{Uri.EscapeDataString(artistName)}&fmt=json";
                    break;
                default:
                    throw new Exception("Invalid media type.");
            }

            Console.WriteLine(url);
            if (!string.IsNullOrEmpty(incs))
            {
                url += $"&metadata=true&incs={Uri.EscapeDataString(incs)}";
            }

            var response = await client.GetAsync(url);
            //response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseContent);

            return data;
        }
        public static async Task<string> GetMusicBrainzId(string entityType, string name, string artist)
        {
            var response = await LookupMetadata<dynamic>(entityType, name, artist, null);
            return response.releases[0].id;
        }
        public static async Task<dynamic> GetListens(string username, string token,
            long? minTs = null, long? maxTs = null, int? count = null)
        {
            var url = $"{lbUrl}/1/user/{username}/listens";

            var jsonResponse = await HttpService.GetResponse<dynamic>(token, url);

            return jsonResponse["payload"]["listens"];
        }
    }
}