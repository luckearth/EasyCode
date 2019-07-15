using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Entity
{
    public class SysModules
    {
        public virtual string Id { get; set; }
        public virtual string ParentId { get; set; }
        public virtual string ApplicationId { get; set; }
        public virtual int ModuleLayer { get; set; }
        public virtual string ModuleName { get; set; }
        public virtual string AreaName { get; set; }
        public virtual string ControllerName { get; set; }
        public virtual string ActionName { get; set; }
        public virtual int ModuleType { get; set; }
        public virtual bool IsExpand { get; set; }
        public virtual string Icon { get; set; }
        public virtual string ModuleDescription { get; set; }
        public virtual int PurviewNum { get; set; }
        public virtual long PurviewSum { get; set; }
        public virtual bool IsDelete { get; set; }
        public virtual int Sort { get; set; }
        public virtual bool IsValidPurView { get; set; } = true;
        public virtual DateTime CreateTime { get; set; }
        public virtual SysApplication Application { get; set; } = new SysApplication();
    }
}
