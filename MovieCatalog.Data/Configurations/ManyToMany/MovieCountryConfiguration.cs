namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieCountryConfiguration : IEntityTypeConfiguration<MovieCountry>
    {
        public void Configure(EntityTypeBuilder<MovieCountry> builder)
        {
            builder
                .HasKey(e => new { e.MovieId, e.CountryId });

            builder
                .HasOne(e => e.Movie)
                .WithMany(e => e.Countries)
                .HasForeignKey(e => e.MovieId);

            builder
                .HasOne(e => e.Country)
                .WithMany(e => e.Movies)
                .HasForeignKey(e => e.CountryId);
        }
    }
}
