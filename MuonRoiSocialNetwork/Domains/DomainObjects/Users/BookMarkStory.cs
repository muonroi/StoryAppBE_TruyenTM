using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Tags;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Users
{
    /// <summary>
    /// Table Bookmark
    /// </summary>
    public class BookmarkStory : Entity
    {
        /// <summary>
        /// Story Guid
        /// </summary>
        [Required(ErrorMessage = nameof(EnumTagsErrorCode.TT06))]
        [Column("story_guid")]
        public Guid StoryGuid { get; set; }
        /// <summary>
        /// User Guid
        /// </summary>
        [Column("user_guid")]
        public Guid UserGuid { get; set; }
        /// <summary>
        /// BookmarkDate
        /// </summary>
        [Column("bookmark_date")]
        public DateTime BookmarkDate { get; set; }
        public AppUser UserMember { get; set; }
    }
}
