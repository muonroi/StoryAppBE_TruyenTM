using AutoMapper;
using BaseConfig.BaseDbContext.BaseRepository;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Infrashtructure;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Categories;
using CategoryEntites = MuonRoi.Social_Network.Categories.Category;
namespace MuonRoiSocialNetwork.Infrastructure.Repositories.Category
{
    /// <summary>
    /// CategoryRepository
    /// </summary>
    public class CategoryRepository : BaseRepository<CategoryEntites>, ICategoryRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="mapper"></param>
        public CategoryRepository(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IMapper mapper) : base(dbContext, authContext)
        {
        }
    }
}
