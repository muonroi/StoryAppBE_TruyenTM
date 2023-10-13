using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MuonRoiSocialNetwork.Infrastructure;
using MuonRoiSocialNetwork.Infrastructure.HubCentral;

namespace MuonRoiSocialNetwork.StartupConfig
{
    internal static class CustomeMidleWareConfiguration
    {
        public static WebApplication CustomMidleWare(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.MapControllers();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });
                endpoints.MapHangfireDashboard();
                endpoints.MapHub<NotificationHub>("/hubs/noti-hub");
                endpoints.MapHub<ChatHub>("/hubs/chat-hub");
            });
            CustomHangfireJobConfiguration.RegisterJobs();
            app.UseHangfireDashboard();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<MuonRoiSocialNetworkDbContext>();
                context.Database.Migrate();
            }
            return app;
        }
    }
}
