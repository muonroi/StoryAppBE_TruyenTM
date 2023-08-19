using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoiSocialNetwork.Common.Models.Category.Response;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category
{
    /// <summary>
    /// Define category interface 
    /// </summary>
    public interface ICategoryQueries : IQueries<CategoryEntities>
    {
        /// <summary>
        /// Get all category
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<CategoryResponse>>> GetAllCategory(int pageIndex = 1, int pageSize = 10);
        /// <summary>
        /// Get special category by id
        /// </summary>
        /// <returns></returns>
        Task<MethodResult<CategoryResponse>> GetCategoryById(int categoryId);
        /// <summary>
        /// Get special category by name
        /// </summary>
        /// <returns></returns>
        Task<MethodResult<bool>> GetCategoryByName(string nameCategory);
    }
}
