using System;
using System.Collections.Generic;
using System.Text;
using EasyCode.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCode.WebFramework.Infrastructure
{
    public class LiteCommonStartup : ILiteStartup
    {
        public int Order => 30;

        public void Configure(IApplicationBuilder application)
        {
            
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            
        }
    }
}
