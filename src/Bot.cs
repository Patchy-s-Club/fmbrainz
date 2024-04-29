using Discord;
using Discord.Net;
using Discord.WebSocket;
using fmbrainz.Commands;
using Newtonsoft.Json;


namespace fmbrainz
{
    class Bot
    {
        private static readonly HttpClient client = new HttpClient();
        private static DiscordSocketClient _client = new();
        
        private static List<Command> commands = new List<Command>()
        {
            new GetUserCommand(),
        };
        
        public static async Task Main()
        {
            
            _client.Log += Log;
            _client.Ready += Client_Ready;
            _client.SlashCommandExecuted += SlashCommandHandler;

            // TODO: var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;
            var token = "x";


            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private static async Task Client_Ready()
        {
            try
            {
                foreach (var command in commands)
                {
                    var builder = new SlashCommandBuilder()
                        .WithName(command.Name)
                        .WithDescription(command.Description);
                    
                    foreach (var option in command.Options)
                    {
                        builder.AddOption(option);
                    }
                    await _client.Rest.CreateGlobalCommand(builder.Build());
                }
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
        private static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.CommandName)
            {
                case "getuser":
                    var user = await LastFm.GetUserInfo((string)command.Data.Options.First().Value);
                    await command.RespondAsync(embed: EmbedResponse.GetUserInfo(user));
                    break;
                case "getartist":
                    var artist = await LastFm.GetArtistInfo((string)command.Data.Options.First().Value);
                    await command.RespondAsync(embed: EmbedResponse.GetArtistInfo(artist));
                    break;
            } 
        }
    }
}