namespace fmbrainz.Models;

public class Album()
{
    public Artist Artist { get; set; }
    public string Title { get; set; }
    public string ReleaseDate { get; set; }

    public Album(Artist artist) : this()
    {
        Artist = artist;
        
    }
}