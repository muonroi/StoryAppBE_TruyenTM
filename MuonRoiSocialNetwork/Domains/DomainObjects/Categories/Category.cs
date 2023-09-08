using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Storys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Categories
{
    /// <summary>
    /// Category Table
    /// </summary>
    public class Category : Entity
    {
        /// <summary>
        /// Name of category
        /// </summary>
        [Required(ErrorMessage = nameof(EnumCategoriesErrorCode.CTS01))]
        [Column("category_name")]
        public string? NameCategory { get; set; }
        /// <summary>
        /// Status category
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; }
        /// <summary>
        /// Icon category
        /// </summary>
        [Column("icon")]
        public string IconName { get; set; } = string.Empty;
        /// <summary>
        /// Storys of category
        /// </summary>
        public List<Story>? Storys { get; set; }
    }
}
