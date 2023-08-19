using MuonRoi.Social_Network.Roles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MuonRoi.Social_Network.Configurations.Roles
{
    /// <summary>
    /// Configuration GroupUserMember
    /// </summary>
    public class GroupUserMemberConfiguration : IEntityTypeConfiguration<GroupUserMember>
    {
        /// <summary>
        /// Configuration GroupUserMember
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<GroupUserMember> builder)
        {
            builder.ToTable(nameof(GroupUserMember).ToLower());
            builder.HasKey(x => x.Id);
        }
    }
}
