using System;
using Autofac.Extensions.DependencyInjection;
using EasyCode.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace EasyCode.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host=CreateHostBuilder(args).Build();
            //正式环境请删除以下代码
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    
                    var userManager = services.GetRequiredService<UserManager<SysUsers>>();
                    var roleManager = services.GetRequiredService<RoleManager<SysRoles>>();
                    ApiDbSeedData.Seed(userManager,roleManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                        {
                            // Set properties and call methods on options
                        })
                        .UseStartup<Startup>()
                        .UseNLog();
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
