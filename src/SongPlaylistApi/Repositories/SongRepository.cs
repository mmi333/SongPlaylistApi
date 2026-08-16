using Microsoft.EntityFrameworkCore;
using SongPlaylistApi.Data;
using SongPlaylistApi.Models;
namespace SongPlaylistApi.Repositories;

public class SongRepository(AppDbContext db) : ISongRepository
{
    public Task<List<Song>> GetAllAsync(CancellationToken ct = default) => db.Songs.AsNoTracking().OrderBy(x => x.Id).ToListAsync(ct);
    public Task<Song?> GetByIdAsync(int id, CancellationToken ct = default) => db.Songs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<Song> CreateAsync(Song song, CancellationToken ct = default) { db.Songs.Add(song); await db.SaveChangesAsync(ct); return song; }
    public async Task<bool> UpdateAsync(Song song, CancellationToken ct = default) { var existing = await db.Songs.FindAsync([song.Id], ct); if (existing is null) return false; existing.Name = song.Name; existing.Artist = song.Artist; existing.Album = song.Album; existing.Genre = song.Genre; existing.Duration = song.Duration; existing.ReleaseDate = song.ReleaseDate; await db.SaveChangesAsync(ct); return true; }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) { var existing = await db.Songs.FindAsync([id], ct); if (existing is null) return false; db.Songs.Remove(existing); await db.SaveChangesAsync(ct); return true; }
}
