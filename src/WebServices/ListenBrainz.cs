namespace fmbrainz
{
    internal class ListenBrainz
    {
        private static string baseUrl = "https://api.listenbrainz.org";

        public static async Task<dynamic> GetListens(string username, string token,
            long? minTs = null, long? maxTs = null, int? count = null)
        {
            var url = $"{baseUrl}/1/user/{username}/listens";
            var parameters = new Dictionary<string, string>();
            if (minTs.HasValue) parameters.Add("min_ts", minTs.Value.ToString());
            if (maxTs.HasValue) parameters.Add("max_ts", maxTs.Value.ToString());
            if (count.HasValue) parameters.Add("count", count.Value.ToString());

            var jsonResponse = await HttpService.GetResponse<dynamic>(token, url, parameters);

            return jsonResponse["payload"]["listens"];
        }
    }
}