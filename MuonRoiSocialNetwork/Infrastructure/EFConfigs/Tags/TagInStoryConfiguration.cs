using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MuonRoi.Social_Network.Tags;

namespace MuonRoi.Social_Network.Configurations.Tags
{
    /// <summary>
    /// Configuration TagInStory
    /// </summary>
    public class TagInStoryConfiguration : IEntityTypeConfiguration<TagInStory>
    {
        /// <summary>
        /// Configuration TagInStory
        /// </summary>
        public void Configure(EntityTypeBuilder<TagInStory> builder)
        {
            builder.ToTable(nameof(TagInStory).ToLower());
            builder.HasKey(x => new { x.Id });

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.TagInStory)
                .HasForeignKey(x => x.TagId);

            builder.HasOne(x => x.Story)
                .WithMany(x => x.TagInStory)
                .HasForeignKey(x => x.StoryId);
        }
    }
}
