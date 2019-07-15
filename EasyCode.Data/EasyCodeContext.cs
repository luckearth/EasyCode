using System;
using System.Collections.Generic;
using System.Text;
using EasyCode.Core.Data.Extensions;
using EasyCode.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyCode.Data
{
    public class LiteCodeContext : IdentityDbContext<SysUsers, SysRoles, string>
    {
        public LiteCodeContext(DbContextOptions<LiteCodeContext> options) : base(options)
        {

        }

        public DbSet<SysApplication> SysApplications { get; set; }
        public DbSet<SysModules> SysModuleses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SysUsers>().ToTable("Sys_Users");
            builder.Entity<SysRoles>().ToTable("Sys_Roles");

            builder.UseEntityTypeConfiguration(typeof(SysUsers));

        }

        public new DbSet<SysUserRole> UserRoles { get; set; }
    }
}
