using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories
{
    /// <summary>
    /// Define review interface
    /// </summary>
    public interface IReviewStoryQueries : IQueries<StoryReview>
    {
        /// <summary>
        /// Get list comments of story
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<StoryReviewModelResponse>>> GetListCommentsOfStory(int storyId, int pageIndex = 1, int pageSize = 10);
    }
}
