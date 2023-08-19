using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoi.Social_Network.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Chapters;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Chapters
{
    /// <summary>
    /// Chapter repository
    /// </summary>
    public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public ChapterRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
        }
    }
}
