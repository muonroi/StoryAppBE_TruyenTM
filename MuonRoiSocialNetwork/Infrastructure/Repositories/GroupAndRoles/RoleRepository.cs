using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Chapters;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.GroupAndRoles
{
    /// <summary>
    /// Role Repository
    /// </summary>
    public class RoleRepository : BaseRepository<AppRole>, IRoleRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <param name="auth"></param>
        public RoleRepository(MuonRoiSocialNetworkDbContext dbcontext, AuthContext auth) : base(dbcontext, auth) { }

    }
}
