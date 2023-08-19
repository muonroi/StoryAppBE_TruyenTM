using AutoMapper;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Application.Commands.Tags;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Response;
using MuonRoiSocialNetwork.Common.Models.Tags.Response;

namespace MuonRoiSocialNetwork.Infrastructure.Map.TagAndTagInStories
{
    /// <summary>
    /// Register map
    /// </summary>
    public class TagProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TagProfile()
        {
            CreateMap<TagModelResponse, Tag>();
            CreateMap<Tag, TagModelResponse>();
            CreateMap<TagInStory, TagInStoriesModelResponse>();
            CreateMap<TagInStoriesModelResponse, TagInStory>();
            CreateMap<CreateTagCommand, Tag>();
            CreateMap<UpdateTagCommand, Tag>();
            CreateMap<CreateTagInStoryCommand, TagInStory>();
            CreateMap<UpdateTagInStoryCommand, TagInStory>();
        }
    }
}
