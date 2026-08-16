namespace SongPlaylistApi.DTOs;

public record PlaylistRequest(
    string UserId,
    string PlaylistName,
    DateTime CreatedDate);
