using Discord;

namespace fmbrainz;

public abstract class EmbedResponse
{
    public static Embed GetArtistInfo(dynamic artistInfo)
    {
        string tags = "";
        foreach (var tag in artistInfo.artist.tags.tag)
        {
            tags += tag.name.ToString() + ", ";
        }

        return new EmbedBuilder()
            .WithTitle(artistInfo.artist.name.ToString())
            .AddField("Listeners", artistInfo.artist.stats.listeners.ToString(), false)
            .AddField("Play Count", artistInfo.artist.stats.playcount.ToString(), false)
            .AddField("Tags", tags, false)
            .WithUrl(artistInfo.artist.url.ToString())
            .WithThumbnailUrl(artistInfo.artist.image[2]["#text"].ToString())
            .WithColor(Color.Blue)
            .Build();
    }

    public static Embed GetUserInfo(dynamic userInfo)
    {
        var embed = new EmbedBuilder()
            .WithTitle(userInfo.user.name.ToString())
            .WithDescription(
                $"{userInfo.user.playcount.ToString()} plays\n" +
                $"{userInfo.user.track_count.ToString()} unique tracks\n" +
                $"{userInfo.user.artist_count.ToString()} artists\n" +
                $"{userInfo.user.album_count.ToString()} albums"
            )
            .WithUrl(userInfo.user.url.ToString())
            .WithThumbnailUrl(userInfo.user.image[2]["#text"].ToString())
            .WithColor(Color.Red)
            .Build();
        return embed;
    }

    public static Embed GetListens(dynamic tracks)
    {
        var embed = new EmbedBuilder();
        embed = tracks.recenttracks.track[0]["@attr"].nowplaying == "true"
            ? embed.WithAuthor("Currently listening to")
            : embed.WithAuthor("Last listened to");

        embed.WithDescription($"**{tracks.recenttracks.track[0].artist["#text"].ToString()}** on " +
                              $"**{tracks.recenttracks.track[0].album["#text"].ToString()}**")
            .WithTitle(tracks.recenttracks.track[0].name.ToString())
            .WithThumbnailUrl(tracks.recenttracks.track[0].image[2]["#text"].ToString())
            .WithColor(Color.Red);

        return embed.Build();
    }
}