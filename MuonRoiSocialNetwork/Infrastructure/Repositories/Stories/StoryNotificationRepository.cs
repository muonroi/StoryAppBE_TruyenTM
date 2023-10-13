using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Stories
{
    /// <summary>
    /// Story notification repository
    /// </summary>
    public class StoryNotificationRepository : BaseRepository<StoryNotifications>, IStoryNotificationRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public StoryNotificationRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        { }
    }
}
