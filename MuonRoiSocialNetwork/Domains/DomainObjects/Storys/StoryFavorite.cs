using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoiSocialNetwork.Domains.DomainObjects.Storys
{
    /// <summary>
    /// Table list user favorite of story
    /// </summary>
    public class StoryFavorite : Entity
    {
        /// <summary>
        /// Guid of user
        /// </summary>
        [Column("user_guid")]
        public Guid UserGuid { get; set; }
        /// <summary>
        /// Guid of story
        /// </summary>
        [Column("story_id")]
        public long StoryId { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public AppUser AppUser { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public Story Story { get; set; }
    }
}
