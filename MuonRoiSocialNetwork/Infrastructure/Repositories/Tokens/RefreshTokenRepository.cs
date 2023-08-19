using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Infrashtructure;
using Microsoft.EntityFrameworkCore;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Token;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Token
{
    /// <summary>
    /// Repository of refresh Token
    /// </summary>
    public class RefreshTokenRepository : BaseRepository<UserLogin>, IRefreshtokenRepository
    {
        private readonly MuonRoiSocialNetworkDbContext _dbContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="auth"></param>
        public RefreshTokenRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext auth) : base(dbContext, auth)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Handle function get info refresh token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string[]>> GetInfoRefreshTokenAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new Dictionary<string, string[]> { { "false", new[] { "false" } } };
            }
            UserLogin? userLoggin = await _dbContext.UserLoggins.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id && !x.IsDeleted).ConfigureAwait(false);
            if (userLoggin is null)
            {
                return new Dictionary<string, string[]> { { "FirstLogin", new[] { "FirstLogin" } } };
            }
            Dictionary<string, string[]> keyValuePairs = new()
            {
               { userLoggin.UserId.ToString(), new[] { userLoggin.KeySalt, userLoggin.RefreshToken,userLoggin.RefreshTokenExpiryTimeTS.ToString() } }
            };
            return keyValuePairs;
        }
    }
}
