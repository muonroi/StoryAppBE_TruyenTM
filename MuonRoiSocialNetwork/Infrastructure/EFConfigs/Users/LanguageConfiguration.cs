using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;

namespace MuonRoiSocialNetwork.Infrastructure.EFConfigs.Users
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        /// <summary>
        /// UserLogginConfiguration
        /// </summary>
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable(nameof(Language).ToLower());
            builder.HasKey(x => x.LanguageName);

        }
    }
}
