using Discord;
using Discord.WebSocket;

namespace fmbrainz.Commands;

public abstract class Command
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract List<SlashCommandOptionBuilder> Options { get; }
    public abstract Task ExecuteAsync(SocketSlashCommand message);
    
}