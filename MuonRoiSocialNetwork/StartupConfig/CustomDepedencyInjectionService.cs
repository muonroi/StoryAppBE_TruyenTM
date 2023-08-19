using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Categories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.GroupAndRoles;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Token;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Auth;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.Extentions.Mail;
using MuonRoiSocialNetwork.Infrastructure.Queries.Auth;
using MuonRoiSocialNetwork.Infrastructure.Queries.Category;
using MuonRoiSocialNetwork.Infrastructure.Queries.Chapters;
using MuonRoiSocialNetwork.Infrastructure.Queries.GroupAndRoles;
using MuonRoiSocialNetwork.Infrastructure.Queries.Stories.Review;
using MuonRoiSocialNetwork.Infrastructure.Queries.Stories;
using MuonRoiSocialNetwork.Infrastructure.Queries.TagsAndTagInStories;
using MuonRoiSocialNetwork.Infrastructure.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Category;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Chapters;
using MuonRoiSocialNetwork.Infrastructure.Repositories.GroupAndRoles;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Stories.Reviews;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Stories;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Tag;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Token;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Users;
using MuonRoiSocialNetwork.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AutoMapper;

namespace MuonRoiSocialNetwork.StartupConfig
{
    internal static class CustomDepedencyInjectionService
    {
        public static IServiceCollection RegisterTransient(this IServiceCollection services)
        {
            return services;
        }
        public static IServiceCollection RegisterScoped(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, MailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserQueries, UserQueries>();
            services.AddScoped<IRefreshtokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IRoleQueries, RoleQueries>();
            services.AddScoped<IGroupQueries, GroupQueries>();
            services.AddScoped<IStoriesRepository, StoriesRepository>();
            services.AddScoped<IStoriesQueries, StoriesQueries>();
            services.AddScoped<ITagQueries, TagQueries>();
            services.AddScoped<ITagInStoriesQueries, TagInStoriesQueries>();
            services.AddScoped<ICategoryQueries, CategoryQueries>();
            services.AddScoped<IStoriesFavoriteRepository, StoryFavoriteRepository>();
            services.AddScoped<IStoriesFavoriteQueries, StoriesFavoriteQueries>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagInStoryRepository, TagInStoriesRepository>();
            services.AddScoped<IChapterQueries, ChapterQueries>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IReviewStoryRepository, ReviewStoryRepository>();
            services.AddScoped<IReviewStoryQueries, ReviewStoryQueries>();
            services.AddScoped<IRefreshTokenQueries, RefreshTokenQueries>();
            return services;
        }
        public static IServiceCollection RegisterSingleton(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(sp =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    CustomMapperConfiguration.DefineMapperConfiguration(cfg);
                });
                return config.CreateMapper();
            });
            return services;
        }
    }
}
