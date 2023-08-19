using AutoMapper;
using Moq;
using Microsoft.Extensions.Configuration;
using MuonRoiSocialNetwork.Test;
using MuonRoiSocialNetwork.Application.Commands.Users;
using Shouldly;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Users;
using BaseConfig.EntityObject.Entity;
using Microsoft.AspNetCore.Http;
using MuonRoiSocialNetwork.Infrastructure.Services;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using BaseConfig.Extentions.ObjectHandle;
using Microsoft.Extensions.Logging;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Users;
using MuonRoiSocialNetwork.Infrastructure.Queries.Users;

namespace TestProject1
{
    public class CreateUserCommandTest
    {
        private readonly MockDataBase _baseData = new();
        private readonly IMapper _mapper;
        public readonly UserRepository _user;
        public readonly UserQueries _userQueries;
        private readonly IConfiguration _config;
        private readonly Mock<IEmailService> _mail;
        private readonly ILoggerFactory _logger;
        public CreateUserCommandTest()
        {
            _user = _baseData._userRepoBase;
            _mapper = _baseData._maperBase;
            _config = _baseData._configBase;
            _userQueries = _baseData._userQueriesBase;
            _mail = _baseData._emailServiceBase;
            _logger = _baseData._loggerBase.Object;
        }
        [Fact]
        public async Task RegisterSuccess()
        {
            CreateUserCommand user = new()
            {
                Name = "test",
                Surname = "test2",
                Email = "leanhphi1706@gmail.com",
                PhoneNumber = "1234567890",
                BirthDate = DateTime.Now,
                UserName = "test11",
                PasswordHash = "1234567Az*99",
                Address = "string",
                Gender = MuonRoi.Social_Network.User.EnumGender.Male,
            };
            // CreateUserCommandHandler handler = new(_mapper, _user, _userQueries, _config, _mail.Object, _logger, null);
            // MethodResult<UserModelResponse> result = await handler.Handle(user, CancellationToken.None);
            // result.ShouldBeOfType<MethodResult<UserModelResponse>>();
        }
        [Fact]
        public async Task RegisterFail_NotValidRequest()
        {
            CreateUserCommand user = new()
            {
                Name = "test",
                Surname = "test2",
                Email = "leanhphi@1706@gmail.com",
                PhoneNumber = "1234567890",
                BirthDate = DateTime.Now,
                UserName = "test11",
                PasswordHash = "12345",
                Address = "string",
                Gender = MuonRoi.Social_Network.User.EnumGender.Male,
            };
            AppUser newUser = _mapper.Map<AppUser>(user);
            MethodResult<UserModelResponse> methodResult = new()
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            newUser.IsValid();
            methodResult.AddResultFromErrorList(newUser.ErrorMessages);
            // CreateUserCommandHandler handler = new(_mapper, _user, _userQueries, _config, _mail.Object, _logger, null);
            //   MethodResult<UserModelResponse> result = await handler.Handle(user, CancellationToken.None);
            //   bool resultMessageAndCode = CheckObjectEqual.ObjectAreEqual(result, methodResult);
            //   Assert.True(resultMessageAndCode);
        }
        [Fact]
        public async Task RegisterFail_UserIsExist()
        {
            CreateUserCommand user = new()
            {
                Name = "test",
                Surname = "test2",
                Email = "leanhphi1706@gmail.com",
                PhoneNumber = "1234567890",
                BirthDate = DateTime.Now,
                UserName = "test1",
                PasswordHash = "1234567Az*99",
                Address = "string",
                Gender = MuonRoi.Social_Network.User.EnumGender.Male,
            };
            MethodResult<UserModelResponse> methodResult = new()
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR13C),
                        new[] { Helpers.GenerateErrorResult(nameof(user.UserName), user.UserName ?? "") }
                    );
            //  CreateUserCommandHandler handler = new(_mapper, _user, _userQueries, _config, _mail.Object, _logger, null);
            //  MethodResult<UserModelResponse> result = await handler.Handle(user, CancellationToken.None);
            // bool resultMessageAndCode = CheckObjectEqual.ObjectAreEqual(result, methodResult);
            //  Assert.True(resultMessageAndCode);
        }
        [Fact]
        public async Task RegisterFail_DbContextIsNull()
        {
            CreateUserCommand user = new()
            {
                Name = "test",
                Surname = "test2",
                Email = "leanhphi1706@gmail.com",
                PhoneNumber = "1234567890",
                BirthDate = DateTime.Now,
                UserName = "test1",
                PasswordHash = "1234567Az*99",
                Address = "string",
                Gender = MuonRoi.Social_Network.User.EnumGender.Male,
            };
            MethodResult<UserModelResponse> methodResult = new()
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            //  var _users = new UserRepository(null);
            //  CreateUserCommandHandler handler = new(_mapper, _user, _userQueries, _config, _mail.Object, _logger, null);
            //  MethodResult<UserModelResponse> result = await handler.Handle(user, CancellationToken.None);
            ///  bool resultMessageAndCode = CheckObjectEqual.ObjectAreEqual(result, methodResult);
            //  Assert.False(resultMessageAndCode);
        }
    }
}