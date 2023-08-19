using BaseConfig.BaseDbContext.Common;
using TagInStoriesEntities = MuonRoi.Social_Network.Tags.TagInStory;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags
{
    /// <summary>
    /// Define tag in story interface
    /// </summary>
    public interface ITagInStoryRepository : IRepository<TagInStoriesEntities>
    {
    }
}
