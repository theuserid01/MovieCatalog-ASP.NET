namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieLanguageConfiguration : IEntityTypeConfiguration<MovieLanguage>
    {
        public void Configure(EntityTypeBuilder<MovieLanguage> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.LanguageId });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.Languages)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Language)
                .WithMany(e => e.Movies)
                .HasForeignKey(e => e.LanguageId);
        }
    }
}
