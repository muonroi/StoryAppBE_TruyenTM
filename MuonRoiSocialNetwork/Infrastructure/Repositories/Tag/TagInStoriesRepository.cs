using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using TagInStoryEntities = MuonRoi.Social_Network.Tags.TagInStory;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Tag
{
    /// <summary>
    /// Define tag in story repository
    /// </summary>
    public class TagInStoriesRepository : BaseRepository<TagInStoryEntities>, ITagInStoryRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public TagInStoriesRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
        }
    }
}
