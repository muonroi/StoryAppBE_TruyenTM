using Hangfire;
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
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });
            CustomHangfireJobConfiguration.RegisterJobs();
            app.UseHangfireDashboard();
            return app;
        }
    }
}
