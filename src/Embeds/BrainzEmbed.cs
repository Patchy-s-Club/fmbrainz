using Discord;
using fmbrainz.Models;

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

    public static Embed GetArtistEmbed(Artist artist)
    {
        var embed = new EmbedBuilder()
            .WithTitle(artist.Name)
            .WithDescription($"**{artist.Name}** is from {(artist.Area != null ? artist.Area : "somewhere on Earth..")}")
            .AddField("Type", artist.Type, true)
            .WithImageUrl(artist.Image)
            .WithColor(Color.Purple);

        if (artist.Type == "Person")
        {
            if (artist.Gender != null)
            {
                embed.AddField("Gender", artist.Gender, true);
            }

            if (artist.Lived != null)
            {
                embed.AddField("Lived", artist.Lived, true);
            }
        }
        embed.AddField("Genres", artist.Genres.Count > 0 ? string.Join(", ", artist.Genres) : "No tags", false);
        embed.AddField("Listeners", artist.Listeners.ToString(), true);
        embed.AddField("Play Count", artist.PlayCount.ToString(), true);
        return embed.Build();
    }
    
}