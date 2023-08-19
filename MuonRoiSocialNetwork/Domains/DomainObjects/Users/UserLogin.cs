using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoiSocialNetwork.Domains.DomainObjects.Users
{
    /// <summary>
    /// Userloggin table
    /// </summary>
    public class UserLogin : Entity
    {
        /// <summary>
        /// UserId
        /// </summary>
        [Column("user_id")]
        public Guid UserId { get; set; }
        /// <summary>
        /// Refresh Token
        /// </summary>
        [Column("refresh_token")]
        public string? RefreshToken { get; set; }
        /// <summary>
        /// Key decrypt token
        /// </summary>
        [Column("key_salf")]
        public string? KeySalt { get; set; }
        /// <summary>
        /// Refresh Token ExpiryTime
        /// </summary>
        [Column("refreshtoken_expirytime_ts")]
        public double? RefreshTokenExpiryTimeTS { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        public AppUser? AppUser { get; set; }
    }
}
