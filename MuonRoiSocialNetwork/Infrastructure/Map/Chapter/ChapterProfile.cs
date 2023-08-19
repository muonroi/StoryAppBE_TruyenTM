using AutoMapper;
using ChapterEntites = MuonRoi.Social_Network.Chapters.Chapter;
using MuonRoiSocialNetwork.Application.Commands.Chapter;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;
using MuonRoiSocialNetwork.Common.Models.Chapter.Request;

namespace MuonRoiSocialNetwork.Infrastructure.Map.Chapter
{
    /// <summary>
    /// Register map
    /// </summary>
    public class ChapterProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChapterProfile()
        {
            CreateMap<ChapterModelResponse, ChapterEntites>();
            CreateMap<ChapterEntites, ChapterModelResponse>();
            CreateMap<CreateChapterCommand, ChapterEntites>();
            CreateMap<UpdateChapterCommand, ChapterEntites>();
            CreateMap<ChapterModelRequest, ChapterEntites>();
        }
    }
}
