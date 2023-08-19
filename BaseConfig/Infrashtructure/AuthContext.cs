using BaseConfig.Infrashtructure.Interface;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BaseConfig.Infrashtructure
{
    public class AuthContext
    {
        public string CurrentUserId { get; set; } = string.Empty;

        public string CurrentNameUser { get; set; } = string.Empty;

        public string CurrentUsername { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int GroupId { get; set; }

        public string Guid { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string ApiKey { get; set; } = string.Empty;

        public string SiteKey { get; set; } = string.Empty;

        public List<string> Roles { get; set; }
        public AuthContext()
        {
        }

        public AuthContext(string currentUserId, string currentNameUser, string currentUsername, string email, int groupId, string accessToken, string apiKey, string contextGuid, string siteKey)
        {
            SiteKey = siteKey;
            CurrentUserId = currentUserId;
            CurrentNameUser = currentNameUser;
            CurrentUsername = currentUsername;
            Email = email;
            GroupId = groupId;
            AccessToken = accessToken;
            ApiKey = apiKey;
            Guid = (string.IsNullOrEmpty(contextGuid) ? System.Guid.NewGuid().ToString() : contextGuid);
            if (string.IsNullOrEmpty(AccessToken))
            {
                return;
            }
            try
            {
                JwtSecurityTokenHandler
                               jwtSecurityTokenHandler = new();
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(AccessToken.Replace("Bearer ", ""));
                List<Claim> list = jwtSecurityToken.Claims.ToList();
                if (list.Exists((Claim m) => m.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                {
                    Roles = (from m in list
                             where m.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                             select m.Value).ToList();
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        public AuthContext(IHttpContextAccessor httpContextAccessor)
        {
            string subject = "Authorization";
            string text = httpContextAccessor.HttpContext.Request.Headers[subject];
            if (!string.IsNullOrEmpty(text))
            {
                AccessToken = text;
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(text.Replace("Bearer ", ""));
                List<Claim> list = jwtSecurityToken.Claims.ToList();

                subject = "user_id";
                if (list.Exists((Claim c) => c.Type == subject))
                {
                    string user_guid = list.First((Claim c) => c.Type == subject).Value;
                    if (!string.IsNullOrEmpty(user_guid))
                    {
                        CurrentUserId = user_guid;
                    }
                }

                subject = "email";
                if (list.Exists((Claim c) => c.Type == subject))
                {
                    string email = list.First((Claim c) => c.Type == subject).Value;
                    if (!string.IsNullOrEmpty(email))
                    {
                        Email = email;
                    }
                }

                subject = "group_id";
                if (list.Exists((Claim c) => c.Type == subject))
                {
                    string group_id = list.First((Claim c) => c.Type == subject).Value;
                    if (!string.IsNullOrEmpty(group_id))
                    {
                        GroupId = int.Parse(group_id);
                    }
                }

                subject = "username";
                if (list.Exists((Claim c) => c.Type == subject))
                {
                    CurrentUsername = list.First((Claim c) => c.Type == subject).Value;
                }
                subject = "name_user";
                if (list.Exists((Claim c) => c.Type == subject))
                {
                    CurrentNameUser = list.First((Claim c) => c.Type == subject).Value;
                }
                if (list.Exists((Claim m) => m.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                {
                    Roles = (from m in list
                             where m.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                             select m.Value).ToList();
                }

                SiteKey = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{CurrentUserId};{GroupId}"));
            }
            else
            {
                subject = "SITEKEY";
                if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey(subject))
                {
                    string text2 = httpContextAccessor.HttpContext.Request.Headers[subject];
                    if (!string.IsNullOrEmpty(text2))
                    {
                        SiteKey = text2;
                        byte[] bytes = Convert.FromBase64String(text2);
                        string @string = Encoding.UTF8.GetString(bytes);
                        if (!string.IsNullOrEmpty(@string) && @string.IndexOf(';') > -1)
                        {
                            string[] array = @string.Split(';');
                            CurrentUserId = array[0];
                            GroupId = int.Parse(array[1]);
                        }
                    }
                }
            }

            subject = "APIKEY";
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey(subject))
            {
                ApiKey = httpContextAccessor.HttpContext.Request.Headers[subject];
            }

            subject = "context_guid";
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey(subject))
            {
                Guid = httpContextAccessor.HttpContext.Request.Headers[subject];
            }
            else
            {
                Guid = System.Guid.NewGuid().ToString();
            }
        }

        public AuthContext(IAmqpContext amqpContext)
        {
            string result = amqpContext.GetHeaderByKey("user_id");
            CurrentUserId = result;
            string headerByKey = amqpContext.GetHeaderByKey("user_guid");
            if (!string.IsNullOrEmpty(headerByKey))
            {
                CurrentUserId = headerByKey;
            }
            AccessToken = amqpContext.GetHeaderByKey("access_token");
            ApiKey = amqpContext.GetHeaderByKey("APIKEY");
            string headerByKey2 = amqpContext.GetHeaderByKey("context_guid");
            Guid = (string.IsNullOrEmpty(headerByKey2) ? System.Guid.NewGuid().ToString() : headerByKey2);
            if (!string.IsNullOrEmpty(AccessToken))
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(AccessToken.Replace("Bearer ", ""));
                List<Claim> list = jwtSecurityToken.Claims.ToList();
                if (list.Exists((Claim m) => m.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                {
                    Roles = (from m in list
                             where m.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                             select m.Value).ToList();
                }
            }

            headerByKey = amqpContext.GetHeaderByKey("site_key");
            if (!string.IsNullOrEmpty(headerByKey))
            {
                SiteKey = headerByKey;
            }

            if (!string.IsNullOrEmpty(SiteKey))
            {
                byte[] bytes = Convert.FromBase64String(SiteKey);
                string @string = Encoding.UTF8.GetString(bytes);
                if (!string.IsNullOrEmpty(@string) && @string.IndexOf(';') > -1)
                {
                    string[] array = @string.Split(';');
                    CurrentUserId = array[0];
                    GroupId = int.Parse(array[1]);
                }
            }
            else
            {
                SiteKey = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{CurrentUserId};{GroupId}"));
            }

            CurrentUsername = amqpContext.GetHeaderByKey("username");
        }

        public bool HasRole(string roles)
        {
            if (string.IsNullOrEmpty(roles) || Roles == null || Roles.Count == 0)
            {
                return false;
            }

            return Roles.Exists((string m) => roles.Split(',').Contains<string>(m));
        }
    }
}
