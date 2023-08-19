using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;
using MuonRoi.Social_Network.Users;

namespace MuonRoiSocialNetwork.Infrastructure.EFConfigs.Users
{
    /// <summary>
    /// UserLogginConfiguration
    /// </summary>
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        /// <summary>
        /// UserLogginConfiguration
        /// </summary>
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable(nameof(UserLogin).ToLower());
            builder.HasKey(x => x.Id);

        }
    }
}
