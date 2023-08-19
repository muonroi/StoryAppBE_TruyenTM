using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Tags;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Users
{
    /// <summary>
    /// Table follow
    /// </summary>
    public class FollowingAuthor : Entity
    {
        /// <summary>
        /// UserGuid
        /// </summary>
        [Column("user_guid")]
        public Guid UserGuid { get; set; }
        /// <summary>
        /// StoryGuid
        /// </summary>
        [Required(ErrorMessage = nameof(EnumTagsErrorCode.TT05))]
        [Column("story_guid")]
        public Guid StoryGuid { get; set; }
        public AppUser UserMember { get; set; }
    }
}
