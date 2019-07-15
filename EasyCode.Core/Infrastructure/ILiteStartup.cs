using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCode.Core.Infrastructure
{
    public interface ILiteStartup
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        void Configure(IApplicationBuilder application);

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Order { get; }
    }
}
