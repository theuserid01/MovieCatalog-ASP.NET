namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieArtistGoldenGlobeConfiguration
       : IEntityTypeConfiguration<MovieArtistGoldenGlobe>
    {
        public void Configure(EntityTypeBuilder<MovieArtistGoldenGlobe> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.ArtistId, e.GoldenGlobeId, e.Role });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.ArtistsGoldenGlobes)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Artist)
                .WithMany(e => e.MoviesGoldenGlobes)
                .HasForeignKey(e => e.ArtistId);

            builder
                .HasOne(e => e.GoldenGlobe)
                .WithMany(e => e.MoviesArtists)
                .HasForeignKey(e => e.GoldenGlobeId);
        }
    }
}
