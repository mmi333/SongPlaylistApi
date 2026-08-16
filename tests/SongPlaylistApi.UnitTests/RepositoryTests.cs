using Microsoft.EntityFrameworkCore;

using SongPlaylistApi.Data;

using SongPlaylistApi.Models;

using SongPlaylistApi.Repositories;

namespace SongPlaylistApi.UnitTests;


public class RepositoryTests
{
    private static AppDbContext Db() => new(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact] public async Task SongRepository_GetAll() { 
        using var db = Db();
        db.Songs.Add(new Song { Name = "A", Artist = "B", Album = "C", Genre = "D", Duration = TimeSpan.FromMinutes(3), ReleaseDate = new DateOnly(2020, 1, 1) });

        await db.SaveChangesAsync();
        var r = new SongRepository(db);
        Assert.Single(await r.GetAllAsync());
 }

    [Fact] public async Task SongRepository_GetById() { 
        using var db = Db();
        var s = new Song { Name = "A", Artist = "B", Album = "C", Genre = "D", Duration = TimeSpan.FromMinutes(3), ReleaseDate = new DateOnly(2020, 1, 1) };
        db.Songs.Add(s);
        await db.SaveChangesAsync();
        Assert.Equal(s.Id, (await new SongRepository(db).GetByIdAsync(s.Id))!.Id);
 }
    [Fact] public async Task SongRepository_CreateUpdateDelete() { 
        using var db = Db();
        var r = new SongRepository(db);
        var s = await r.CreateAsync(new Song { Name = "A", Artist = "B", Album = "C", Genre = "D", Duration = TimeSpan.FromMinutes(3), ReleaseDate = new DateOnly(2020, 1, 1) });
        Assert.True(await r.UpdateAsync(new Song { Id = s.Id, Name = "X", Artist = "B", Album = "C", Genre = "D", Duration = s.Duration, ReleaseDate = s.ReleaseDate }));
        Assert.True(await r.DeleteAsync(s.Id));
        Assert.False(await r.DeleteAsync(s.Id));
 }
    [Fact] public async Task PlaylistRepository_CrudAndSongs() { 
        using var db = Db();
        var r = new PlaylistRepository(db);

        var s = new Song { Name = "A", Artist = "B", Album = "C", Genre = "D", Duration = TimeSpan.FromMinutes(3), ReleaseDate = new DateOnly(2020, 1, 1) };

        var p = new Playlist { UserId = "u", PlaylistName = "P", CreatedDate = DateTime.UtcNow };

        db.Songs.Add(s);
        db.Playlists.Add(p);
        await db.SaveChangesAsync();
        Assert.Single(await r.GetAllAsync());
        Assert.NotNull(await r.GetByIdAsync(p.Id));

        Assert.True(await r.AddSongAsync(p.Id, s.Id));

        Assert.Single(await r.GetSongsAsync(p.Id));

        Assert.True(await r.RemoveSongAsync(p.Id, s.Id));

        Assert.Empty(await r.GetSongsAsync(p.Id));
        Assert.True(await r.UpdateAsync(new Playlist { Id = p.Id, UserId = "u2", PlaylistName = "P2", CreatedDate = p.CreatedDate }));
        Assert.True(await r.DeleteAsync(p.Id));
 }
}
