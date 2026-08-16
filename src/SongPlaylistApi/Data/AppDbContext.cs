using Microsoft.EntityFrameworkCore;
using SongPlaylistApi.Models;

namespace SongPlaylistApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<SongPlaylist> SongPlaylists => Set<SongPlaylist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>(entity =>
        {
            entity.ToTable("songs");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(x => x.Artist).HasColumnName("artist").IsRequired().HasMaxLength(200);
            entity.Property(x => x.Album).HasColumnName("album").IsRequired().HasMaxLength(200);
            entity.Property(x => x.Genre).HasColumnName("genre").IsRequired().HasMaxLength(100);
            entity.Property(x => x.Duration).HasColumnName("duaration").IsRequired();
            entity.Property(x => x.ReleaseDate).HasColumnName("release_date").IsRequired();
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.ToTable("playlists");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(x => x.UserId).HasColumnName("user_id").IsRequired().HasMaxLength(200);
            entity.Property(x => x.PlaylistName).HasColumnName("playlist_name").IsRequired().HasMaxLength(200);
            entity.Property(x => x.CreatedDate).HasColumnName("created_date").IsRequired();
            entity.HasIndex(x => x.UserId);
        });

        modelBuilder.Entity<SongPlaylist>(entity =>
        {
            entity.ToTable("songPlaylist");
            entity.HasKey(x => new { x.SongId, x.PlaylistId });
            entity.Property(x => x.SongId).HasColumnName("song_id");
            entity.Property(x => x.PlaylistId).HasColumnName("playlist_id");
            entity.HasOne(x => x.Song).WithMany(x => x.SongPlaylists).HasForeignKey(x => x.SongId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Playlist).WithMany(x => x.SongPlaylists).HasForeignKey(x => x.PlaylistId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
