using Newtonsoft.Json;
namespace fmbrainz
{
    internal class LastFm
    {
        private static readonly HttpClient client = new HttpClient();
        
        // TODO: Move to configuration file
        private static string token = "x";
        private static string secret = "x";
        
        private static string baseUrl = "http://ws.audioscrobbler.com/2.0/?";

        public static async Task<dynamic> GetArtistInfo(string artistName)
        {
            string requestUrl = $"{baseUrl}method=artist.getinfo&artist={artistName}&api_key={token}&format=json";
            Console.WriteLine(requestUrl);

            dynamic? artistInfo = await HttpService.GetResponse<dynamic>(token, requestUrl);
            return artistInfo;
        }

        public static async Task<dynamic> GetUserInfo(string username)
        {
            string requestUrl = $"{baseUrl}method=user.getinfo&user={username}&api_key={token}&format=json";
            Console.WriteLine(requestUrl);

            dynamic userInfo = await HttpService.GetResponse<dynamic>(token, requestUrl);
            return userInfo;
        }

        public static async Task<dynamic> GetUserTracks(string username)
        {
            string requestUrl = $"{baseUrl}method=user.getrecenttracks&user={username}&api_key={token}&format=json";
            Console.WriteLine(requestUrl);

            dynamic userInfo = await HttpService.GetResponse<dynamic>(token, requestUrl);
            return userInfo;
        }


    }
}
