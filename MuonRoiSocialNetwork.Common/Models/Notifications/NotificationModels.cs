using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.Enum;

namespace MuonRoiSocialNetwork.Common.Models.Notifications
{
    public class NotificationModels
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NotificationUrl { get; set; }
        public string ImgUrl { get; set; }
        public EnumStateNotification NotificationSate { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public Guid UserGuid { get; set; }
        public long StoryId { get; set; }

    }
}
