using AutoMapper;
using MuonRoi.Social_Network.Roles;
using MuonRoiSocialNetwork.Application.Commands.GroupAndRoles;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;

namespace MuonRoiSocialNetwork.Infrastructure.Map.GroupAndRoles
{
    /// <summary>
    /// Register map
    /// </summary>
    public class GroupAndRoleProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupAndRoleProfile()
        {
            CreateMap<InitialRoleCommand, AppRole>();
            CreateMap<AppRole, RoleInitialBaseResponse>();
            CreateMap<InitialGroupCommand, GroupUserMember>();
            CreateMap<GroupUserMember, GroupInitialBaseResponse>();
        }
    }
}