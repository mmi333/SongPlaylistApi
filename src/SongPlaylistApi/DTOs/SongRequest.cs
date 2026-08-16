namespace SongPlaylistApi.DTOs;

public record SongRequest(
    string Name,
    string Artist,
    string Album,
    string Genre,
    TimeSpan Duaration,
    DateOnly ReleaseDate);
