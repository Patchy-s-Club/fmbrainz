using Discord;

namespace fmbrainz;

public abstract class EmbedResponse
{
    public static Embed GetArtistInfo(dynamic artistInfo)
    {
        var embed = new EmbedBuilder()
            .WithTitle(artistInfo.artist.name.ToString())
            .WithDescription(artistInfo.artist.bio.summary.ToString())
            .WithUrl(artistInfo.artist.url.ToString())
            .WithThumbnailUrl(artistInfo.artist.image[2]["#text"].ToString())
            .WithColor(Color.Blue)
            .Build();
        return embed;
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

    public static Embed GetListens(dynamic listens)
    {
        var embed = new EmbedBuilder()
            .WithTitle("Listens")
            .WithDescription("Listens")
            .WithColor(Color.Red)
            .Build();
        return embed;
    }
}