using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static IdentityModel.ClaimComparer;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace MuonRoiSocialNetwork.StartupConfig
{
    internal static class CustomAuthenticationConfiguration
    {
        public static IServiceCollection CustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            SymmetricSecurityKey symmetricKey = new(Convert.FromBase64String(configuration[ConstAppSettings.Instance.APPLICATIONSERECT] ?? "Application:SERECT"));
            string? myIssuer = configuration[ConstAppSettings.Instance.ENV_SERECT];
            string? myAudience = configuration[ConstAppSettings.Instance.APPLICATIONAPPDOMAIN];
            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = symmetricKey,
                ValidIssuer = myIssuer,
                ValidAudience = myAudience,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            };
            services.AddAuthentication(delegate (AuthenticationOptions x)
            {
                x.DefaultAuthenticateScheme = "Bearer";
                x.DefaultChallengeScheme = "Bearer";
            }).AddIdentityServerJwt().AddJwtBearer(delegate (JwtBearerOptions x)
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = validationParameters;
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
    }
}
