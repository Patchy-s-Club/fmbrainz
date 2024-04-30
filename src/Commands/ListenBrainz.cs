using Discord;
using Discord.WebSocket;
using fmbrainz.Embeds;
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
        new SlashCommandOptionBuilder()
            .WithName("getartist")
            .WithDescription("Get artist.")
            .WithType(ApplicationCommandOptionType.SubCommand)
            .AddOption("artist", ApplicationCommandOptionType.String, "The name of the artist.", true)
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
            case "getartist":
                var artistName = command.Data.Options.First().Options.First().Value.ToString();
                var info = await ListenBrainz.GetArtistInfo(artistName, null);
                if (info.artists != null)
                {
                    foreach (var artist in info.artists)
                    {
                        string currentArtistName = artist.name; // Store the artist's name in a variable
                        if (currentArtistName.Equals(artistName, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine(artist);
                            var img = await Spotify.GetArtistImage(artist.name.ToString());
                            img = img["url"].ToString();
                            await command.RespondAsync(embed: BrainzEmbed.GetLbArtistEmbed(artist, img));
                            break;
                        }
                    }
                }

                break;
        }
    }
}