using System;
using System.Collections.Generic;
using System.Text;
using EasyCode.Core.Data;
using EasyCode.Core.Data.Extensions;
using EasyCode.Core.Infrastructure;
using EasyCode.Data;
using EasyCode.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCode.WebFramework.Infrastructure
{
    public class LiteDbContextStartup : ILiteStartup
    {
        public int Order => 6;

        public void Configure(IApplicationBuilder application)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<EasyCodeContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
            services.AddUnitOfWork<EasyCodeContext>();
            //services.AddScoped<RoleManager<SysRoles>>();
            services.AddIdentityCore<SysUsers>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<EasyCodeContext>();
            
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
        }
    }
}
