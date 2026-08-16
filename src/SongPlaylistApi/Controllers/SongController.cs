using Microsoft.AspNetCore.Mvc;
using SongPlaylistApi.DTOs;
using SongPlaylistApi.Services;
namespace SongPlaylistApi.Controllers;

[ApiController]
[Route("api/songs")]
public class SongController(ISongService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await service.GetAllAsync(ct));
    [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id, CancellationToken ct) => (await service.GetByIdAsync(id, ct)) is { } song ? Ok(song) : NotFound();
    [HttpPost] public async Task<IActionResult> Create(SongRequest request, CancellationToken ct) { var song = await service.CreateAsync(request, ct); return CreatedAtAction(nameof(GetById), new { id = song.Id }, song); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, SongRequest request, CancellationToken ct) => await service.UpdateAsync(id, request, ct) ? NoContent() : NotFound();
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id, CancellationToken ct) => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
