using Autofac;
using Autofac.Extensions.DependencyInjection;
using BaseConfig.BaseStartUp;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using MuonRoiSocialNetwork.Common.Models;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Infrastructure;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;

namespace MuonRoiSocialNetwork.StartupConfig
{
    internal static class CustomRegisterService
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            var customAppsetting = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
            var config = customAppsetting.Build();
            builder.Services.CustomAuthentication(config);
            builder.Services.CustomeAuthorization();
            builder.Services.RegisterTransient();
            builder.Services.SwaggerConfiguration();
            builder.Services.RegisterScoped();
            builder.Services.AddHealthChecks();
            builder.Services.RegisterSingleton();
            builder.Services.AddControllers();
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config[ConstAppSettings.Instance.CONNECTIONSTRING_REDIS] ?? ConstAppSettings.Instance.CONNECTIONSTRING_REDIS;
                options.InstanceName = "MemoryMuonroi_";
            });
            builder.Services.AddDbContext<MuonRoiSocialNetworkDbContext>(opt =>
            {
                opt.UseNpgsql(config[ConstAppSettings.Instance.CONNECTIONSTRING_DB] ?? ConstAppSettings.Instance.CONNECTIONSTRING_DB, sql => sql.EnableRetryOnFailure(3));
            });
            builder.Services.Configure<SMTPConfigModel>(config.GetSection($"{NameAppSetting.SMTPCONFIG}"));
            builder.Services.AddSignalR();
            var hangfire_connection = config[ConstAppSettings.Instance.CONNECTIONSTRING_HANGFIRE] ?? ConstAppSettings.Instance.CONNECTIONSTRING_HANGFIRE;
            builder.Services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(hangfire_connection)
                    );
            builder.Services.AddHangfireServer();
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            ContainerBuilder containerBuilder = new();
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AuthContextModule()));
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            Log.Logger = new LoggerConfiguration()
                                        .WriteTo.Console()
                                        .WriteTo.File(new JsonFormatter(renderMessage: false), "Logs/ActionAPI.json")
                                        .WriteTo.File("Logs/Error.logs",
                                                      restrictedToMinimumLevel: LogEventLevel.Error,
                                                      rollingInterval: RollingInterval.Day)
                                        .MinimumLevel.Debug()
                                        .CreateLogger();
            return builder;
        }
    }
}
