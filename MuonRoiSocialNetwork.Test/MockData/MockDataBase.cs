using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Infrastructure;
using MuonRoiSocialNetwork.Infrastructure.Map.Users;
using MuonRoiSocialNetwork.Infrastructure.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Users;
using MuonRoiSocialNetwork.Infrastructure.Services;

namespace MuonRoiSocialNetwork.Test
{
    public class MockDataBase
    {
        public IConfiguration _configBase;
        public Mock<IEmailService> _emailServiceBase;
        public UserRepository _userRepoBase;
        public UserQueries _userQueriesBase;
        public MuonRoiSocialNetworkDbContext _userdbContext;
        public MapperConfiguration _mapperConfiguration;
        public IMapper _maperBase;
        public Mock<IMediator> _mediator;
        public Mock<ILoggerFactory> _loggerBase;
        public Mock<IDistributedCache> _cacheBase;
        public MockDataBase()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                               .AddEntityFrameworkInMemoryDatabase()
                               .BuildServiceProvider();
            DbContextOptionsBuilder<MuonRoiSocialNetworkDbContext> builder = new DbContextOptionsBuilder<MuonRoiSocialNetworkDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .UseInternalServiceProvider(serviceProvider);
            _mediator = new Mock<IMediator>();
            _loggerBase = new Mock<ILoggerFactory>();
            _userdbContext = new MuonRoiSocialNetworkDbContext(builder.Options, _mediator.Object);
            _userRepoBase = new UserRepository(_userdbContext);
            _configBase = new ConfigurationBuilder().AddJsonFile($"{NameAppSetting.appsettings}.json", optional: false).Build();
            _emailServiceBase = new Mock<IEmailService>();
            _cacheBase = new Mock<IDistributedCache>();
            _mapperConfiguration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserProfile>();
            });
            _maperBase = _mapperConfiguration.CreateMapper();
            _userQueriesBase = new UserQueries(_userdbContext, _maperBase, _configBase);

            #region InitData User
            AppUser user1 = new()
            {
                Id = new Guid("6A2A1E72-7C06-47AC-861C-75046C75A588"),
                Name = "test",
                Surname = "test2",
                Email = "leanhphi1706@gmail.com",
                PhoneNumber = "1234567890",
                BirthDate = new DateTime(2002, 06, 17),
                UserName = "test1",
                PasswordHash = "1234567Az*99",
                Avatar = "av1",
                Salt = "1231231221312"

            };
            AppUser user2 = new()
            {
                Id = new Guid("00F13A88-54B8-4A07-9AFB-636C2C93C200"),
                Name = "test2",
                Surname = "test3",
                Email = "leanhphi1707@gmail.com",
                PhoneNumber = "1234567890",
                BirthDate = new DateTime(2002, 06, 18),
                UserName = "test2",
                PasswordHash = "1234567Az*99",
                Avatar = "av2",
                Salt = "1231231221312",
                AccessFailedCount = 0,
            };
            AppUser user3 = new()
            {
                Id = new Guid("00F13A88-54B8-4A07-9AFB-636C2C93C202"),
                Name = "test3",
                Surname = "test3",
                Email = "leanhphi1707@gmail.com",
                PhoneNumber = "1234567890",
                BirthDate = new DateTime(2002, 06, 18),
                UserName = "testlocked",
                PasswordHash = "1234567Az*99",
                Avatar = "av2",
                Status = EnumAccountStatus.Locked
            };
            if (!_userdbContext.AppUsers.Any(t => t.Id.Equals(new Guid("6A2A1E72-7C06-47AC-861C-75046C75A588"))))
                _userdbContext.AppUsers.Add(user1);
            if (!_userdbContext.AppUsers.Any(t => t.Id.Equals(new Guid("00F13A88-54B8-4A07-9AFB-636C2C93C200"))))
                _userdbContext.AppUsers.Add(user2);
            if (!_userdbContext.AppUsers.Any(t => t.Id.Equals(new Guid("00F13A88-54B8-4A07-9AFB-636C2C93C202"))))
                _userdbContext.AppUsers.Add(user3);
            _userdbContext.SaveChanges();
            #endregion
        }
    }
}