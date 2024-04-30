using Discord;

namespace fmbrainz.Embeds;

public class BrainzEmbed
{
    public static Embed GetLbListensEmbed(dynamic listens)
    {
        var embed = new EmbedBuilder();
        return embed.WithAuthor(listens[0].user_name.ToString())
            .WithDescription(
                $"**{listens[0].track_metadata.track_name.ToString()}** by **{listens[0].track_metadata.artist_name.ToString()}**")
            .WithColor(Color.Purple)
            .Build();
    }

    public static Embed GetLbArtistEmbed(dynamic listens, string img)
    {
        var embed = new EmbedBuilder();
        var area = listens["begin-area"] == null
            ? $"**{listens.area.name}**"
            : $"**{listens["begin-area"]["name"]}, {listens.area.name}** ";
        embed.WithTitle(listens.name.ToString())
            .WithDescription("They are from " + area);
        var tags = "";
        foreach (var tag in listens.tags)
        {
            Console.WriteLine(tag.name);
            tags += tags.Length > 0 ? $", {(string)tag.name}" : (string)tag.name;
        }

        return embed.AddField("Tags", tags)
            .WithImageUrl(img)
            .WithColor(Color.Purple)
            .Build();
    }
}