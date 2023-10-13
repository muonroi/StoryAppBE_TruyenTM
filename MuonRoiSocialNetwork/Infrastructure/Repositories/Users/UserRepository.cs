using MuonRoi.Social_Network.Users;
using Microsoft.EntityFrameworkCore;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Extentions.String;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using System.Data;
using BaseConfig.MethodResult;

namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Users
{
    /// <summary>
    /// Handler user
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly MuonRoiSocialNetworkDbContext _dbcontext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public UserRepository(MuonRoiSocialNetworkDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        /// <summary>
        /// Handle check user is exist? by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> ExistUserByUsernameAsync(string username)
        {
            MethodResult<bool> methodResult = new()
            {
                Result = await _dbcontext.AppUsers.AsNoTracking()
                 .AnyAsync(x => x.UserName.Equals(username))
                 .ConfigureAwait(false),
                StatusCode = StatusCodes.Status200OK
            };
            return methodResult;
        }
        /// <summary>
        /// Handle check user is exist ? by guid
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> ExistUserByGuidAsync(Guid userGuid)
        {
            MethodResult<bool> methodResult = new()
            {
                Result = await _dbcontext.AppUsers
                .AsNoTracking()
                .AnyAsync(x => x.Id.Equals(userGuid))
                .ConfigureAwait(false),
                StatusCode = StatusCodes.Status200OK
            };
            return methodResult;
        }
        /// <summary>
        /// Handle create new user no role
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public async Task<MethodResult<int>> CreateNewUserAsync(AppUser newUser)
        {
            MethodResult<int> methodResult = new();
            try
            {
                StringManagers.WithRegex(newUser.Name ?? "");
                StringManagers.WithRegex(newUser.Surname ?? "");
                StringManagers.WithRegex(newUser.Address ?? "");
                newUser.Status = EnumAccountStatus.UnConfirm;
                newUser.BirthDate.IsValidDateTime();
                newUser.Avatar ??= newUser.Avatar ?? "".Trim();
                DateTime utcNow = DateTime.UtcNow;
                newUser.CreatedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
                newUser.UpdatedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
                _dbcontext.Add(newUser);
                methodResult.Result = await _dbcontext.SaveChangesAsync();
                methodResult.StatusCode = StatusCodes.Status200OK;
                return methodResult;
            }
            catch
            {
                methodResult.Result = -1;
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }

        }
        /// <summary>
        /// Handle confirmed email
        /// </summary>
        /// <param name="checkUser"></param>
        /// <returns></returns>
        public async Task<MethodResult<int>> ConfirmedEmail(AppUser checkUser)
        {
            MethodResult<int> methodResult = new();
            try
            {
                checkUser.EmailConfirmed = true;
                checkUser.Status = EnumAccountStatus.Confirmed;
                DateTime utcNow = DateTime.UtcNow;
                checkUser.UpdatedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
                _dbcontext.AppUsers.Update(checkUser);
                methodResult.Result = await _dbcontext.SaveChangesAsync();
                methodResult.StatusCode = StatusCodes.Status200OK;
                return methodResult;
            }
            catch
            {
                methodResult.Result = -1;
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }

        }
        /// <summary>
        /// Handle update info user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="salt"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(AppUser user, string? salt, string? passwordHash)
        {
            try
            {
                using var context = _dbcontext;
                DateTime utcNow = DateTime.UtcNow;
                user.UpdatedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
                if (salt is not null && passwordHash is not null)
                {
                    user.Salt = salt;
                    user.PasswordHash = passwordHash;
                }
                _dbcontext.AppUsers.Update(user);
                await _dbcontext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        /// <summary>
        /// Handle delete user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<MethodResult<int>> DeleteUserAsync(Guid userGuid)
        {
            MethodResult<int> methodResult = new();
            try
            {
                AppUser? userDelete = await _dbcontext.AppUsers.Where(x => x.Id.Equals(userGuid) && !x.IsDeleted).FirstOrDefaultAsync();
                if (userDelete is null)
                {
                    methodResult.Result = -1;
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    return methodResult;
                }
                DateTime utcNow = DateTime.UtcNow;
                userDelete.DeletedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
                userDelete.IsDeleted = true;
                _dbcontext.AppUsers.Update(userDelete);
                methodResult.Result = await _dbcontext.SaveChangesAsync();
                methodResult.StatusCode = StatusCodes.Status200OK;
                return methodResult;
            }
            catch
            {
                methodResult.Result = -1;
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }

        }
        /// <summary>
        /// Handle change password
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="salt"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> UpdatePassworAsync(Guid userGuid, string? salt, string? passwordHash)
        {
            MethodResult<bool> methodResult = new();
            AppUser? appUser = await _dbcontext.AppUsers.FirstOrDefaultAsync(x => x.Id.Equals(userGuid)).ConfigureAwait(false);
            if (appUser is null || salt is null || passwordHash is null)
            {
                methodResult.Result = false;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            appUser.UpdatedDateTS = DateTime.UtcNow.GetTimeStamp(includedTimeValue: true);
            appUser.Salt = salt;
            appUser.PasswordHash = passwordHash;
            appUser.AccountStatus = EnumAccountStatus.None;
            _dbcontext.AppUsers.Update(appUser);
            int resultUpdate = await _dbcontext.SaveChangesAsync();
            if (resultUpdate <= 0)
            {
                methodResult.Result = false;
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            methodResult.Result = true;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult; ;
        }
    }
}
