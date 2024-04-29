using Discord;
using Discord.WebSocket;

namespace fmbrainz.Commands;

public class GetUserCommand : Command
{
    public override string Name => "getuser";
    public override string Description => "Get user.";

    public override List<SlashCommandOptionBuilder> Options =>
    [
        new SlashCommandOptionBuilder()
            .WithName("username")
            .WithDescription("The username of the user.")
            .WithType(ApplicationCommandOptionType.String)
            .WithRequired(true)
    ];

    public override async Task ExecuteAsync(SocketSlashCommand command)
    {
        var user = await LastFm.GetUserInfo((string)command.Data.Options.First().Value);
        await command.RespondAsync(embed: EmbedResponse.GetUserInfo(user));
    }
}

public class GetArtistCommand : Command
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
        var artist = await LastFm.GetArtistInfo((string)command.Data.Options.First().Value);
        await command.RespondAsync(embed: EmbedResponse.GetArtistInfo(artist));
    }
}

public class GetUserTracks : Command
{
    public override string Name => "gettracks";
    public override string Description => "Get user tracks.";
    public override List<SlashCommandOptionBuilder> Options =>
    [
        new SlashCommandOptionBuilder()
            .WithName("username")
            .WithDescription("The username of the user.")
            .WithType(ApplicationCommandOptionType.String)
            .WithRequired(true)
    ];
    public override async Task ExecuteAsync(SocketSlashCommand command)
    {
        var user = await LastFm.GetUserTracks((string)command.Data.Options.First().Value);
        await command.RespondAsync(embed: EmbedResponse.GetListens(user));
    }
}