using System.Net.Http.Headers;

namespace fmbrainz.WebServices;

public class Spotify
{
    private static readonly string? token =
        "x";
    private static readonly string spotifyUrl = "https://api.spotify.com/v1/search";

    private readonly HttpClient client = new()
    {
        DefaultRequestHeaders =
        {
            UserAgent = { ProductInfoHeaderValue.Parse("fmbrainz/1.0") }
        }
    };
    public Spotify()
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static async Task<dynamic> GetArtistImage(string artist)
    {
        var parameters = new Dictionary<string, string>
        {
            { "q", "artist:" + artist },
            { "type", "artist" },
            { "market", "US" },
            { "limit", "1" }
        };
        var response = await HttpService.GetResponse<dynamic>(token, spotifyUrl, parameters);
        return response.artists.items[0].images[0];
    }
}