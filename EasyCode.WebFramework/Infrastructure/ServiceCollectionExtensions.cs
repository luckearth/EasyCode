using System;
using System.Collections.Generic;
using System.Text;
using EasyCode.Core;
using EasyCode.Core.Configuration;
using EasyCode.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
namespace EasyCode.WebFramework.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static (IEngine, LiteConfig) ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.ConfigureStartupConfig<TokenProviderOptions>(configuration.GetSection("TokenProviderOptions"));
            var liteConfig=services.ConfigureStartupConfig<LiteConfig>(configuration.GetSection("LiteConfig"));
            services.AddHttpContextAccessor();
            services.AddOptions();
            

            var engine = EngineContext.Create();

            engine.ConfigureServices(services, configuration,liteConfig);

            return (engine, liteConfig);

        }
        /// <summary>
        /// 绑定配置信息
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="services"></param>
        public static void AddLiteAuthentication(this IServiceCollection services)
        {

        }
    }
}
