using AutoMapper;
using MuonRoi.Social_Network.Roles;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Category;
using MuonRoiSocialNetwork.Application.Commands.Chapter;
using MuonRoiSocialNetwork.Application.Commands.GroupAndRoles;
using MuonRoiSocialNetwork.Application.Commands.Stories;
using MuonRoiSocialNetwork.Application.Commands.Tags;
using MuonRoiSocialNetwork.Application.Commands.Users;
using MuonRoiSocialNetwork.Common.Models.Category.Response;
using MuonRoiSocialNetwork.Common.Models.Chapter.Request;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response;
using MuonRoiSocialNetwork.Common.Models.Stories.Request;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Response;
using MuonRoiSocialNetwork.Common.Models.Tags.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Request;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Domains.DomainObjects.Groups;
using MuonRoiSocialNetwork.Infrastructure.Map.Chapter;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;
using ChapterEntites = MuonRoi.Social_Network.Chapters.Chapter;


namespace MuonRoiSocialNetwork.StartupConfig
{
    internal static class CustomMapperConfiguration
    {
        public static void DefineMapperConfiguration(IMapperConfigurationExpression configuration)
        {
            #region User
            configuration.CreateMap<AppUser, UserModelRequest>();
            configuration.CreateMap<CreateUserCommand, AppUser>();
            configuration.CreateMap<AppUser, UserModelResponse>();
            configuration.CreateMap<AppUser, UpdateInformationCommand>();
            configuration.CreateMap<UpdateInformationCommand, AppUser>();
            configuration.CreateMap<BaseUserResponse, AppUser>();
            configuration.CreateMap<AppUser, BaseUserResponse>();
            configuration.CreateMap<AppUser, ChangePasswordCommand>();
            configuration.CreateMap<AppUser, ChangeStatusCommand>();
            configuration.CreateMap<ChangeStatusCommand, AppUser>();
            configuration.CreateMap<ChangePasswordCommand, AppUser>();
            configuration.CreateMap<BaseUserResponse, UserModelResponse>();
            configuration.CreateMap<UserModelRequest, CreateUserCommand>();
            configuration.CreateMap<CreateUserCommand, UserModelRequest>();
            #endregion
            #region Group & role
            configuration.CreateMap<InitialRoleCommand, AppRole>();
            configuration.CreateMap<AppRole, RoleInitialBaseResponse>();
            configuration.CreateMap<InitialGroupCommand, GroupUserMember>();
            configuration.CreateMap<GroupUserMember, GroupInitialBaseResponse>();
            #endregion
            #region Story
            configuration.CreateMap<Story, StoryModelResponse>();
            configuration.CreateMap<StoryModelRequest, Story>();
            configuration.CreateMap<CommentStoryCommand, StoryReview>();
            configuration.CreateMap<StoryReviewModelRequest, StoryReview>();
            #endregion
            #region Tag
            configuration.CreateMap<TagModelResponse, Tag>();
            configuration.CreateMap<Tag, TagModelResponse>();
            configuration.CreateMap<TagInStory, TagInStoriesModelResponse>();
            configuration.CreateMap<TagInStoriesModelResponse, TagInStory>();
            configuration.CreateMap<CreateTagCommand, Tag>();
            configuration.CreateMap<UpdateTagCommand, Tag>();
            configuration.CreateMap<CreateTagInStoryCommand, TagInStory>();
            configuration.CreateMap<UpdateTagInStoryCommand, TagInStory>();
            #endregion
            #region Category
            configuration.CreateMap<CategoryResponse, CategoryEntities>();
            configuration.CreateMap<CategoryEntities, CategoryResponse>();
            configuration.CreateMap<CreateCategoryCommand, CategoryEntities>();
            configuration.CreateMap<UpdateCategoryCommand, CategoryEntities>();
            #endregion
            #region Chapter
            configuration.CreateMap<ChapterModelResponse, ChapterEntites>();
            configuration.CreateMap<ChapterEntites, ChapterModelResponse>();
            configuration.CreateMap<CreateChapterCommand, ChapterEntites>();
            configuration.CreateMap<UpdateChapterCommand, ChapterEntites>();
            configuration.CreateMap<ChapterModelRequest, ChapterEntites>();
            #endregion
        }
    }
}
