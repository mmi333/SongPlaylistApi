# Song Playlist API

Simple ASP.NET Core 10 Web API using MVC-style controllers, services, repositories, EF Core 10 and PostgreSQL. .NET 10 and EF Core 10 are LTS releases; EF Core 10 requires the .NET 10 SDK/runtime. Npgsql 10 provides the PostgreSQL EF Core provider.

## Architecture

- `Controllers`: HTTP/MVC layer.
- `Services`: application/business layer.
- `Repositories`: data-access layer.
- `Data/AppDbContext`: EF Core model and PostgreSQL mappings.
- `Data/DatabaseInitializer`: creates the database when missing, creates tables, and seeds demo data.
- `Database/schema.sql` and `Database/seed.sql`: equivalent SQL for manual database setup.

No authentication or authorization is implemented. `userId` is an ordinary client-supplied string.

## Schema

### songs
`id` PK, `name`, `artist`, `album`, `genre`, `duration` (PostgreSQL interval), `release_date`.

### playlists
`id` PK, `user_id`, `playlist_name`, `created_date`.

### songPlaylist
Composite PK `(song_id, playlist_id)`, with foreign keys to `songs(id)` and `playlists(id)`. Deletes cascade from either parent. 

## Endpoints

### Songs

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/songs` | List songs |
| GET | `/api/songs/{id}` | Get a song |
| POST | `/api/songs` | Create a song |
| PUT | `/api/songs/{id}` | Update a song |
| DELETE | `/api/songs/{id}` | Delete a song |

Song JSON:
```json
{"name":"Numb","artist":"Linkin Park","album":"Meteora","genre":"Rock","duration":"00:03:05","releaseDate":"2003-03-25"}
```

### Playlists

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/playlists` | List playlists |
| GET | `/api/playlists/{id}` | Get a playlist |
| POST | `/api/playlists` | Create a playlist |
| PUT | `/api/playlists/{id}` | Update a playlist |
| DELETE | `/api/playlists/{id}` | Delete a playlist |
| GET | `/api/playlists/{playlistId}/songs` | List songs in playlist |
| POST | `/api/playlists/{playlistId}/songs/{songId}` | Add song to playlist |
| DELETE | `/api/playlists/{playlistId}/songs/{songId}` | Remove song from playlist |

Playlist JSON:
```json
{"userId":"user-1","playlistName":"Favorites","createdDate":"2026-08-16T10:00:00Z"}
```

## PostgreSQL with Docker

The included compose file starts PostgreSQL on host port 5432:

```bash
docker compose up -d
```

The API connects to `localhost:5432`, database `song_playlist_db`, user `postgres`, password `postgres`. On first startup it connects to PostgreSQL's `postgres` maintenance database, creates `song_playlist_db` if it does not exist, creates all tables with EF Core, and inserts example data.

If you prefer SQL manually:

```bash
psql "host=localhost port=5432 dbname=postgres user=postgres password=postgres" -c 'CREATE DATABASE song_playlist_db;'
psql "host=localhost port=5432 dbname=song_playlist_db user=postgres password=postgres" -f Database/schema.sql
psql "host=localhost port=5432 dbname=song_playlist_db user=postgres password=postgres" -f Database/seed.sql
```

## Run on Linux

Prerequisites: .NET 10 SDK and Docker. .NET 10 is supported through November 2028.

```bash
cd SongPlaylistApi
docker compose up -d
dotnet restore
dotnet run --project src/SongPlaylistApi --urls http://localhost:5000
```

The console prints the listening URL. With the usual ASP.NET development profile it will be similar to `http://localhost:5000`/`https://localhost:7000`; use the actual URL printed by `dotnet run`.

Swagger is available at `/swagger`.

## curl examples

Set the base URL to the URL printed by `dotnet run`:

```bash
BASE=http://localhost:5000

curl "$BASE/api/songs"
curl "$BASE/api/playlists"

curl -X POST "$BASE/api/songs" -H 'Content-Type: application/json' -d '{"name":"Everlong","artist":"Foo Fighters","album":"The Colour and the Shape","genre":"Rock","duration":"00:04:10","releaseDate":"1997-05-20"}'

curl -X POST "$BASE/api/playlists" -H 'Content-Type: application/json' -d '{"userId":"user-123","playlistName":"Workout","createdDate":"2026-08-16T10:00:00Z"}'

curl -X POST "$BASE/api/playlists/1/songs/1"
curl "$BASE/api/playlists/1/songs"
curl -X DELETE "$BASE/api/playlists/1/songs/1"

curl -X PUT "$BASE/api/songs/1" -H 'Content-Type: application/json' -d '{"name":"Numb (Updated)","artist":"Linkin Park","album":"Meteora","genre":"Rock","duration":"00:03:05","releaseDate":"2003-03-25"}'

curl -X DELETE "$BASE/api/songs/1"
curl -X DELETE "$BASE/api/playlists/1"
```

## Tests

Unit tests cover repository, service, and controller methods. Integration tests run the real ASP.NET pipeline against an isolated PostgreSQL Docker container using Testcontainers. Testcontainers' PostgreSQL module is designed for disposable PostgreSQL containers and supports .NET 10.

Run all tests:

```bash
dotnet test
```

Unit tests only:

```bash
dotnet test tests/SongPlaylistApi.UnitTests
```

Integration tests only (Docker must be running):

```bash
dotnet test tests/SongPlaylistApi.IntegrationTests
```

## Notes

This is deliberately simple: no authentication, authorization, pagination, validation framework, or external user table is included. For a production system, add migrations instead of `EnsureCreated`, request validation, structured error handling, authentication/authorization, and optimistic concurrency.
