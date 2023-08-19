using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Roles
{
    /// <summary>
    /// Table of group user
    /// </summary>
    public class GroupUserMember : Entity
    {
        /// <summary>
        /// Group id
        /// </summary>
        [Column("id")]
        public override long Id { get; set; }
        /// <summary>
        /// Name of groups
        /// </summary>
        [Column("group_name")]
        public string GroupName { get; set; } = string.Empty;
        /// <summary>
        /// List role of group
        /// </summary>
        [Column("roles")]
        public string Roles { get; set; } = string.Empty;
        /// <summary>
        /// UserMember
        /// </summary>
        public ICollection<AppUser>? UserMember { get; set; }
    }
}
