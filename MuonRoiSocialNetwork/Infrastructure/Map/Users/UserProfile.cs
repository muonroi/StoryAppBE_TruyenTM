using AutoMapper;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Request;
using MuonRoiSocialNetwork.Common.Models.Users.Response;

namespace MuonRoiSocialNetwork.Infrastructure.Map.Users
{
    /// <summary>
    /// Register map
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserProfile()
        {
            CreateMap<AppUser, UserModelRequest>();
            CreateMap<CreateUserCommand, AppUser>();
            CreateMap<AppUser, UserModelResponse>();
            CreateMap<AppUser, UpdateInformationCommand>();
            CreateMap<UpdateInformationCommand, AppUser>();
            CreateMap<BaseUserResponse, AppUser>();
            CreateMap<AppUser, BaseUserResponse>();
            CreateMap<AppUser, ChangePasswordCommand>();
            CreateMap<AppUser, ChangeStatusCommand>();
            CreateMap<ChangeStatusCommand, AppUser>();
            CreateMap<ChangePasswordCommand, AppUser>();
            CreateMap<BaseUserResponse, UserModelResponse>();
            CreateMap<UserModelRequest, CreateUserCommand>();
            CreateMap<CreateUserCommand, UserModelRequest>();
        }
    }
}
