using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.Enum;
using Newtonsoft.Json;

namespace MuonRoiSocialNetwork.Common.Models.Notifications.Base
{
    public class BaseNotificationModels
    {
        [JsonProperty("notificationcontent")]
        public string NotificationContent { get; set; } = string.Empty;
        [JsonProperty("timecreated")]
        public string TimeCreated { get; set; } = string.Empty;
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
        [JsonProperty("type")]
        public NotificationType Type { get; set; }
    }
}
