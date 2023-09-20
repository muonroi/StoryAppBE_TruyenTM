using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BaseConfig.JWT
{
    public class GenarateJwtToken
    {
        private readonly IConfiguration _configuration;
        public GenarateJwtToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenarateJwt(UserModelResponse user, int expiresTime, List<string>? listRoles = null)
        {
            SymmetricSecurityKey symmetricKey = new(Convert.FromBase64String(_configuration.GetSection(ConstAppSettings.Instance.APPLICATIONSERECT).Value));
            JwtSecurityTokenHandler tokenHandler = new();
            string? myIssuer = _configuration.GetSection(ConstAppSettings.Instance.ENV_SERECT).Value;
            string? myAudience = _configuration.GetSection(ConstAppSettings.Instance.APPLICATIONAPPDOMAIN).Value;
            DateTime now = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim("name_user", $"{user.Surname} {user.Name }" ?? ""),
                new Claim("username", user.Username ?? ""),
                new Claim("user_id", user.Id.ToString()),
                new Claim("email", user.Email ?? ""),
                new Claim("group_id", user.GroupId.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };
            if (listRoles != null && listRoles.Any())
            {
                foreach (var role in listRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, user.RoleName ?? "visitorUser"));
            }
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddDays(expiresTime),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(symmetricKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken? stoken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(stoken);
            return token;
        }
    }
}
