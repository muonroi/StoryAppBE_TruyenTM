using AutoMapper;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Application.Commands.Stories;
using MuonRoiSocialNetwork.Common.Models.Stories.Request;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;

namespace MuonRoiSocialNetwork.Infrastructure.Map.Stories
{
    /// <summary>
    /// Register map
    /// </summary>
    public class StoriesProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoriesProfile()
        {
            CreateMap<Story, StoryModelResponse>();
            CreateMap<StoryModelRequest, Story>();
            CreateMap<CommentStoryCommand, StoryReview>();
            CreateMap<StoryReviewModelRequest, StoryReview>();
        }
    }
}
