using Discord;
using Discord.WebSocket;
using fmbrainz.Embeds;
using fmbrainz.Models;
using fmbrainz.WebServices;

namespace fmbrainz.Commands;

public class GetArtist : Command
{
    public override string Name => "getartist";
    public override string Description => "Get artist.";

    public override List<SlashCommandOptionBuilder> Options =>
    [
        new SlashCommandOptionBuilder()
            .WithName("artist")
            .WithDescription("The name of the artist.")
            .WithType(ApplicationCommandOptionType.String)
            .WithRequired(true)
    ];
    public override async Task ExecuteAsync(SocketSlashCommand command)
    {
        var artistName = command.Data.Options.First().Value.ToString();
        var info = await ListenBrainz.GetArtistInfo(artistName, null);
        var spotify = await Spotify.GetArtist(artistName);
        var lastfm = await LastFm.GetArtistInfo(artistName);
        Console.WriteLine(lastfm);
        Artist artist = new Artist(info, spotify, lastfm);
        await command.RespondAsync(embed: BrainzEmbed.GetArtistEmbed(artist));
    }
}