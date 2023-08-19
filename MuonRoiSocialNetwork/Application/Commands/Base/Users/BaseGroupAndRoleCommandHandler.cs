using AutoMapper;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;

namespace MuonRoiSocialNetwork.Application.Commands.Base.Users
{
    /// <summary>
    /// Base group and role command
    /// </summary>
    public class BaseGroupAndRoleCommandHandler
    {

        /// <summary>
        /// property _mapper
        /// </summary>
        protected readonly IMapper _mapper;
        /// <summary>
        /// property get config
        /// </summary>
        protected readonly IConfiguration _configuration;
        /// <summary>
        /// property _roleRepository
        /// </summary>
        protected readonly IRoleRepository _roleRepository;
        /// <summary>
        /// property _groupRepository
        /// </summary>
        protected readonly IGroupRepository _groupRepository;
        /// <summary>
        /// property _groupQueries
        /// </summary>
        protected readonly IGroupQueries _groupQueries;
        /// <summary>
        /// property _groupRepository
        /// </summary>
        protected readonly IRoleQueries _roleQueries;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="roleRepository"></param>
        /// <param name="groupRepository"></param>
        /// <param name="roleQueries"></param>
        /// <param name="groupQueries"></param>
        protected BaseGroupAndRoleCommandHandler(IMapper mapper, IConfiguration configuration, IRoleRepository roleRepository, IGroupRepository groupRepository, IRoleQueries roleQueries, IGroupQueries groupQueries)
        {
            _mapper = mapper;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _groupRepository = groupRepository;
            _roleQueries = roleQueries;
            _groupQueries = groupQueries;
        }
    }
}
