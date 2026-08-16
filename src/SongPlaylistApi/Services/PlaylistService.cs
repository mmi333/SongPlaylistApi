using SongPlaylistApi.DTOs; using SongPlaylistApi.Models; using SongPlaylistApi.Repositories;
namespace SongPlaylistApi.Services;
public class PlaylistService(IPlaylistRepository repo) : IPlaylistService
{
 public Task<List<Playlist>> GetAllAsync(CancellationToken ct=default)=>repo.GetAllAsync(ct);
 public Task<Playlist?> GetByIdAsync(int id,CancellationToken ct=default)=>repo.GetByIdAsync(id,ct);
 public Task<Playlist> CreateAsync(PlaylistRequest r,CancellationToken ct=default)=>repo.CreateAsync(new Playlist{UserId=r.UserId,PlaylistName=r.PlaylistName,CreatedDate=r.CreatedDate},ct);
 public Task<bool> UpdateAsync(int id,PlaylistRequest r,CancellationToken ct=default)=>repo.UpdateAsync(new Playlist{Id=id,UserId=r.UserId,PlaylistName=r.PlaylistName,CreatedDate=r.CreatedDate},ct);
 public Task<bool> DeleteAsync(int id,CancellationToken ct=default)=>repo.DeleteAsync(id,ct);
 public Task<List<Song>> GetSongsAsync(int playlistId,CancellationToken ct=default)=>repo.GetSongsAsync(playlistId,ct);
 public Task<bool> AddSongAsync(int playlistId,int songId,CancellationToken ct=default)=>repo.AddSongAsync(playlistId,songId,ct);
 public Task<bool> RemoveSongAsync(int playlistId,int songId,CancellationToken ct=default)=>repo.RemoveSongAsync(playlistId,songId,ct);
}
