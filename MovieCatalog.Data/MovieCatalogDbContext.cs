namespace MovieCatalog.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using MovieCatalog.Data.Configurations.ManyToMany;
    using MovieCatalog.Data.Models;
    using MovieCatalog.Data.Models.ManyToMany;

    public class MovieCatalogDbContext : IdentityDbContext<User>
    {
        public MovieCatalogDbContext(DbContextOptions<MovieCatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<GoldenGlobe> GoldenGlobes { get; set; }

        public DbSet<HomeVideo> HomeVideos { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Oscar> Oscars { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Poster> Posters { get; set; }

        public DbSet<Release> Releases { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Studio> Studios { get; set; }

        public DbSet<HomeVideoSubtitle> HomeVideosSubtitles { get; set; }

        public DbSet<MovieCast> MoviesCasts { get; set; }

        public DbSet<MovieColor> MoviesColors { get; set; }

        public DbSet<MovieCountry> MoviesCountries { get; set; }

        public DbSet<MovieCrew> MoviesCrews { get; set; }

        public DbSet<MovieGenre> MoviesGenres { get; set; }

        public DbSet<MovieArtistGoldenGlobe> MoviesArtistsGoldenGlobes { get; set; }

        public DbSet<MovieArtistOscar> MoviesArtistsOscars { get; set; }

        public DbSet<MovieLanguage> MoviesLanguages { get; set; }

        public DbSet<MovieStudio> MoviesStudios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Artist>()
                .HasIndex(e => e.ImdbId)
                .IsUnique();

            builder.Entity<Color>()
                .HasIndex(e => e.Name)
                .IsUnique();

            builder.Entity<Country>()
                .HasIndex(e => e.Name)
                .IsUnique();

            builder.Entity<Country>()
                .HasIndex(e => e.OfficialName)
                .IsUnique();

            builder.Entity<Genre>()
               .HasIndex(e => e.Name)
               .IsUnique();

            builder.Entity<Language>()
                .HasIndex(e => e.Name)
                .IsUnique();

            builder.Entity<Studio>()
                .HasIndex(e => e.Name)
                .IsUnique();

            // One-to-Many

            builder.Entity<Artist>()
                .HasMany(e => e.Photos)
                .WithOne(e => e.Artist)
                .HasForeignKey(e => e.ArtistId);

            builder.Entity<Country>()
                .HasMany(e => e.HomeVideos)
                .WithOne(e => e.Country)
                .HasForeignKey(e => e.CountryId);

            builder.Entity<Country>()
                .HasMany(e => e.Releases)
                .WithOne(e => e.Country)
                .HasForeignKey(e => e.CountryId);

            builder.Entity<Location>()
                .HasMany(e => e.Releases)
                .WithOne(e => e.Location)
                .HasForeignKey(e => e.LocationId);

            builder.Entity<Movie>()
                .HasMany(e => e.HomeVideos)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId);

            builder.Entity<Movie>()
                .HasMany(e => e.Posters)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId);

            builder.Entity<Movie>()
                .HasMany(e => e.Releases)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId);

            builder.Entity<Movie>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId);

            builder.Entity<User>()
                .HasMany(e => e.Movies)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            builder.Entity<User>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId);

            builder.ApplyConfiguration(new HomeVideoSubtitleConfiguration());

            builder.ApplyConfiguration(new MovieArtistGoldenGlobeConfiguration());

            builder.ApplyConfiguration(new MovieArtistOscarConfiguration());

            builder.ApplyConfiguration(new MovieCastConfiguration());

            builder.ApplyConfiguration(new MovieColorConfiguration());

            builder.ApplyConfiguration(new MovieCountryConfiguration());

            builder.ApplyConfiguration(new MovieCrewConfiguration());

            builder.ApplyConfiguration(new MovieGenreConfiguration());

            builder.ApplyConfiguration(new MovieLanguageConfiguration());

            builder.ApplyConfiguration(new MovieStudioConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
