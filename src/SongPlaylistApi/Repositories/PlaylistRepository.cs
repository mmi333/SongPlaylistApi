using Microsoft.EntityFrameworkCore;
using SongPlaylistApi.Data;
using SongPlaylistApi.Models;
namespace SongPlaylistApi.Repositories;
public class PlaylistRepository(AppDbContext db) : IPlaylistRepository
{
    public Task<List<Playlist>> GetAllAsync(CancellationToken ct = default) => db.Playlists.AsNoTracking().OrderBy(x=>x.Id).ToListAsync(ct);
    public Task<Playlist?> GetByIdAsync(int id, CancellationToken ct = default) => db.Playlists.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id, ct);
    public async Task<Playlist> CreateAsync(Playlist playlist, CancellationToken ct = default) { db.Playlists.Add(playlist); await db.SaveChangesAsync(ct); return playlist; }
    public async Task<bool> UpdateAsync(Playlist playlist, CancellationToken ct = default) { var existing=await db.Playlists.FindAsync([playlist.Id], ct); if(existing is null)return false; existing.UserId=playlist.UserId; existing.PlaylistName=playlist.PlaylistName; existing.CreatedDate=playlist.CreatedDate; await db.SaveChangesAsync(ct); return true; }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) { var existing=await db.Playlists.FindAsync([id], ct); if(existing is null)return false; db.Playlists.Remove(existing); await db.SaveChangesAsync(ct); return true; }
    public Task<List<Song>> GetSongsAsync(int playlistId, CancellationToken ct = default) => db.SongPlaylists.Where(x=>x.PlaylistId==playlistId).Select(x=>x.Song).AsNoTracking().OrderBy(x=>x.Id).ToListAsync(ct);
    public async Task<bool> AddSongAsync(int playlistId, int songId, CancellationToken ct = default) { if(!await db.Playlists.AnyAsync(x=>x.Id==playlistId,ct)||!await db.Songs.AnyAsync(x=>x.Id==songId,ct))return false; if(await db.SongPlaylists.AnyAsync(x=>x.PlaylistId==playlistId&&x.SongId==songId,ct))return true; db.SongPlaylists.Add(new SongPlaylist{PlaylistId=playlistId,SongId=songId}); await db.SaveChangesAsync(ct); return true; }
    public async Task<bool> RemoveSongAsync(int playlistId, int songId, CancellationToken ct = default) { var link=await db.SongPlaylists.FirstOrDefaultAsync(x=>x.PlaylistId==playlistId&&x.SongId==songId,ct); if(link is null)return false; db.SongPlaylists.Remove(link); await db.SaveChangesAsync(ct); return true; }
}
