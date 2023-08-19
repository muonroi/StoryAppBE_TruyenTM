using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Common.Models.Tags.Response;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories
{
    /// <summary>
    /// Define interface tag 
    /// </summary>
    public interface ITagQueries : IQueries<Tag>
    {
        /// <summary>
        /// Get all tag
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Task<MethodResult<PagingItemsDTO<TagModelResponse>>> GetAllTag(int pageIndex = 1, int pageSize = 10);
        /// <summary>
        /// Get special tag by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MethodResult<TagModelResponse>> GetTagById(int id);
        /// <summary>
        /// Get special tag by name
        /// </summary>
        /// <param name="nameTag"></param>
        /// <returns></returns>
        public Task<MethodResult<bool>> GetTagByName(string nameTag);
    }
}
