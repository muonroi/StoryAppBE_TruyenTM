using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Common.Models.Stories.Request.Search;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories
{
    /// <summary>
    /// Interface of stories (queries)
    /// </summary>
    public interface IStoriesQueries : IQueries<Story>
    {
        /// <summary>
        /// Search all story paging
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        Task<MethodResult<StoryModelResponse>> GetStoryAsync(int storyId);
        /// <summary>
        /// Search all story paging
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<StoryModelResponse>>> GetStoriesAsync(int pageIndex = 1, int pageSize = 20);
        /// <summary>
        /// Search story by parameters
        /// </summary>
        /// <param name="requestSearch"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<StoryModelResponse>>> GetStoriesByParameters(SearchStoriesModelRequest requestSearch);
        /// <summary>
        /// Recommend stories by id
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<StoryModelResponse>>> RecommendStoriesById(int storyId, int pageIndex = 1, int pageSize = 10);
    }
}
