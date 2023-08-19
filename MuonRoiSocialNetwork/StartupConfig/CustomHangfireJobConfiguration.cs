using Hangfire;
using MuonRoiSocialNetwork.Infrastructure.Queries.Stories;

namespace MuonRoiSocialNetwork.StartupConfig
{
    internal static class CustomHangfireJobConfiguration
    {
        public static void RegisterJobs()
        {
            RecurringJob.AddOrUpdate<StoriesQueries>("get-all-stories", x => x.ExecuteAsyncPrepareReferencesStoriesTableJob(), "0 0 * * *");
        }
    }
}
