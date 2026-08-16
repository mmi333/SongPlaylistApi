using Microsoft.AspNetCore.Mvc; using SongPlaylistApi.DTOs; using SongPlaylistApi.Services;
namespace SongPlaylistApi.Controllers;
[ApiController][Route("api/playlists")]
public class PlaylistController(IPlaylistService service) : ControllerBase
{
 [HttpGet] public async Task<IActionResult> GetAll(CancellationToken ct)=>Ok(await service.GetAllAsync(ct));
 [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id,CancellationToken ct)=> (await service.GetByIdAsync(id,ct)) is { } playlist ? Ok(playlist) : NotFound();
 [HttpPost] public async Task<IActionResult> Create(PlaylistRequest request,CancellationToken ct){var p=await service.CreateAsync(request,ct);return CreatedAtAction(nameof(GetById),new{id=p.Id},p);}
 [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id,PlaylistRequest request,CancellationToken ct)=>await service.UpdateAsync(id,request,ct)?NoContent():NotFound();
 [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id,CancellationToken ct)=>await service.DeleteAsync(id,ct)?NoContent():NotFound();
 [HttpGet("{playlistId:int}/songs")] public async Task<IActionResult> GetSongs(int playlistId,CancellationToken ct)=>Ok(await service.GetSongsAsync(playlistId,ct));
 [HttpPost("{playlistId:int}/songs/{songId:int}")] public async Task<IActionResult> AddSong(int playlistId,int songId,CancellationToken ct)=>await service.AddSongAsync(playlistId,songId,ct)?NoContent():NotFound();
 [HttpDelete("{playlistId:int}/songs/{songId:int}")] public async Task<IActionResult> RemoveSong(int playlistId,int songId,CancellationToken ct)=>await service.RemoveSongAsync(playlistId,songId,ct)?NoContent():NotFound();
}
