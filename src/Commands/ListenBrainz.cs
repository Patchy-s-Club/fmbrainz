using Discord;
using Discord.WebSocket;
using fmbrainz.Embeds;
using fmbrainz.Models;
using fmbrainz.WebServices;

namespace fmbrainz.Commands;

public class lb : Command
{
    public override string Name => "lb";
    public override string Description => "Parent lb command.";

    public override List<SlashCommandOptionBuilder> Options =>
    [
        new SlashCommandOptionBuilder()
            .WithName("getuserlistens")
            .WithDescription("Get user listens.")
            .WithType(ApplicationCommandOptionType.SubCommand)
            .AddOption("username", ApplicationCommandOptionType.String, "The username of the user.", true)
            .WithRequired(false),
    ];

    public override async Task ExecuteAsync(SocketSlashCommand command)
    {
        var subCommand = command.Data.Options.First().Name;
        switch (subCommand)
        {
            case "getuserlistens":
                var username = command.Data.Options.First().Options.First().Value.ToString();
                var user = await ListenBrainz.GetListens(username, null);
                await command.RespondAsync(embed: BrainzEmbed.GetLbListensEmbed(user));
                break;
        }
    }
}