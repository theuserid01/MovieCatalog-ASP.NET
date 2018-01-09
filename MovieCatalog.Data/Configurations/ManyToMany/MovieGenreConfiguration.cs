namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
    {
        public void Configure(EntityTypeBuilder<MovieGenre> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.GenreId });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.Genres)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Genre)
                .WithMany(e => e.Movies)
                .HasForeignKey(e => e.GenreId);
        }
    }
}
