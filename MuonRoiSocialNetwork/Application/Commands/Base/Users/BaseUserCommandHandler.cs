using AutoMapper;
using BaseConfig.Infrashtructure;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace MuonRoiSocialNetwork.Application.Commands.Base.Users
{
    /// <summary>
    /// Handler base user
    /// </summary>
    public class BaseUserCommandHandler
    {
        /// <summary>
        /// property _mapper
        /// </summary>
        protected readonly IMapper _mapper;
        /// <summary>
        /// property get config
        /// </summary>
        protected readonly IConfiguration _configuration;
        /// <summary>
        /// property _userQueries
        /// </summary>
        protected readonly IUserQueries _userQueries;
        /// <summary>
        /// property _userRepository
        /// </summary>
        protected readonly IUserRepository _userRepository;
        /// <summary>
        /// Info token
        /// </summary>
        protected readonly AuthContext _authContext;

        /// <summary>
        /// Handler base
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userQueries"></param>
        /// <param name="userRepository"></param>
        /// <param name="authContext"></param>
        protected BaseUserCommandHandler(IMapper mapper, IConfiguration configuration, IUserQueries userQueries, IUserRepository userRepository, AuthContext authContext)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userQueries = userQueries;
            _userRepository = userRepository;
            _authContext = authContext;
        }
        /// <summary>
        /// Hash password based salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        protected string HashPassword(string password, string salt)
        {
            // derive a 256-bit subkey (use HMACSHA512 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 4));
            return hashed;
        }
        /// <summary>
        /// Genarete salt random
        /// </summary>
        /// <returns></returns>
        protected string GenarateSalt()
        {
            byte[] randomBytes = new byte[256 / 4];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        /// <summary>
        /// Genarete random string with length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected static string RandomString(int length)
        {
            if (length < 0) { return null; }
            string alphabet = SettingUserDefault.Instance.alphabet;
            StringBuilder stringBuilder = new();
            using (var rng = RandomNumberGenerator.Create())
            {
                int count = (int)Math.Ceiling(Math.Log(alphabet.Length, 2) / 8.0);
                Debug.Assert(count <= sizeof(uint));
                int offset = BitConverter.IsLittleEndian ? 0 : sizeof(uint) - count;
                int max = (int)(Math.Pow(2, count * 8) / alphabet.Length) * alphabet.Length;
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (stringBuilder.Length < length)
                {
                    rng.GetBytes(uintBuffer, offset, count);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    if (num < max)
                    {
                        stringBuilder.Append(alphabet[(int)(num % alphabet.Length)]);
                    }
                }

            }
            return stringBuilder.ToString();
        }
    }
}
