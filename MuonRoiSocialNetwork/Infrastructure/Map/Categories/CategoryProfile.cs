using AutoMapper;
using MuonRoiSocialNetwork.Application.Commands.Category;
using MuonRoiSocialNetwork.Common.Models.Category.Response;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;

namespace MuonRoiSocialNetwork.Infrastructure.Map.Categories
{
    /// <summary>
    /// Register category map
    /// </summary>
    public class CategoryProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CategoryProfile()
        {
            CreateMap<CategoryResponse, CategoryEntities>();
            CreateMap<CategoryEntities, CategoryResponse>();
            CreateMap<CreateCategoryCommand, CategoryEntities>();
            CreateMap<UpdateCategoryCommand, CategoryEntities>();
        }
    }
}
