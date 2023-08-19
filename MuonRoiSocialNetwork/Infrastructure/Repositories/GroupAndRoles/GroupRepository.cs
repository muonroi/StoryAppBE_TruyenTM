using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using MuonRoi.Social_Network.Roles;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.GroupAndRoles
{
    /// <summary>
    /// Group repository
    /// </summary>
    public class GroupRepository : BaseRepository<GroupUserMember>, IGroupRepository
    {
        private readonly MuonRoiSocialNetworkDbContext _dbContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        public GroupRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
            _dbContext = dbContext;
        }

    }
}
