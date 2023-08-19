using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Storys
{
    /// <summary>
    /// Table StoryPublished
    /// </summary>
    public class StoryPublish : Entity
    {
        /// <summary>
        /// Guid story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumTagsErrorCode.TT06))]
        [Column("story_guid")]
        public Guid StoryGuid { get; set; }
        /// <summary>
        /// Guid User
        /// </summary>
        [Column("user_guid")]
        public Guid UserGuid { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public AppUser UserMember { get; set; }

    }
}
