using Microsoft.AspNetCore.SignalR;

namespace MuonRoiSocialNetwork.Infrastructure.HubCentral
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync($"ReceiveNotification , {Context.ConnectionId}");
        }
    }
}
