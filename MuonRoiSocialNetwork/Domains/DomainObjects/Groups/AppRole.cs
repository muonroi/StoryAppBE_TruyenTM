using MuonRoi.Social_Network.Roles;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using BaseConfig.EntityObject.EntityObject;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using BaseConfig.EntityObject.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoiSocialNetwork.Domains.DomainObjects.Groups
{
    /// <summary>
    /// AppRole
    /// </summary>
    public class AppRole : Entity
    {
        /// <summary>
        /// Role name
        /// </summary>
        [Column("role_name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Normal name
        /// </summary>
        [Column("normalized_name")]
        public string NormalizedName { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }
    }
}
