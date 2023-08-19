using BaseConfig.BaseDbContext.Common;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories
{
    /// <summary>
    /// Declare interface repository
    /// </summary>
    public interface IStoriesFavoriteRepository : IRepository<StoryFavorite>
    {
    }
}
