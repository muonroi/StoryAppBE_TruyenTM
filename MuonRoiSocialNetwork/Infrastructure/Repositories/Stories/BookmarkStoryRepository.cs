using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Stories
{
    /// <summary>
    /// Declare class
    /// </summary>
    public class BookmarkStoryRepository : BaseRepository<BookmarkStory>, IBookmarkStoryRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public BookmarkStoryRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        { }
    }
}
