using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Stories
{
    /// <summary>
    /// Declare class
    /// </summary>
    public class BookmarkStoryQueries : BaseQuery<BookmarkStory>, IBookmarkStoryQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="cache"></param>
        /// <param name="mapper"></param>
        public BookmarkStoryQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IDistributedCache cache, IMapper mapper) : base(dbContext, authContext, cache, mapper)
        {
        }
        /// <summary>
        /// Check exist story book mark of user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="storyGuid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<BookmarkStoryModelResponse>> ExistBookmarkStoryOfUser(Guid storyGuid, Guid userGuid)
        {
            var methodResult = new MethodResult<BookmarkStoryModelResponse>();
            var bookmarkResult = await _dbSet.Where(x => x.StoryGuid == storyGuid && x.UserGuid == userGuid).FirstOrDefaultAsync();
            if (bookmarkResult is null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.Result = null;
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = _mapper.Map<BookmarkStoryModelResponse>(bookmarkResult);
            return methodResult;
        }
    }
}
