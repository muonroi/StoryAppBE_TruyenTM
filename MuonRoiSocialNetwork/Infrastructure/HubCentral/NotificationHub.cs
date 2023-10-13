using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MuonRoiSocialNetwork.Common.Models.Notifications;
using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.GroupName;
using MuonRoiSocialNetwork.Infrastructure.HubCentral.Base;
using Serilog;

namespace MuonRoiSocialNetwork.Infrastructure.HubCentral
{
    /// <summary>
    /// Config class signalr
    /// </summary>
    public class NotificationHub : Hub
    {
        private readonly static BaseNotification<string> _connections =
            new();
        /// <summary>
        /// Connected
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            Log.Information($"UserId: {Context.UserIdentifier} is connected!");
            return Task.CompletedTask;
        }
        /// <summary>
        /// Disconnected
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            base.OnDisconnectedAsync(exception);
            Log.Information($"UserId: {Context.UserIdentifier} is disconnected! with reason: {exception?.Message} {exception}");
            return Task.CompletedTask;
        }
        /// <summary>
        /// Join user to group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task JoinGroup(string groupName)
          => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        /// <summary>
        /// Remove user in group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task LeaveGroup(string groupName)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        /// <summary>
        /// Notification to all user
        /// </summary>
        /// <param name="notificationModels"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task NotifyToAllUser(NotificationModels notificationModels)
        => await Clients.All.SendAsync(GroupHelperConst.Instance.StreamGlobal, notificationModels);
        /// <summary>
        /// Notification to special user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="notificationModels"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task NotifyToSpecialUser(NotificationModels notificationModels, Guid userGuid)
        => await Clients.User(userGuid.ToString()).SendAsync(GroupHelperConst.Instance.StreamNameSingle, notificationModels);
    }
}
