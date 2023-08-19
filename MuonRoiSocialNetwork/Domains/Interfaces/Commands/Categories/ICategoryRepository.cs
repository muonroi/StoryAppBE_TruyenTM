using BaseConfig.BaseDbContext.Common;
using CategoryEntites = MuonRoi.Social_Network.Categories.Category;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Categories
{
    /// <summary>
    /// Declare interface category
    /// </summary>
    public interface ICategoryRepository : IRepository<CategoryEntites>
    {
    }
}
