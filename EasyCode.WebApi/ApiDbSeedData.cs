using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCode.Entity;
using Microsoft.AspNetCore.Identity;

namespace EasyCode.WebApi
{
    public class ApiDbSeedData
    {
        public ApiDbSeedData(UserManager<SysUsers> userManager)
        {

        }

        public static async Task Seed(UserManager<SysUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAndClaims(userManager, roleManager);
            await SeedAdmin(userManager);
        }

        private static async Task SeedRolesAndClaims(UserManager<SysUsers> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync("admin"))
            { 
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "admin"
                });
            }

            if (!await roleManager.RoleExistsAsync("user"))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "user"
                });
            }


            var adminRole = await roleManager.FindByNameAsync("admin");
            var adminRoleClaims = await roleManager.GetClaimsAsync(adminRole);

            if (adminRoleClaims.All(x => x.Type != "user"))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim("user", "true"));
            }
            if (adminRoleClaims.All(x => x.Type != "user"))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim("admin", "true"));
            }

            var userRole = await roleManager.FindByNameAsync("user");
            var userRoleClaims = await roleManager.GetClaimsAsync(userRole);
            if (userRoleClaims.All(x => x.Type != "user"))
            {
                await roleManager.AddClaimAsync(userRole, new System.Security.Claims.Claim("user", "true"));
            }
        }

        private static async Task SeedAdmin(UserManager<SysUsers> userManager)
        {
            var u = await userManager.FindByNameAsync("admin");
            if (u == null)
            {
                u = new SysUsers
                {
                    UserName = "admin",
                    Email = "admin@nothing.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsLock = false,
                };
                var x = await userManager.CreateAsync(u, "Admin1234!");
            }
            var uc = await userManager.GetClaimsAsync(u);
            if (!uc.Any(x => x.Type == "admin"))
            {
                await userManager.AddClaimAsync(u, new System.Security.Claims.Claim("admin", true.ToString()));
            }
            if (!await userManager.IsInRoleAsync(u, "admin"))
                await userManager.AddToRoleAsync(u, "admin");
        }
    }
}
