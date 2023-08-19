using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Stories
{
    /// <summary>
    /// Story favorite repository handle
    /// </summary>
    public class StoryFavoriteRepository : BaseRepository<StoryFavorite>, IStoriesFavoriteRepository
    {
        private readonly MuonRoiSocialNetworkDbContext _dbcontext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public StoryFavoriteRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
            _dbcontext = dbContext;
        }
    }
}
