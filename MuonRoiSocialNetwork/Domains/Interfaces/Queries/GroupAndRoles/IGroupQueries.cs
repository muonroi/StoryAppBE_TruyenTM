using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Roles;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles
{
    /// <summary>
    /// Interface of group usermember (queries)
    /// </summary>
    public interface IGroupQueries : IQueries<GroupUserMember>
    {
        /// <summary>
        /// Get group by name
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<MethodResult<bool>> GetByNameAsync(string groupName);
    }
}
