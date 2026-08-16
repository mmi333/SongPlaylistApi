using Microsoft.EntityFrameworkCore;
using Npgsql;
using SongPlaylistApi.Models;

namespace SongPlaylistApi.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is missing.");

        await CreateDatabaseIfMissingAsync(connectionString);

        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();
        await SeedAsync(db);
    }

    private static async Task CreateDatabaseIfMissingAsync(string connectionString)
    {
        var target = new NpgsqlConnectionStringBuilder(connectionString);
        var databaseName = target.Database;
        if (string.IsNullOrWhiteSpace(databaseName)) throw new InvalidOperationException("Database name is required.");

        target.Database = "postgres";
        await using var connection = new NpgsqlConnection(target.ConnectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1 FROM pg_database WHERE datname = @name";
        command.Parameters.AddWithValue("name", databaseName);
        var exists = await command.ExecuteScalarAsync() is not null;
        if (!exists)
        {
            await using var create = connection.CreateCommand();
            create.CommandText = $"CREATE DATABASE {QuoteIdentifier(databaseName)}";
            await create.ExecuteNonQueryAsync();
        }
    }

    private static string QuoteIdentifier(string value) => """ + value.Replace(""", """") + """;

    private static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Songs.AnyAsync() || await db.Playlists.AnyAsync()) return;

        var songs = new[]
        {
            new Song { Name="Numb", Artist="Linkin Park", Album="Meteora", Genre="Rock", Duration=new TimeSpan(0,3,05), ReleaseDate=new DateOnly(2003,3,25) },
            new Song { Name="Billie Jean", Artist="Michael Jackson", Album="Thriller", Genre="Pop", Duration=new TimeSpan(0,4,54), ReleaseDate=new DateOnly(1982,11,30) },
            new Song { Name="Lose Yourself", Artist="Eminem", Album="8 Mile", Genre="Hip Hop", Duration=new TimeSpan(0,5,26), ReleaseDate=new DateOnly(2002,10,28) }
        };
        db.Songs.AddRange(songs);
        var playlist = new Playlist { UserId="demo-user", PlaylistName="Demo Favorites", CreatedDate=DateTime.UtcNow };
        db.Playlists.Add(playlist);
        await db.SaveChangesAsync();
        db.SongPlaylists.AddRange(songs.Take(2).Select(s => new SongPlaylist { SongId=s.Id, PlaylistId=playlist.Id }));
        await db.SaveChangesAsync();
    }
}
