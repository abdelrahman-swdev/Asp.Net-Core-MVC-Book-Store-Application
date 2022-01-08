using Book_Store_App.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Book_Store_App
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("app");

            try
            {
                var roleManagerService = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManagerService = services.GetRequiredService<UserManager<ApplicationUser>>();
                var contextService = services.GetRequiredService<ApplicationDbContext>();

                await SeedRoles.SeedRolesAsync(roleManagerService);
                await SeedUsers.SeedAdminUserAsync(userManagerService);
                await SeedLanguages.SeedInitialLanguages(contextService);

                logger.LogInformation("Data Seeded");
                logger.LogInformation("Application Started");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error while seeding data");
            }


            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
