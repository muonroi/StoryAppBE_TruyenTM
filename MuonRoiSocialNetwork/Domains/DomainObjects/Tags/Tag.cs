using BaseConfig.EntityObject.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Tags
{
    /// <summary>
    /// Table for Tag
    /// </summary>
    public class Tag : Entity
    {
        /// <summary>
        /// Tag for name
        /// </summary>
        [Required(ErrorMessage = nameof(EnumTagsErrorCode.TT00))]
        [MaxLength(50, ErrorMessage = nameof(EnumTagsErrorCode.TT01))]
        [MinLength(3, ErrorMessage = nameof(EnumTagsErrorCode.TT02))]
        [Column("tag_name")]
        public string TagName { get; set; }
        /// <summary>
        /// Details for tag
        /// </summary>
        [Required(ErrorMessage = nameof(EnumTagsErrorCode.TT03))]
        [MaxLength(500, ErrorMessage = nameof(EnumTagsErrorCode.TT04))]
        [Column("details")]
        public string Details { get; set; }
        public List<TagInStory> TagInStory { get; set; }
    }
}
