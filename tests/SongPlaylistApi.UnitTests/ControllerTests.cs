using Microsoft.AspNetCore.Mvc;

using SongPlaylistApi.Controllers;

using SongPlaylistApi.DTOs;

using SongPlaylistApi.Models;

using SongPlaylistApi.Services;

namespace SongPlaylistApi.UnitTests;


public class ControllerTests
{
    private sealed class Songs : ISongService { public Task<List<Song>> GetAllAsync(CancellationToken c = default) => Task.FromResult(new List<Song>());

public Task<Song?> GetByIdAsync(int id, CancellationToken c = default) => Task.FromResult<Song?>(id == 1 ? new Song { Id = 1 } : null);
 
public Task<Song> CreateAsync(SongRequest r, CancellationToken c = default) => Task.FromResult(new Song { Id = 1 });

public Task<bool> UpdateAsync(int id, SongRequest r, CancellationToken c = default) => Task.FromResult(id == 1);

public Task<bool> DeleteAsync(int id, CancellationToken c = default) => Task.FromResult(id == 1);

}
    private sealed class Playlists : IPlaylistService { public Task<List<Playlist>> GetAllAsync(CancellationToken c = default) => Task.FromResult(new List<Playlist>());

public Task<Playlist?> GetByIdAsync(int id, CancellationToken c = default) => Task.FromResult<Playlist?>(id == 1 ? new Playlist { Id = 1 } : null);

public Task<Playlist> CreateAsync(PlaylistRequest r, CancellationToken c = default) => Task.FromResult(new Playlist { Id = 1 });

public Task<bool> UpdateAsync(int id, PlaylistRequest r, CancellationToken c = default) => Task.FromResult(id == 1);

public Task<bool> DeleteAsync(int id, CancellationToken c = default) => Task.FromResult(id == 1);

public Task<List<Song>> GetSongsAsync(int id, CancellationToken c = default) => Task.FromResult(new List<Song>());

public Task<bool> AddSongAsync(int p, int s, CancellationToken c = default) => Task.FromResult(p == 1 && s == 1);
 public Task<bool> RemoveSongAsync(int p, int s, CancellationToken c = default) => Task.FromResult(p == 1 && s == 1);
 }
    [Fact] public async Task SongController_AllActions() { 
        var c = new SongController(new Songs());


        Assert.IsType<OkObjectResult>(await c.GetAll(default));

        Assert.IsType<OkObjectResult>(await c.GetById(1, default));

        Assert.IsType<NotFoundResult>(await c.GetById(2, default));
        Assert.IsType<CreatedAtActionResult>(await c.Create(new SongRequest("N", "A", "Al", "G", TimeSpan.FromMinutes(3), new DateOnly(2020, 1, 1)), default));
        Assert.IsType<NoContentResult>(await c.Update(1, new SongRequest("N", "A", "Al", "G", TimeSpan.FromMinutes(3), new DateOnly(2020, 1, 1)), default));
        Assert.IsType<NoContentResult>(await c.Delete(1, default));
 }
    [Fact] public async Task PlaylistController_AllActions() { 
        var c = new PlaylistController(new Playlists());
        var req = new PlaylistRequest("u", "P", DateTime.UtcNow);
        Assert.IsType<OkObjectResult>(await c.GetAll(default));
        Assert.IsType<OkObjectResult>(await c.GetById(1, default));
        Assert.IsType<NotFoundResult>(await c.GetById(2, default));
        Assert.IsType<CreatedAtActionResult>(await c.Create(req, default));
        Assert.IsType<NoContentResult>(await c.Update(1, req, default));
        Assert.IsType<NoContentResult>(await c.Delete(1, default));
        Assert.IsType<OkObjectResult>(await c.GetSongs(1, default));
        Assert.IsType<NoContentResult>(await c.AddSong(1, 1, default));
        Assert.IsType<NoContentResult>(await c.RemoveSong(1, 1, default));
 }
}
