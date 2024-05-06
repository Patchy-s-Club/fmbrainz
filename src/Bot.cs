using Discord;
using Discord.Net;
using Discord.WebSocket;
using fmbrainz.Commands;
using Newtonsoft.Json;


namespace fmbrainz
{
    static class Bot
    {
        private static readonly HttpClient client = new HttpClient();
        private static DiscordSocketClient _client = new();

        private static List<Command> commands = new List<Command>()
        {
            new lfm(),
            new lb(),
            new GetArtist()
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
                    Console.WriteLine($"Command {command.Name} has been registered.");
                }
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            }
        }

        private static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            var commandName = command.Data.Name;
            var commandToExecute = commands.FirstOrDefault(c => c.Name == commandName);

            if (commandToExecute != null)
            {
                await commandToExecute.ExecuteAsync(command);
            }
            else
            {
                await command.RespondAsync("Unknown command.");
            }
        }    }
}