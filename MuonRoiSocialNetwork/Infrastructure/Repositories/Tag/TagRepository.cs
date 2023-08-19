using BaseConfig.BaseDbContext.BaseRepository;
using TagEntities = MuonRoi.Social_Network.Tags.Tag;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using BaseConfig.Infrashtructure;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Tag
{
    /// <summary>
    /// Decalre tag repository
    /// </summary>
    public class TagRepository : BaseRepository<TagEntities>, ITagRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public TagRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
        }
    }
}
