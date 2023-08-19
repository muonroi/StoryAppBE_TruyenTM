using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Roles;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.GroupAndRoles
{
    /// <summary>
    /// Handler queries
    /// </summary>
    public class GroupQueries : BaseQuery<GroupUserMember>, IGroupQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="mapper"></param>
        /// <param name="cache"></param>
        public GroupQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IMapper mapper, IDistributedCache cache) : base(dbContext, authContext, cache, mapper)
        {
        }
        /// <summary>
        /// Handler get group by name
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> GetByNameAsync(string groupName)
        {
            MethodResult<bool> methodResult = new();
            if (string.IsNullOrEmpty(groupName))
            {
                methodResult.Result = false;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = await _queryable.AsNoTracking().AnyAsync(x => x.GroupName == groupName);
            return methodResult;
        }
    }
}
