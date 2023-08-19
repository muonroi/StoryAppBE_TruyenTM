using MuonRoi.Social_Network.Chapters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MuonRoi.Social_Network.Configurations.Chapters
{
    /// <summary>
    /// Configuration chapter
    /// </summary>
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        /// <summary>
        /// Configuration chapter
        /// </summary>
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable(nameof(Chapter).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.Story)
                .WithMany(x => x.Chapters)
                .HasForeignKey(x => x.StoryId);
        }
    }
}
