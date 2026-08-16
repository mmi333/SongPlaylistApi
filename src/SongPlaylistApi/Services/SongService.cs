using SongPlaylistApi.DTOs; using SongPlaylistApi.Models; using SongPlaylistApi.Repositories;
namespace SongPlaylistApi.Services;
public class SongService(ISongRepository repo) : ISongService
{
 public Task<List<Song>> GetAllAsync(CancellationToken ct=default)=>repo.GetAllAsync(ct);
 public Task<Song?> GetByIdAsync(int id,CancellationToken ct=default)=>repo.GetByIdAsync(id,ct);
 public Task<Song> CreateAsync(SongRequest r,CancellationToken ct=default)=>repo.CreateAsync(new Song{Name=r.Name,Artist=r.Artist,Album=r.Album,Genre=r.Genre,Duration=r.Duaration,ReleaseDate=r.ReleaseDate},ct);
 public Task<bool> UpdateAsync(int id,SongRequest r,CancellationToken ct=default)=>repo.UpdateAsync(new Song{Id=id,Name=r.Name,Artist=r.Artist,Album=r.Album,Genre=r.Genre,Duration=r.Duaration,ReleaseDate=r.ReleaseDate},ct);
 public Task<bool> DeleteAsync(int id,CancellationToken ct=default)=>repo.DeleteAsync(id,ct);
}
