using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Stories
{
    /// <summary>
    /// Story repository
    /// </summary>
    public class StoriesRepository : BaseRepository<Story>, IStoriesRepository
    {
        private readonly MuonRoiSocialNetworkDbContext _dbcontext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public StoriesRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
            _dbcontext = dbContext;
        }
        /// <summary>
        /// Update single column
        /// </summary>
        /// <param name="entityUpdate"></param>
        /// <param name="columName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<int> UpdateSingleEntry(Story entityUpdate, string columName, object value)
        {
            if (entityUpdate == null) return -1;
            entityUpdate.GetType().GetProperty(columName)?.SetValue(entityUpdate, value);
            _dbcontext.Entry(entityUpdate).Property(columName).IsModified = true;
            return await _dbcontext.SaveChangesAsync();
        }
    }
}
