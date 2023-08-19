using BaseConfig.MethodResult;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles
{
    /// <summary>
    /// Interfacec initial role queries
    /// </summary>
    public interface IRoleQueries
    {
        /// <summary>
        /// Get role by guid
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<MethodResult<RoleInitialBaseResponse>> GetRoleByIdAsync(long roleId);
        /// <summary>
        /// Get role by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<MethodResult<bool>> IsRoleByNameExistAsync(string name);
    }
}
