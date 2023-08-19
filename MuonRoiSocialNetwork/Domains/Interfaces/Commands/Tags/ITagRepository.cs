using BaseConfig.BaseDbContext.Common;
using TagEntities = MuonRoi.Social_Network.Tags.Tag;
namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags
{
    /// <summary>
    /// Define interface tag 
    /// </summary>
    public interface ITagRepository : IRepository<TagEntities>
    {
    }
}
