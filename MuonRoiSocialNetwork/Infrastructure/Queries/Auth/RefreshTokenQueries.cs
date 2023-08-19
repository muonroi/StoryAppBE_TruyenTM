using AutoMapper;
using BaseConfig.BaseDbContext;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.Infrashtructure;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Auth;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Auth
{
    public class RefreshTokenQueries : BaseQuery<UserLogin>, IRefreshTokenQueries
    {
        public RefreshTokenQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IDistributedCache cache, IMapper mapper) : base(dbContext, authContext, cache, mapper)
        {
        }
    }
}
