using Microsoft.AspNetCore.SignalR;

namespace MuonRoiSocialNetwork.Infrastructure.HubCentral.Base
{
    public interface IdBasedUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("sub").Value;
        }
    }
}
