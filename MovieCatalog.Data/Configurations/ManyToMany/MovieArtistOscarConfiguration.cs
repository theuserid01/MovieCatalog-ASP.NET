namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieArtistOscarConfiguration : IEntityTypeConfiguration<MovieArtistOscar>
    {
        public void Configure(EntityTypeBuilder<MovieArtistOscar> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.ArtistId, e.OscarId, e.Role });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.ArtistsOscars)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Artist)
                .WithMany(e => e.MoviesOscars)
                .HasForeignKey(e => e.ArtistId);

            builder
                .HasOne(e => e.Oscar)
                .WithMany(e => e.MoviesArtists)
                .HasForeignKey(e => e.OscarId);
        }
    }
}
