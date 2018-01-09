namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieCrewConfiguration : IEntityTypeConfiguration<MovieCrew>
    {
        public void Configure(EntityTypeBuilder<MovieCrew> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.ArtistId, e.Role });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.MoviesCrews)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Artist)
                .WithMany(e => e.MoviesCrews)
                .HasForeignKey(e => e.ArtistId);
        }
    }
}
