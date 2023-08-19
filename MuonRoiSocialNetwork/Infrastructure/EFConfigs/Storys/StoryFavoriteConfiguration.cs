using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;

namespace MuonRoiSocialNetwork.Infrastructure.EFConfigs.Storys
{
    /// <summary>
    /// Configuration favorite of story
    /// </summary>
    public class StoryFavoriteConfiguration : IEntityTypeConfiguration<StoryFavorite>
    {
        /// <summary>
        /// Configuration favorite of story
        /// </summary>
        public void Configure(EntityTypeBuilder<StoryFavorite> builder)
        {
            builder.ToTable(nameof(StoryFavorite).ToLower());
            builder.HasKey(x => new { x.StoryId, x.UserGuid });

            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.StoryFavorite)
                .HasForeignKey(x => x.UserGuid);

            builder.HasOne(x => x.Story)
                .WithMany(x => x.StoryFavorite)
                .HasForeignKey(x => x.StoryId);

        }
    }
}
