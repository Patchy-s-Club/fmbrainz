using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;


namespace fmbrainz
{
    class Bot
    {
        private static readonly HttpClient client = new HttpClient();

        private static DiscordSocketClient _client = new();

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
            var a = new SlashCommandBuilder()
                .WithName("getuser")
                .WithDescription("Get user.")
                .AddOption("user", ApplicationCommandOptionType.String, "The user", isRequired: true)
                .Build();

            var b = new SlashCommandBuilder()
                .WithName("getartist")
                .WithDescription("Get artist.")
                .Build();

            try
            {
                await _client.CreateGlobalApplicationCommandAsync(a);
                await _client.CreateGlobalApplicationCommandAsync(b);

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
                    await command.RespondAsync(user.user.name);
                    break;
            } 

        }
    }
}