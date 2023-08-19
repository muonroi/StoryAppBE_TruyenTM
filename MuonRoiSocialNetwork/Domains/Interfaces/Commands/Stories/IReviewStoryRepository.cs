using BaseConfig.BaseDbContext.Common;
using MuonRoi.Social_Network.Storys;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories
{
    /// <summary>
    /// Define review story
    /// </summary>
    public interface IReviewStoryRepository : IRepository<StoryReview>
    {
    }
}
