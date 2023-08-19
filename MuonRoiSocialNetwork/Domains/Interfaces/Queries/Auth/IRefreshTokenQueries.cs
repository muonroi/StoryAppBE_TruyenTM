using BaseConfig.BaseDbContext.Common;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Auth
{
    public interface IRefreshTokenQueries : IQueries<UserLogin>
    {
    }
}
