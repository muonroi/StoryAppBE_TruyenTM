using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Roles;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Request;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.GroupAndRoles
{
    /// <summary>
    /// Request init role
    /// </summary>
    public class InitialRoleCommand : RoleInitialBaseRequest, IRequest<MethodResult<RoleInitialBaseResponse>>
    { }
    /// <summary>
    /// Handler command init role
    /// </summary>
    public class InitialRoleCommandHandler : BaseGroupAndRoleCommandHandler, IRequestHandler<InitialRoleCommand, MethodResult<RoleInitialBaseResponse>>
    {
        private readonly ILogger<InitialRoleCommandHandler> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="roleRepository"></param>
        /// <param name="groupRepository"></param>
        /// <param name="logger"></param>
        /// <param name="roleQueries"></param>
        /// <param name="groupQueries"></param>
        public InitialRoleCommandHandler(IMapper mapper, IConfiguration configuration, IRoleRepository roleRepository, IGroupRepository groupRepository, ILoggerFactory logger, IRoleQueries roleQueries, IGroupQueries groupQueries) : base(mapper, configuration, roleRepository, groupRepository, roleQueries, groupQueries)
        {
            _logger = logger.CreateLogger<InitialRoleCommandHandler>();
        }

        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<RoleInitialBaseResponse>> Handle(InitialRoleCommand request, CancellationToken cancellationToken)
        {
            MethodResult<RoleInitialBaseResponse> methodResult = new();
            try
            {
                #region Validation
                AppRole newRole = _mapper.Map<AppRole>(request);
                if (!newRole.IsValid())
                {
                    throw new CustomException(newRole.ErrorMessages);
                }
                #endregion

                #region Check role is not exist
                var isExistRole = await _roleQueries.IsRoleByNameExistAsync(request.Name);
                if (isExistRole.Result)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumRoleErrorCodes.ROL07C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.Name), request.Name) }
                    );
                    return methodResult;
                }
                #endregion

                #region Create Role
                newRole.NormalizedName = newRole.Name.ToUpper();
                await _roleRepository.ExecuteTransactionAsync(async () =>
                 {
                     _roleRepository.Add(newRole);
                     await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
                     var roleInfoResult = await _roleQueries.GetRoleByIdAsync(newRole.Id);
                     RoleInitialBaseResponse? roleInfo = roleInfoResult.Result;
                     methodResult.Result = roleInfo;
                     methodResult.StatusCode = StatusCodes.Status200OK;
                     return methodResult;
                 });

                #endregion

                return methodResult;
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(InitialRoleCommand) STEP CUSTOMEXCEPTION --> ID USER {ex} ---->");
#pragma warning disable CS8604
                methodResult.AddResultFromErrorList(ex?.ErrorMessages);
#pragma warning restore CS8604
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(InitialRoleCommand) STEP EXEPTION MESSAGE --> ID USER {ex} ---->");
                _logger.LogError($" -->(InitialRoleCommand) STEP EXEPTION STACK --> ID USER {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }

        }
    }
}
