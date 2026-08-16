using SongPlaylistApi.Models;
namespace SongPlaylistApi.Repositories;
public interface IPlaylistRepository
{
    Task<List<Playlist>> GetAllAsync(CancellationToken ct = default);
    Task<Playlist?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Playlist> CreateAsync(Playlist playlist, CancellationToken ct = default);
    Task<bool> UpdateAsync(Playlist playlist, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<List<Song>> GetSongsAsync(int playlistId, CancellationToken ct = default);
    Task<bool> AddSongAsync(int playlistId, int songId, CancellationToken ct = default);
    Task<bool> RemoveSongAsync(int playlistId, int songId, CancellationToken ct = default);
}
