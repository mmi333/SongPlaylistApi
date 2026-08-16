using System.Text.Json.Serialization;

namespace SongPlaylistApi.Models;

public class Song
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    [JsonPropertyName("duaration")]
    public TimeSpan Duration { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public ICollection<SongPlaylist> SongPlaylists { get; set; } = new List<SongPlaylist>();
}
