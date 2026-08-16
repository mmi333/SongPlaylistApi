namespace SongPlaylistApi.Models;

public class Playlist
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string PlaylistName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public ICollection<SongPlaylist> SongPlaylists { get; set; } = new List<SongPlaylist>();
}
