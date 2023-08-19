using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories
{
    /// <summary>
    /// Declare interface stories favorite queries
    /// </summary>
    public interface IStoriesFavoriteQueries : IQueries<StoryFavorite>
    {
        /// <summary>
        /// Check user was like story or not?
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="storyId"></param>
        /// <returns></returns>
        Task<MethodResult<bool>> IsUserWasLikeStoryAsync(Guid userGuid, long storyId);
    }
}
