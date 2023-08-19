using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;

namespace MuonRoiSocialNetwork.Infrastructure.EFConfigs.Groups
{
    /// <summary>
    /// Configuration role
    /// </summary>
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        /// <summary>
        /// Configuration role
        /// </summary>
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable(nameof(AppRole).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).IsUnicode();
        }
    }
}
