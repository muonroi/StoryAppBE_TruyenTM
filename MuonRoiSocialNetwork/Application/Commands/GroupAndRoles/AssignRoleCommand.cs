using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Roles;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.GroupAndRoles
{
    /// <summary>
    /// Request assign role to group
    /// </summary>
    public class AssignRoleCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Group is exist (required)
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Role id exist (required)
        /// </summary>
        public List<string> RoleId { get; set; }
    }
    /// <summary>
    /// Handler assign role
    /// </summary>
    public class AssignRoleCommandHandler : BaseGroupAndRoleCommandHandler, IRequestHandler<AssignRoleCommand, MethodResult<bool>>
    {
        private readonly ILogger<AssignRoleCommandHandler> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="roleRepository"></param>
        /// <param name="groupRepository"></param>
        /// <param name="roleQueries"></param>
        /// <param name="logger"></param>
        /// <param name="groupQueries"></param>
        public AssignRoleCommandHandler(IMapper mapper, IConfiguration configuration, IRoleRepository roleRepository, IGroupRepository groupRepository, IRoleQueries roleQueries, ILoggerFactory logger, IGroupQueries groupQueries) : base(mapper, configuration, roleRepository, groupRepository, roleQueries, groupQueries)
        {
            _logger = logger.CreateLogger<AssignRoleCommandHandler>();
        }
        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumRoleErrorCodes.ROL02C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.GroupId), EnumRoleErrorCodes.ROL02C) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #region Validation
                foreach (var item in request.RoleId)
                {
                    var roleResult = await _roleQueries.GetRoleByIdAsync(int.Parse(item)).ConfigureAwait(false);
                    RoleInitialBaseResponse existRole = roleResult.Result;
                    if (existRole is null)
                    {
                        methodResult.StatusCode = StatusCodes.Status400BadRequest;
                        methodResult.AddApiErrorMessage(
                            nameof(EnumRoleErrorCodes.ROL02C),
                            new[] { Helpers.GenerateErrorResult(nameof(request.GroupId), request.GroupId) }
                        );
                        methodResult.Result = false;
                        return methodResult;
                    }
                }

                #endregion

                #region Check is exist group and Assign role
                GroupUserMember existGroup = await _groupRepository.GetByIdAsync(request.GroupId);
                if (existGroup is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumGroupErrorCodes.GRP04C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.GroupId), request.GroupId) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                existGroup.Roles = string.Join(",", request.RoleId);
                await _groupRepository.ExecuteTransactionAsync(async () =>
                {
                    _groupRepository.Update(existGroup);
                    await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
                    methodResult.Result = true;
                    return methodResult;
                });

                #endregion

                methodResult.Result = true;
                return methodResult;
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(AssignRoleCommand) STEP CUSTOMEXCEPTION --> ID USER {ex} ---->");
#pragma warning disable CS8604
                methodResult.AddResultFromErrorList(ex?.ErrorMessages);
#pragma warning restore CS8604
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(AssignRoleCommand) STEP EXEPTION MESSAGE --> ID USER {ex} ---->");
                _logger.LogError($" -->(AssignRoleCommand) STEP EXEPTION STACK --> ID USER {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }
        }
    }
}
