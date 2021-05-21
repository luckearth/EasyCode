using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCode.Core.Utility;
using EasyCode.Entity;
using EasyCode.IService;
using EasyCode.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyCode.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private ITokenProviderService _tokenProvider;
        private UserManager<SysUsers> _userManager;
        public AccountController(ITokenProviderService tokenProvider, UserManager<SysUsers> userManager)
        {
            _tokenProvider = tokenProvider;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var applications =await _tokenProvider.GetApplicationsAsync();
            
            await _tokenProvider.UpdateApplication(applications);
            return Ok("Hello");
        }
        [AllowAnonymous]
        [HttpPost,Route("Login")]
        public async Task<IActionResult> PostLogin([FromBody]SysLoginViewModel model)
        {
            var result = new Result();
            try
            {
                
                SysUsers manager = await _userManager.FindByNameAsync(model.UserName);
                var password = await _userManager.CheckPasswordAsync(manager, model.Password);
                if (password)
                {
                    result.Succeeded = true;
                    result.Message = "请求成功";
                    result.Data = _tokenProvider.GeTokenViewModel(model.UserName,manager.Id);
                }
                else
                {
                    result.Code = -101;
                    result.Message = "用户名或密码错误！";
                    result.Succeeded = false;
                }

            }
            catch (Exception e)
            {
                result.Code = -100;
                result.Message = "数据异常";
                result.Succeeded = false;
            }
            
            return Json(result);
        }
    }
}