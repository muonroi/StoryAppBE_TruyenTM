using AutoMapper;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.GroupAndRoles
{
    /// <summary>
    /// Handler role queries
    /// </summary>
    public class RoleQueries : IRoleQueries
    {
        private readonly MuonRoiSocialNetworkDbContext _dbcontext;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <param name="mapper"></param>
        public RoleQueries(MuonRoiSocialNetworkDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }
        /// <summary>
        /// Get role by guid
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<MethodResult<RoleInitialBaseResponse>> GetRoleByIdAsync(long roleId)
        {
            MethodResult<RoleInitialBaseResponse> methodResult = new();
            if (_dbcontext == null || _dbcontext.AppRoles == null)
            {
                methodResult.Result = null;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            AppRole? roleInfo = await _dbcontext.AppRoles.FirstOrDefaultAsync(x => x.Id == roleId);
            if (roleInfo == null)
            {
                methodResult.Result = null;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            RoleInitialBaseResponse returnInfo = _mapper.Map<RoleInitialBaseResponse>(roleInfo);
            methodResult.Result = returnInfo;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// Get role by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> IsRoleByNameExistAsync(string name)
        {
            MethodResult<bool> methodResult = new();
            if (string.IsNullOrEmpty(name))
            {
                methodResult.Result = false;
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                return methodResult;
            }
            methodResult.Result = await _dbcontext.AppRoles.AnyAsync(x => x.Name == name);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
