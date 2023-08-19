using MuonRoi.Social_Network.Storys;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MuonRoi.Social_Network.Configurations.Storys
{
    /// <summary>
    /// Configuration StoryReview
    /// </summary>
    public class StoryReviewConfiguration : IEntityTypeConfiguration<StoryReview>
    {
        /// <summary>
        /// Configuration StoryReview
        /// </summary>
        public void Configure(EntityTypeBuilder<StoryReview> builder)
        {
            builder.ToTable(nameof(StoryReview).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Content);
            builder.HasOne(x => x.UserMember).WithMany(x => x.StoryReview).HasForeignKey(x => x.UserGuid);
        }
    }
}