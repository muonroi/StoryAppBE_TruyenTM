using BaseConfig.BaseDbContext.Common;
using MuonRoi.Social_Network.Roles;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles
{
    /// <summary>
    /// Interface init group methods
    /// </summary>
    public interface IGroupRepository : IRepository<GroupUserMember>
    {

    }
}
