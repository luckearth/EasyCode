using System;
using System.Collections.Generic;
using System.Text;
using EasyCode.ViewModel;

namespace EasyCode.IService
{
    public interface ITokenProviderService
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        ResponseTokenViewModel GeTokenViewModel(string username);
        /// <summary>
        /// 获取刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        ResponseTokenViewModel GetRefreshToken(string refreshToken);
    }
}
