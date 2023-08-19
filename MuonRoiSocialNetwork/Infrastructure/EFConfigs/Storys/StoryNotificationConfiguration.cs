using MuonRoi.Social_Network.Storys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MuonRoi.Social_Network.Configurations.Storys
{
    /// <summary>
    /// Configuration StoryNotification
    /// </summary>
    public class StoryNotificationConfiguration : IEntityTypeConfiguration<StoryNotifications>
    {
        /// <summary>
        /// Configuration StoryNotification
        /// </summary>
        public void Configure(EntityTypeBuilder<StoryNotifications> builder)
        {
            builder.ToTable(nameof(StoryNotifications).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.Story).WithMany(x => x.StoryNotifications).HasForeignKey(x => x.StoryId);
            builder.HasOne(x => x.UserMember).WithMany(x => x.StoryNotifications).HasForeignKey(x => x.UserGuid);
        }
    }
}
