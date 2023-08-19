using MuonRoi.Social_Network.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MuonRoi.Social_Network.Configurations.Users
{
    /// <summary>
    /// Configuration FollowingAuthor
    /// </summary>
    public class FollowingAuthorConfiguration : IEntityTypeConfiguration<FollowingAuthor>
    {
        /// <summary>
        /// Configuration FollowingAuthor
        /// </summary>
        public void Configure(EntityTypeBuilder<FollowingAuthor> builder)
        {
            builder.ToTable(nameof(FollowingAuthor).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.UserMember)
              .WithMany(x => x.FollowingAuthor)
              .HasForeignKey(x => x.UserGuid);
        }
    }
}
