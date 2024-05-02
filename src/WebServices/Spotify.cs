using System.Net.Http.Headers;

namespace fmbrainz.WebServices;

public class Spotify
{
    private static readonly string spotifyUrl = "https://api.spotify.com/v1/search";

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

    public static async Task<dynamic> GetArtist(string artist)
    {
        string token = await GetToken();
        var parameters = new Dictionary<string, string>
        {
            { "q", "artist:" + artist },
            { "type", "artist" },
            { "market", "US" },
            { "limit", "1" }
        };
        var response = await HttpService.GetResponse<dynamic>(spotifyUrl, token, parameters);
        Console.WriteLine(response);
        return response.artists.items[0];
    }

    public static async Task<dynamic> GetArtistImage(string artist)
    {
        return (await GetArtist(artist)).images[0];
    }
    
    public static async Task<string> GetArtistGenre(string artist)
    {
        var artistData = await Spotify.GetArtist(artist);
        var genres = artistData.genres; // Assuming genres is the correct property name
        string genresString = string.Join(", ", genres);
        Console.WriteLine(genresString);
        return genresString;
    }
};