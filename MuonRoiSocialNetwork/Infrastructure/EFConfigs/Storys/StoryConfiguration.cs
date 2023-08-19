using MuonRoi.Social_Network.Storys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MuonRoi.Social_Network.Configurations.Storys
{
    /// <summary>
    /// Configuration Story
    /// </summary>
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        /// <summary>
        /// Configuration Story
        /// </summary>
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.ToTable(nameof(Story).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IsShow).HasDefaultValue(false);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Storys)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
