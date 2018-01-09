namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieColorConfiguration : IEntityTypeConfiguration<MovieColor>
    {
        public void Configure(EntityTypeBuilder<MovieColor> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.ColorId });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.Colors)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Color)
                .WithMany(e => e.Movies)
                .HasForeignKey(e => e.ColorId);
        }
    }
}
