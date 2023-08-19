using MuonRoi.Social_Network.Categories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MuonRoi.Social_Network.Configurations.Categories
{
    /// <summary>
    /// Configuration category
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Configuration category
        /// </summary>
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(nameof(Category).ToLower());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
