using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace EasyCode.Entity
{
    public class SysUserTokens : IdentityUserToken<string>
    {
        public DateTime ExpiresUtc { get; set; }
        public string Token { get; set; }
    }
    public class SysUsers : IdentityUser<string>
    {
        public string DepartmentId { get; set; }
        public string FullName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsLock { get; set; }
    }
}
