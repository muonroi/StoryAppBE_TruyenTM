using MuonRoi.Social_Network.Tags;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;

namespace MuonRoiSocialNetwork.Common.Models.Tuples
{
    public class CustomTupleItemOfStory : Tuple<bool, List<Tag>, List<TagInStory>, List<CategoryEntities>>
    {
        public CustomTupleItemOfStory(bool status, List<Tag> tagInfo, List<TagInStory> tagsInStoriesInfo, List<CategoryEntities> categoriesInfo) : base(status, tagInfo, tagsInStoriesInfo, categoriesInfo)
        { }

        public bool Status { get { return this.Item1; } }
        public List<Tag> Tags { get { return this.Item2; } }
        public List<TagInStory> TagsInStories { get { return this.Item3; } }
        public List<CategoryEntities> Categories { get { return this.Item4; } }

    }
}
