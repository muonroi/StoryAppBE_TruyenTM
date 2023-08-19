using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.GroupName;

namespace MuonRoiSocialNetwork.Common.Settings.StorySettings
{
    public sealed class StorySettingDefault
    {
        public StorySettingDefault()
        {

        }
        public static StorySettingDefault Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly StorySettingDefault instance = new();
        }
        /// <summary>
        /// Key queryable comments of story
        /// </summary>
        public readonly string keyQueryableResponseComments = "QueryableResponseComments-{0}";
        /// <summary>
        /// Key queryable tag info when search
        /// </summary>
        public readonly string keyQueryableResponseTags = "QueryableResponseTags";
        /// <summary>
        /// Key queryable category info when search
        /// </summary>
        public readonly string keyQueryableResponseCategorys = "keyQueryableResponseCategorys";
        /// <summary>
        /// Key tag info when search
        /// </summary>
        public readonly string keyModelResponseTags = "ModelResponseTags";
        /// <summary>
        /// Key tag info when search
        /// </summary>
        public readonly string keyModelResponseTagInStory = "ModelResponseTagInStory";
        /// <summary>
        /// Key category info when search
        /// </summary>
        public readonly string keyModelResponseCategorys = "ModelResponseCategorys";

        /// <summary>
        /// Key chapter info when search
        /// </summary>
        public readonly string keyModelResponseChapters = "ModelResponseChapters-{0}";
        /// <summary>
        /// Key total chapter for storyId
        /// </summary>
        public readonly string keyModelResponseTotalChapters = "ModelResponseTotalChapters-{0}";
        /// <summary>
        /// Key total chapter all
        /// </summary>
        public readonly string keyModelResponseTotalAllStories = "ModelResponseTotalAllStories-{0}";
        /// <summary>
        /// Life time expiration
        /// </summary>
        public readonly TimeSpan expirationTimeModelAllStories = TimeSpan.FromDays(1);
        /// <summary>
        /// Life time slidingExpiration
        /// </summary>
        public readonly TimeSpan slidingExpirationModelAllStories = TimeSpan.FromDays(1);
        /// <summary>
        /// Life time expiration
        /// </summary>
        public readonly TimeSpan expirationTimeLogin = TimeSpan.FromMinutes(120);
        /// <summary>
        /// Life time slidingExpiration
        /// </summary>
        public readonly TimeSpan slidingExpirationLogin = TimeSpan.FromMinutes(125);
    }
}
