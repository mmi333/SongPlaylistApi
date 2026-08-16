using SongPlaylistApi.DTOs;
using SongPlaylistApi.Models;
namespace SongPlaylistApi.Services;
public interface ISongService
{
    Task<List<Song>> GetAllAsync(CancellationToken ct=default); Task<Song?> GetByIdAsync(int id,CancellationToken ct=default); Task<Song> CreateAsync(SongRequest request,CancellationToken ct=default); Task<bool> UpdateAsync(int id,SongRequest request,CancellationToken ct=default); Task<bool> DeleteAsync(int id,CancellationToken ct=default);
}
