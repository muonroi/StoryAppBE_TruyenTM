namespace MuonRoiSocialNetwork.StartupConfig
{
    internal static class CustomAuthorizationConfiguration
    {
        public static IServiceCollection CustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SU", policy => policy.RequireRole("READ", "WRITE", "EDIT", "DELETE"));
                options.AddPolicy("MOD", policy => policy.RequireRole("READ", "WRITE", "EDIT"));
                options.AddPolicy("AUTHOR", policy => policy.RequireRole("READ", "WRITE"));
                options.AddPolicy("VIEWER", policy => policy.RequireRole("READ"));
            });
            return services;
        }
    }
}
