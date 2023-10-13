using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.BaseDbContext.Common;
using BaseConfig.Extentions.Image;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Common.Models.Notifications;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Stories
{
    /// <summary>
    /// Story notification queries
    /// </summary>
    public class StoryNotificationQueries : BaseQuery<StoryNotifications>, IStoryNotificationQueries
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="cache"></param>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="configuration"></param>
        public StoryNotificationQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IDistributedCache cache, IMapper mapper, IUserRepository userRepository, IConfiguration configuration) : base(dbContext, authContext, cache, mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        /// <summary>
        /// Get notification for user
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<MethodResult<PagingItemsDTO<NotificationModels>>> GetNotifycationByUserGuid(int pageIndex, int pageSize)
        {
            var methodResult = new MethodResult<PagingItemsDTO<NotificationModels>>();
            var isUserExist = await _userRepository.ExistUserByGuidAsync(Guid.Parse(_authContext.CurrentUserId));
            if (!isUserExist.IsOK)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            var notificationForUser = _queryable.AsNoTracking().Where(x => x.UserGuid == Guid.Parse(_authContext.CurrentUserId)).Select(x => x);
            if (notificationForUser == null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.AddApiErrorMessage(
                    nameof(EnumNotificationStoryErrorCodes.NT06),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumNotificationStoryErrorCodes.NT06), EnumNotificationStoryErrorCodes.NT06) }
                );
                return methodResult;
            }
            var notifcationPaging = await GetListPaging(notificationForUser, pageIndex, pageSize).ConfigureAwait(false);
            var resultNotification = _mapper.Map<List<NotificationModels>>(notifcationPaging.Items);
            for (int i = 0; i < resultNotification.Count; i++)
            {
                resultNotification[i].ImgUrl = HandlerImages.TakeLinkImage(_configuration, resultNotification[i].ImgUrl);
            }
            methodResult.Result = new PagingItemsDTO<NotificationModels>
            {
                Items = resultNotification,
                PagingInfo = notifcationPaging.PagingInfo
            };
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
