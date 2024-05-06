using System.Net.Http.Headers;

namespace fmbrainz.WebServices;

public class Spotify
{
    private static readonly string spotifyUrl = "https://api.spotify.com/v1/";

    private static readonly HttpClient Client = new()
    {
        DefaultRequestHeaders =
        {
            UserAgent = { ProductInfoHeaderValue.Parse("fmbrainz/1.0") },
        }
    };

    private static async Task<string> GetToken()
    {
        var response = await HttpService.GetResponse<dynamic>(
            "https://open.spotify.com/get_access_token?reason=transport&productType=web_player");
        return response.accessToken;
    }

    private static async Task<dynamic> Search(string query, string type, string market, int limit)
    {
        string token = await GetToken();
        var parameters = new Dictionary<string, string>
        {
            { "q",  query },
            { "type", type },
            { "market", market },
            { "limit", limit.ToString() }
        };
        var response = await HttpService.GetResponse<dynamic>(spotifyUrl+"search", token, parameters);
        return response;
    }

    public static async Task<dynamic> GetArtist(string artist)
    {
        var response = await Search(artist, "artist", "US", 1);
        return response.artists.items[0];
    }

    public static async Task<dynamic> GetAlbum(string album)
    {
        var response = await Search(album, "album", "US", 1);
        string id = response.albums.items[0].id;
        return await HttpService.GetResponse<dynamic>(spotifyUrl + $"albums/{id}", await GetToken());
    }
};