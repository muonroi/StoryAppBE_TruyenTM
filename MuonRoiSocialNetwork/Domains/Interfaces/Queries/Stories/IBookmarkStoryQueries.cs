using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using SixLabors.ImageSharp.Formats.Gif;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories
{
    /// <summary>
    /// Declare interface
    /// </summary>
    public interface IBookmarkStoryQueries : IQueries<BookmarkStory>
    {
        /// <summary>
        /// Check eixst sstory bookmark of user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="storyGuid"></param>
        /// <returns></returns>
        Task<MethodResult<BookmarkStoryModelResponse>> ExistBookmarkStoryOfUser(Guid storyGuid, Guid userGuid);
    }
}
