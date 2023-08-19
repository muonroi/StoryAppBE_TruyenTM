using BaseConfig.BaseDbContext.Common;
using MuonRoi.Social_Network.Roles;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Request;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles
{
    /// <summary>
    /// Interface init role methods
    /// </summary>
    public interface IRoleRepository : IRepository<AppRole>
    {
    }
}
