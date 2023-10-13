using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuonRoi.Social_Network.Storys
{
    /// <summary>
    /// Table Notifications
    /// </summary>
    public class StoryNotifications : Entity
    {
        /// <summary>
        /// User guid
        /// </summary>
        [Column("user_guid")]
        public Guid UserGuid { get; set; }
        /// <summary>
        /// Story Guid
        /// </summary>
        [Column("story_id")]
        public long StoryId { get; set; }
        /// <summary>
        /// Url''s notifition
        /// </summary>
        [Required(ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT00))]
        [MaxLength(1000, ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT01))]
        [Column("notify_url")]
        public string NotificationUrl { get; set; }
        /// <summary>
        /// Title''s notifition
        /// </summary>
        [Required(ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT02))]
        [MaxLength(200, ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT03))]
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// Message''s notifition
        /// </summary>
        [Required(ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT04))]
        [MaxLength(350, ErrorMessage = nameof(EnumNotificationStoryErrorCodes.NT05))]
        [Column("message")]
        public string Message { get; set; }
        /// <summary>
        /// State''s notifition
        /// </summary>
        [Column("notification_state")]
        public EnumStateNotification NotificationSate { get; set; }
        /// <summary>
        /// Notification type
        /// </summary>
        [Column("notification_type")]
        public NotificationType NotificationType { get; set; }
        /// <summary>
        /// Url img of story
        /// </summary>
        [Required(ErrorMessage = nameof(EnumStoryErrorCode.ST06))]
        [MaxLength(1000, ErrorMessage = nameof(EnumStoryErrorCode.ST10))]
        [Column("img_url")]
        public string ImgUrl { get; set; }

        public Story Story { get; set; }
        public AppUser UserMember { get; set; }
    }
}
