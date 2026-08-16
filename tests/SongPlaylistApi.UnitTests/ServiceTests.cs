using SongPlaylistApi.DTOs;

using SongPlaylistApi.Models;

using SongPlaylistApi.Repositories;

using SongPlaylistApi.Services;

namespace SongPlaylistApi.UnitTests;


public class ServiceTests
{
    private sealed class SongRepo : ISongRepository {
public List<Song> Items = [];
 public Task<List<Song>> GetAllAsync(CancellationToken c = default) => Task.FromResult(Items.ToList());

 public Task<Song?> GetByIdAsync(int id, CancellationToken c = default) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));

 public Task<Song> CreateAsync(Song s, CancellationToken c = default) {
s.Id = Items.Count + 1;
 Items.Add(s);

 return Task.FromResult(s);

 }
public Task<bool> UpdateAsync(Song s, CancellationToken c = default) { 
var x = Items.FirstOrDefault(y => y.Id == s.Id);
 if (x is null) return Task.FromResult(false);
 Items[Items.IndexOf(x)] = s;
 return Task.FromResult(true);
 } 

public Task<bool> DeleteAsync(int id, CancellationToken c = default) => Task.FromResult(Items.RemoveAll(x => x.Id == id) > 0);
 }
    private sealed class PlaylistRepo : IPlaylistRepository { 
public List<Playlist> Items = [];
 public List<Song> Songs = [];
 public Task<List<Playlist>> GetAllAsync(CancellationToken c = default) => Task.FromResult(Items.ToList());
 public Task<Playlist?> GetByIdAsync(int id, CancellationToken c = default) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
 public Task<Playlist> CreateAsync(Playlist p, CancellationToken c = default) { 
        p.Id = Items.Count + 1;
        Items.Add(p);
        return Task.FromResult(p);
 } 
public Task<bool> UpdateAsync(Playlist p, CancellationToken c = default) { 
        var x = Items.FirstOrDefault(y => y.Id == p.Id);
        if (x is null) return Task.FromResult(false);
        Items[Items.IndexOf(x)] = p;
        return Task.FromResult(true);

 }
public Task<bool> DeleteAsync(int id, CancellationToken c = default) => Task.FromResult(Items.RemoveAll(x => x.Id == id) > 0);

 public Task<List<Song>> GetSongsAsync(int id, CancellationToken c = default) => Task.FromResult(Songs.ToList());

 public Task<bool> AddSongAsync(int p, int s, CancellationToken c = default) => Task.FromResult(Items.Any(x => x.Id == p) && Songs.Any(x => x.Id == s));

 public Task<bool> RemoveSongAsync(int p, int s, CancellationToken c = default) => Task.FromResult(true);
 }
    [Fact] public async Task SongService_AllMethods() {
        var r = new SongRepo();
        var s = new SongService(r);
        var req = new SongRequest("N", "A", "Al", "G", TimeSpan.FromMinutes(3), new DateOnly(2020, 1, 1));
        var created = await s.CreateAsync(req);
        Assert.Equal("N", created.Name);
        Assert.Single(await s.GetAllAsync());
        Assert.NotNull(await s.GetByIdAsync(created.Id));
        Assert.True(await s.UpdateAsync(created.Id, req with { Name = "X" }));
        Assert.True(await s.DeleteAsync(created.Id));
 }
    [Fact] public async Task PlaylistService_AllMethods() { 
        var r = new PlaylistRepo();
        var s = new PlaylistService(r);
        var req = new PlaylistRequest("u", "P", DateTime.UtcNow);
        var p = await s.CreateAsync(req);
        Assert.Single(await s.GetAllAsync());
        Assert.NotNull(await s.GetByIdAsync(p.Id));
        Assert.True(await s.UpdateAsync(p.Id, req with { PlaylistName = "X" }));
        Assert.Empty(await s.GetSongsAsync(p.Id));
 }
}
