using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyCode.Entity;
using EasyCode.ViewModel;

namespace EasyCode.IService
{
    public interface ITokenProviderService
    {
        Task<IEnumerable<SysApplication>> GetApplicationsAsync();
        Task<SysApplication> GetApplicationAsync();
        Task UpdateApplication(IEnumerable<SysApplication> applications);
        Task AddApplication(SysApplication application);
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        ResponseTokenViewModel GeTokenViewModel(string username,string userid);
        /// <summary>
        /// 获取刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        ResponseTokenViewModel GetRefreshToken(string refreshToken);
    }
}
