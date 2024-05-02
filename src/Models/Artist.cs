namespace fmbrainz.Models;

public class Artist(dynamic musicbrainz, dynamic spotify, dynamic lastfm)
{
    public string? Name { get; set; } = musicbrainz.name;
    public string? Type { get; set; } = musicbrainz.type;
    public string? Gender { get; set; } = musicbrainz.gender;
    public string? Area { get; set; } = musicbrainz.area.name;
    public string? BirthDate { get; } = musicbrainz["life-span"].begin;
    public string? DeathDate { get; set; } = musicbrainz["life-span"].end;

    public List<string> Tags
    {
        get
        {
            List<string> tags = new();
            foreach (var tag in musicbrainz.tags)
            {
                tags.Add(tag.name);
            }
            return tags;
        }
    }
    
    public List<string> Genres { get; set; } = spotify.genres.ToObject(typeof(List<string>));
    public string? Image { get; set; } = spotify.images[0].url;

    public List<Album> Albums { get; set; } = new();
    
    public string? Lived
    {
        get
        {
            if (BirthDate != null)
            {
                long unixTimestampBorn = new DateTimeOffset(DateTime.Parse(BirthDate)).ToUnixTimeSeconds();
                if (DeathDate != null)
                {
                    long unixTimestampDied = new DateTimeOffset(DateTime.Parse(DeathDate)).ToUnixTimeSeconds();
                    return $"<t:{unixTimestampBorn}:D> - <t:{unixTimestampDied}:D>";
                }
                else
                {
                    return $"<t:{unixTimestampBorn}:D>";
                }
            }
            return "Living";
        }
    }
    
    public long Listeners { get; set; } = lastfm.artist.stats.listeners;
    public long PlayCount { get; set; } = lastfm.artist.stats.playcount;
}