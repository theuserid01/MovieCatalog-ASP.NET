namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieStudioConfiguration : IEntityTypeConfiguration<MovieStudio>
    {
        public void Configure(EntityTypeBuilder<MovieStudio> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.StudioId });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.Studios)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Studio)
                .WithMany(e => e.Movies)
                .HasForeignKey(e => e.StudioId);
        }
    }
}
