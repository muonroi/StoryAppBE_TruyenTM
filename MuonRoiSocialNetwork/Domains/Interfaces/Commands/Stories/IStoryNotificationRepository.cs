using BaseConfig.BaseDbContext.Common;
using MuonRoi.Social_Network.Storys;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories
{
    /// <summary>
    /// Declare repository interface
    /// </summary>
    public interface IStoryNotificationRepository : IRepository<StoryNotifications>
    {
    }
}
