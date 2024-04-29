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