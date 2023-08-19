using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Stories.Reviews
{
    /// <summary>
    /// Define class review story
    /// </summary>
    public class ReviewStoryRepository : BaseRepository<StoryReview>, IReviewStoryRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public ReviewStoryRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
        }
    }
}
