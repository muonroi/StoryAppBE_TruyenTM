using MuonRoi.Social_Network.Storys;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MuonRoi.Social_Network.Configurations.Storys
{
    /// <summary>
    /// Configuration StoryPublish
    /// </summary>
    public class StoryPublishConfiguration : IEntityTypeConfiguration<StoryPublish>
    {
        /// <summary>
        /// Configuration StoryPublish
        /// </summary>
        public void Configure(EntityTypeBuilder<StoryPublish> builder)
        {
            builder.ToTable(nameof(StoryPublish).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.UserMember).WithMany(x => x.StoryPublish).HasForeignKey(x => x.UserGuid);
        }
    }
}
