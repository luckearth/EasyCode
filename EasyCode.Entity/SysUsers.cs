using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace EasyCode.Entity
{
    public class SysUserTokens : IdentityUserToken<string>
    {

    }
    public class SysUsers : IdentityUser<string>
    {
        public string DepartmentId { get; set; }
        public string FullName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsLock { get; set; }
    }
}
