using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users
{
    /// <summary>
    /// Interface UserQuery
    /// </summary>
    public interface IUserQueries
    {
        /// <summary>
        /// Get user by guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns>AppUser</returns>
        Task<MethodResult<AppUser>> GetByGuidAsync(Guid id);
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>AppUser</returns>
        Task<MethodResult<AppUser>> GetByUsernameAsync(string username);
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>UserModel</returns>
        Task<MethodResult<BaseUserResponse>> GetUserModelBynameAsync(string username);
        /// <summary>
        /// Get user by guid
        /// </summary>
        /// <param name="guidUser"></param>
        /// <returns>UserModel</returns>
        Task<MethodResult<BaseUserResponse>> GetUserModelByGuidAsync(Guid guidUser);
        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<MethodResult<AppUser>> GetUserByEmailAsync(string email);
    }
}
