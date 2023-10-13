using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Common.Models.Notifications;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories
{
    /// <summary>
    /// Declare story notification interface
    /// </summary>
    public interface IStoryNotificationQueries : IQueries<StoryNotifications>
    {
        /// <summary>
        /// Get notification for user
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<NotificationModels>>> GetNotifycationByUserGuid(int pageIndex, int pageSize);
    }
}
