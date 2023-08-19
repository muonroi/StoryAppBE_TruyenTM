using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Stories
{
    /// <summary>
    /// Handler stories favorites
    /// </summary>
    public class StoriesFavoriteQueries : BaseQuery<StoryFavorite>, IStoriesFavoriteQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="mapper"></param>
        /// <param name="cache"></param>
        public StoriesFavoriteQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IMapper mapper, IDistributedCache cache) : base(dbContext, authContext, cache, mapper)
        {
        }
        /// <summary>
        /// Handler function check user was like story
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="storyId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> IsUserWasLikeStoryAsync(Guid userGuid, long storyId)
        {
            MethodResult<bool> methodResult = new();
            methodResult.Result = await _queryable.AsNoTracking().AnyAsync(x => x.StoryId == storyId && x.UserGuid == userGuid);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
