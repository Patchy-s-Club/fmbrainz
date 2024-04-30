using Discord;
using Discord.WebSocket;
using fmbrainz.Embeds;
using fmbrainz.WebServices;

namespace fmbrainz.Commands;

public class lfm : Command
{
    public override string Name => "lfm";
    public override string Description => "Parent lfm group.";

    public override List<SlashCommandOptionBuilder> Options =>
    [
        new SlashCommandOptionBuilder()
            .WithName("getuser")
            .WithDescription("Get user.")
            .WithType(ApplicationCommandOptionType.SubCommand)
            .AddOption("username", ApplicationCommandOptionType.String, "The username of the user.", true)
            .WithRequired(false),
        new SlashCommandOptionBuilder()
            .WithName("getartist")
            .WithDescription("Get artist.")
            .WithType(ApplicationCommandOptionType.SubCommand)
            .AddOption("artist", ApplicationCommandOptionType.String, "The name of the artist.", true)
            .WithRequired(false),
        new SlashCommandOptionBuilder()
            .WithName("gettracks")
            .WithDescription("Get user tracks.")
            .AddOption("username", ApplicationCommandOptionType.String, "The username of the user.", true)
            .WithType(ApplicationCommandOptionType.SubCommand)
            .WithRequired(false)
    ];
    public override async Task ExecuteAsync(SocketSlashCommand command)
    {
        var subCommand = command.Data.Options.First().Name;
        switch (subCommand)
        {
            case "getuser":
                var username = command.Data.Options.First().Options.First().Value.ToString();
                var user = await LastFm.GetUserInfo(username);
                await command.RespondAsync(embed: FmEmbed.GetUserInfoEmbed(user));
                break;
            case "getartist":
                var artistName = command.Data.Options.First().Options.First().Value.ToString();
                var artist = await LastFm.GetArtistInfo(artistName);
                await command.RespondAsync(embed: FmEmbed.GetArtistInfoEmbed(artist));
                break;
            case "gettracks":
                var usernameTracks = command.Data.Options.First().Options.First().Value.ToString();
                var userTracks = await LastFm.GetUserTracks(usernameTracks);
                await command.RespondAsync(embed: FmEmbed.GetListensEmbed(userTracks));
                break;
        }

    }
}