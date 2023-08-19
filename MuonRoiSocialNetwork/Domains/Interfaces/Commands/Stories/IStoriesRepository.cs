using BaseConfig.BaseDbContext.Common;
using MuonRoi.Social_Network.Storys;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories
{
    /// <summary>
    /// Interface of stories (repository)
    /// </summary>
    public interface IStoriesRepository : IRepository<Story>
    {
        /// <summary>
        /// Update single entry
        /// </summary>
        /// <param name="entityUpdate"></param>
        /// <param name="columName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<int> UpdateSingleEntry(Story entityUpdate, string columName, object value);
    }
}
