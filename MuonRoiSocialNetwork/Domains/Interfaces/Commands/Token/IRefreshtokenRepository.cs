using BaseConfig.BaseDbContext.Common;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Token
{
    /// <summary>
    /// Interface decalare function handle refresh token table
    /// </summary>
    public interface IRefreshtokenRepository : IRepository<UserLogin>
    {
        /// <summary>
        /// Get data to refresh token table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Dictionary<string, string[]>> GetInfoRefreshTokenAsync(Guid id);
    }
}
