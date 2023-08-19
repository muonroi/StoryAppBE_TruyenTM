using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Enums.Storys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Storys
{
    /// <summary>
    /// Table Review
    /// </summary>
    public class StoryReview : Entity
    {
        /// <summary>
        /// Story guid
        /// </summary>
        [Required(ErrorMessage = nameof(EnumTagsErrorCode.TT06))]
        [Column("story_guid")]
        public Guid StoryGuid { get; set; }
        /// <summary>
        /// User guid
        /// </summary>
        [Column("user_guid")]
        public Guid UserGuid { get; set; }
        /// <summary>
        /// Vote for story
        /// </summary>
        [Column("rating")]
        public double Rating { get; set; }
        /// <summary>
        /// Content for story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumReviewStorys.RVST01))]
        [MinLength(15, ErrorMessage = nameof(EnumReviewStorys.RVST02))]
        [MaxLength(200, ErrorMessage = nameof(EnumReviewStorys.RVST03))]
        [Column("content")]
        public string Content { get; set; } = string.Empty;
        public AppUser UserMember { get; set; }
    }
}
