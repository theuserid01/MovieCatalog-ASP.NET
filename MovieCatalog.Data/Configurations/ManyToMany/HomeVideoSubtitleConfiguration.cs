namespace MovieCatalog.Data.Configurations.ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MovieCatalog.Data.Models.ManyToMany;

    public class HomeVideoSubtitleConfiguration : IEntityTypeConfiguration<HomeVideoSubtitle>
    {
        public void Configure(EntityTypeBuilder<HomeVideoSubtitle> builder)
        {
            builder
                .HasKey(e => new { e.HomeVideoId, e.SubtitleId });

            builder
               .HasOne(e => e.HomeVideo)
               .WithMany(e => e.Subtitles)
               .HasForeignKey(e => e.HomeVideoId);

            builder
               .HasOne(e => e.Subtitle)
               .WithMany(e => e.HomeVideos)
               .HasForeignKey(e => e.SubtitleId);
        }
    }
}
