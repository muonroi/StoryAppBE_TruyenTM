using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MuonRoiSocialNetwork.Common.Models.Notifications;

namespace MuonRoiSocialNetwork.Infrastructure.HubCentral
{
    /// <summary>
    /// Config class signalr
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Notification to all user
        /// </summary>
        /// <param name="notificationModels"></param>
        /// <returns></returns>
        public async Task NotifyToAllUser(NotificationModels notificationModels)
        => await Clients.All.SendAsync("ReceiveNotification", notificationModels);
        /// <summary>
        /// Notification to special user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="notificationModels"></param>
        /// <returns></returns>
        public async Task NotifyToSpecialUser(NotificationModels notificationModels, Guid userGuid)
        => await Clients.User(userGuid.ToString()).SendAsync("ReceiveSingle", notificationModels);
    }
}
