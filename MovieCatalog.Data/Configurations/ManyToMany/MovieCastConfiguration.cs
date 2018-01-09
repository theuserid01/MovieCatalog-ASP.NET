namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieCastConfiguration : IEntityTypeConfiguration<MovieCast>
    {
        public void Configure(EntityTypeBuilder<MovieCast> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.ArtistId, e.Character });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.MoviesCasts)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Artist)
                .WithMany(e => e.MoviesCasts)
                .HasForeignKey(e => e.ArtistId);
        }
    }
}
