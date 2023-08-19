using AutoMapper;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Roles;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;
using Microsoft.EntityFrameworkCore;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Extentions.Image;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Users
{
    /// <summary>
    /// Handle user querys
    /// </summary>
    public class UserQueries : IUserQueries
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly MuonRoiSocialNetworkDbContext _dbcontext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        public UserQueries(MuonRoiSocialNetworkDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbcontext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        /// <summary>
        /// Handle get user by guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MethodResult<AppUser>> GetByGuidAsync(Guid id)
        {
            MethodResult<AppUser> methodResult = new();
            var existUser = await _dbcontext.AppUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted);
            if (existUser is null)
            {
                methodResult.Result = null;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            methodResult.Result = existUser;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// handle get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<MethodResult<AppUser>> GetByUsernameAsync(string username)
        {

            MethodResult<AppUser> methodResult = new();
            var existUser = await _dbcontext.AppUsers
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.UserName.Equals(username) && !x.IsDeleted); ;
            if (existUser is null)
            {
                methodResult.Result = null;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            methodResult.Result = existUser;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// handle get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>UserModel</returns>
        public async Task<MethodResult<BaseUserResponse>> GetUserModelBynameAsync(string username)
        {
            MethodResult<BaseUserResponse> methodResult = new();
            AppUser? appUser = await _dbcontext.AppUsers
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.UserName.Equals(username) && !x.IsDeleted);
            if (appUser == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumUserErrorCodes.USR02C),
                    new[] { nameof(EnumUserErrorCodes.USR02C), nameof(EnumUserErrorCodes.USR02C) ?? "" }
                );
                return methodResult;
            }
            List<AppRole>? roles = await _dbcontext.AppRoles.Where(x => !x.IsDeleted).ToListAsync();
            List<GroupUserMember>? groups = await _dbcontext.GroupUserMembers.Where(x => !x.IsDeleted).ToListAsync();
            if (!roles.Any() || !groups.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumUserErrorCodes.USRC46C),
                    new[] { nameof(EnumUserErrorCodes.USRC46C), nameof(EnumUserErrorCodes.USRC46C) ?? "" }
                );
                return methodResult;
            }
            var group = await _dbcontext.GroupUserMembers.FirstOrDefaultAsync(x => x.Id == appUser.GroupId);
            List<string> userRole = new();
            if (group != null)
            {
                var roleIds = group.Roles.Split(',').ToList();
                foreach (var roleId in roleIds)
                {
                    var tempRole = await _dbcontext.AppRoles.FirstOrDefaultAsync(x => x.Id == appUser.GroupId);
                    if (tempRole != null)
                    {
                        userRole.Add(tempRole.Name);
                    }
                }
            }
            methodResult.Result = _mapper.Map<UserModelResponse>(appUser);
            methodResult.Result.RoleName = string.Join(",", userRole);
            methodResult.Result.GroupName = group?.GroupName ?? "";
            methodResult.Result.CreateDate = DateTimeExtensions.TimeStampToDateTime(appUser.CreatedDateTS.GetValueOrDefault()).AddHours(SettingUserDefault.Instance.hourAsia);
            methodResult.Result.UpdateDate = DateTimeExtensions.TimeStampToDateTime(appUser.UpdatedDateTS.GetValueOrDefault()).AddHours(SettingUserDefault.Instance.hourAsia);
            methodResult.Result.Avatar = HandlerImages.TakeLinkImage(_configuration, methodResult.Result.Avatar ?? "");
            return methodResult;
        }
        /// <summary>
        /// Handle get user by guid
        /// </summary>
        /// <param name="guidUser"></param>
        /// <returns></returns>
        public async Task<MethodResult<BaseUserResponse>> GetUserModelByGuidAsync(Guid guidUser)
        {
            MethodResult<BaseUserResponse> methodResult = new();
            AppUser? appUser = await _dbcontext.AppUsers
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.Id.Equals(guidUser) && !x.IsDeleted);
            if (appUser == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumUserErrorCodes.USR02C),
                    new[] { nameof(EnumUserErrorCodes.USR02C), nameof(EnumUserErrorCodes.USR02C) ?? "" }
                );
                return methodResult;
            }
            List<AppRole>? roles = await _dbcontext.AppRoles.Where(x => !x.IsDeleted).ToListAsync();
            List<GroupUserMember>? groups = await _dbcontext.GroupUserMembers.Where(x => !x.IsDeleted).ToListAsync();
            if (!roles.Any() || !groups.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumUserErrorCodes.USRC46C),
                    new[] { nameof(EnumUserErrorCodes.USRC46C), nameof(EnumUserErrorCodes.USRC46C) ?? "" }
                );
                return methodResult;
            }
            var group = await _dbcontext.GroupUserMembers.FirstOrDefaultAsync(x => x.Id == appUser.GroupId);
            List<string> userRole = new();
            if (group != null)
            {
                var roleIds = group.Roles.Split(',').ToList();
                foreach (var roleId in roleIds)
                {
                    var tempRole = await _dbcontext.AppRoles.FirstOrDefaultAsync(x => x.Id == appUser.GroupId);
                    if (tempRole != null)
                    {
                        userRole.Add(tempRole.Name);
                    }
                }
            }
            methodResult.Result = _mapper.Map<UserModelResponse>(appUser);
            methodResult.Result.RoleName = string.Join(",", userRole);
            methodResult.Result.GroupName = group?.GroupName;
            methodResult.Result.CreateDate = DateTimeExtensions.TimeStampToDateTime(appUser.CreatedDateTS.GetValueOrDefault()).AddHours(SettingUserDefault.Instance.hourAsia);
            methodResult.Result.UpdateDate = DateTimeExtensions.TimeStampToDateTime(appUser.UpdatedDateTS.GetValueOrDefault()).AddHours(SettingUserDefault.Instance.hourAsia);
            methodResult.Result.Avatar = HandlerImages.TakeLinkImage(_configuration, methodResult.Result.Avatar ?? "");
            return methodResult;
        }
        /// <summary>
        /// Get user by email handle
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<MethodResult<AppUser>> GetUserByEmailAsync(string email)
        {
            MethodResult<AppUser> methodResult = new();
            var existUser = await _dbcontext.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted)
                .ConfigureAwait(false);
            if (existUser is null)
            {
                methodResult.Result = null;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            methodResult.Result = existUser;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
