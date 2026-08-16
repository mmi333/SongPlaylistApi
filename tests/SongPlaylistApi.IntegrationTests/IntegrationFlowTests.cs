using System.Net; using System.Net.Http.Json; using Microsoft.AspNetCore.Hosting; using Microsoft.AspNetCore.Mvc.Testing; using Microsoft.Extensions.Configuration; using Testcontainers.PostgreSql; using SongPlaylistApi.DTOs; using SongPlaylistApi.Models;
namespace SongPlaylistApi.IntegrationTests;
public sealed class IntegrationFlowTests : IAsyncLifetime
{
 private readonly PostgreSqlContainer _db = new PostgreSqlBuilder("postgres:17").WithDatabase("song_playlist_test").WithUsername("postgres").WithPassword("postgres").Build();
 private WebApplicationFactory<Program>? _factory; private HttpClient? _client;
 public async Task InitializeAsync(){await _db.StartAsync();_factory=new WebApplicationFactory<Program>().WithWebHostBuilder(b=>b.ConfigureAppConfiguration((_,c)=>c.AddInMemoryCollection(new Dictionary<string,string?>{{"ConnectionStrings:DefaultConnection",_db.GetConnectionString()}})));_client=_factory.CreateClient();}
 public async Task DisposeAsync(){_client?.Dispose();_factory?.Dispose();await _db.DisposeAsync();}
 [Fact] public async Task WholeCrudAndPlaylistFlow(){
  var songReq=new SongRequest("Integration Song","Tester","Integration Album","Test",TimeSpan.FromSeconds(210),new DateOnly(2026,1,1));
  var songResponse=await _client!.PostAsJsonAsync("/api/songs",songReq);Assert.Equal(HttpStatusCode.Created,songResponse.StatusCode);var song=await songResponse.Content.ReadFromJsonAsync<Song>();Assert.NotNull(song);
  var playlistReq=new PlaylistRequest("integration-user","Integration Playlist",DateTime.UtcNow);var pResponse=await _client.PostAsJsonAsync("/api/playlists",playlistReq);Assert.Equal(HttpStatusCode.Created,pResponse.StatusCode);var playlist=await pResponse.Content.ReadFromJsonAsync<Playlist>();Assert.NotNull(playlist);
  Assert.Equal(HttpStatusCode.NoContent,(await _client.PostAsync($"/api/playlists/{playlist!.Id}/songs/{song!.Id}",null)).StatusCode);
  var songs=await _client.GetFromJsonAsync<List<Song>>($"/api/playlists/{playlist.Id}/songs");Assert.Single(songs!);
  songReq=songReq with {Name="Updated Song"};Assert.Equal(HttpStatusCode.NoContent,(await _client.PutAsJsonAsync($"/api/songs/{song.Id}",songReq)).StatusCode);
  Assert.Equal(HttpStatusCode.NoContent,(await _client.DeleteAsync($"/api/playlists/{playlist.Id}/songs/{song.Id}")).StatusCode);
  Assert.Empty(await _client.GetFromJsonAsync<List<Song>>($"/api/playlists/{playlist.Id}/songs")!);
  Assert.Equal(HttpStatusCode.NoContent,(await _client.DeleteAsync($"/api/songs/{song.Id}")).StatusCode);Assert.Equal(HttpStatusCode.NoContent,(await _client.DeleteAsync($"/api/playlists/{playlist.Id}")).StatusCode);
 }
}
