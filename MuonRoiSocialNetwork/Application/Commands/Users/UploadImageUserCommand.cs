using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Extentions.Image;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;

namespace MuonRoiSocialNetwork.Application.Commands.Users
{
    /// <summary>
    /// request
    /// </summary>
    public class UploadImageUserCommand : IRequest<MethodResult<UserModelResponse>>
    {
        /// <summary>
        /// image file
        /// </summary>
        public IFormFile? ImageSrc { get; set; }

    }
    /// <summary>
    /// Handler
    /// </summary>
    public class UploadImageUserCommandHandler : BaseUserCommandHandler, IRequestHandler<UploadImageUserCommand, MethodResult<UserModelResponse>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userQueries"></param>
        /// <param name="userRepository"></param>
        /// <param name="authContext"></param>
        public UploadImageUserCommandHandler(IMapper mapper, IConfiguration configuration, IUserQueries userQueries, IUserRepository userRepository, AuthContext authContext) : base(mapper, configuration, userQueries, userRepository, authContext)
        { }
        /// <summary>
        /// Function
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<UserModelResponse>> Handle(UploadImageUserCommand request, CancellationToken cancellationToken)
        {
            var methodResult = new MethodResult<UserModelResponse>();
            #region Get user info
            var userInfo = await _userQueries.GetByGuidAsync(Guid.Parse(_authContext.CurrentUserId));
            if (userInfo.Result is null || !userInfo.IsOK)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumUserErrorCodes.USR02C),
                    new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                );
                methodResult.Result = null;
                return methodResult;
            }
            #endregion
            #region upload avatar
            if (request.ImageSrc != null)
            {
                var result = await HandlerImages.UploadImageToAwsAsync(_configuration, request.ImageSrc);
                if (!result.Any() || !result.Keys.Any())
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC41C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername ?? "") }
                    );
                    return methodResult;
                }
                userInfo.Result.Avatar = result.Keys.FirstOrDefault() ?? "";
                #region Update user info
                await _userRepository.UpdateUserAsync(userInfo.Result);
                var userResult = _mapper.Map<UserModelResponse>(userInfo.Result);
                userResult.Avatar = HandlerImages.TakeLinkImage(_configuration, userInfo.Result.Avatar);
                methodResult.Result = userResult;
                methodResult.StatusCode = StatusCodes.Status200OK;
                return methodResult;
                #endregion
            }
            #endregion
            return methodResult;
        }
    }
}
