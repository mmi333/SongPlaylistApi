using SongPlaylistApi.Models;
namespace SongPlaylistApi.Repositories;
public interface ISongRepository
{
    Task<List<Song>> GetAllAsync(CancellationToken ct = default);
    Task<Song?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Song> CreateAsync(Song song, CancellationToken ct = default);
    Task<bool> UpdateAsync(Song song, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
