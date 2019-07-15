using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Core.Infrastructure
{
    public interface IStartupTask
    {
        /// <summary>
        /// Executes a task
        /// </summary>
        void Execute();

        /// <summary>
        /// Gets order of this startup task implementation
        /// </summary>
        int Order { get; }
    }
}
