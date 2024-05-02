using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace fmbrainz.WebServices
{
    internal class ListenBrainz
    {
        private static string lbUrl = "https://api.listenbrainz.org";
        private static string mbUrl = "https://musicbrainz.org/ws/2";

        private static async Task<T> LookupMetadata<T>(string mediaType, string name, string artistName, string incs)
        {
            string url = "";
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

            if (!string.IsNullOrEmpty(incs))
            {
                url += $"&metadata=true&incs={Uri.EscapeDataString(incs)}";
            }

            return await HttpService.GetResponse<dynamic>(url, null);
        }

        public static async Task<string> GetMusicBrainzId(string entityType, string name, string artist)
        {
            var response = await LookupMetadata<dynamic>(entityType, name, artist, null);
            return response.releases[0].id;
        }

        public static async Task<dynamic> GetListens(string username, string? token)
        {
            var url = $"{lbUrl}/1/user/{username}/listens";
            var jsonResponse = await HttpService.GetResponse<dynamic>(url, token);
            return jsonResponse["payload"]["listens"];
        }

        public static async Task<dynamic> GetArtistInfo(string artistName, string? token)
        {
            var url = $"{mbUrl}/artist/?query=artist:{Uri.EscapeDataString(artistName)}&fmt=json";
            dynamic response = await HttpService.GetResponse<dynamic>(url, token);
            if (response.artists != null)
            {
                foreach (var artist in response.artists)
                {
                    string currentArtistName = artist.name;
                    if (currentArtistName.Equals(artistName, StringComparison.OrdinalIgnoreCase))
                    {
                        return artist;
                    }
                }
            }

            return "Artist not found.";
        }
    }
}