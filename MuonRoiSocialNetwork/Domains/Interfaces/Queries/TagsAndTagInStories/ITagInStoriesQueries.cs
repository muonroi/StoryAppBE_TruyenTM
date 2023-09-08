using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Response;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories
{
    /// <summary>
    /// Define interface query tag in story
    /// </summary>
    public interface ITagInStoriesQueries : IQueries<TagInStory>
    {
        /// <summary>
        /// Get special tag in story by id
        /// </summary>
        /// <param name="idTag"></param>
        /// <param name="storyId"></param>
        /// <returns></returns>
        Task<MethodResult<TagInStoriesModelResponse>> GetTagInStoriesById(int idTag, int storyId);
        /// <summary>
        /// Get all tag in story
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<MethodResult<List<TagInStoriesModelResponse>>> GetAllTagInStory(int pageIndex, int pageSize);
    }
}
