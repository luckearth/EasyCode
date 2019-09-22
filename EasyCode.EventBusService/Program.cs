using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EasyCode.EventBusService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                var serviceProvider = new ServiceCollection();
                serviceProvider.AddLogging(builder => {
                    builder.AddConfiguration(configuration.GetSection("Logging"));
                    builder.AddConsole();
                });
                //serviceProvider.AddScoped<Test>();
                using (var bootstraper = new ServiceBootstrapper())
                {
                    bootstraper.Build(serviceProvider);
                    bootstraper.Start();
                    Console.WriteLine("Hello World!");
                    Console.ReadKey();
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
