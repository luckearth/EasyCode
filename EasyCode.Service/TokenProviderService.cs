using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EasyCode.Core;
using EasyCode.Core.Data;
using EasyCode.Data;
using EasyCode.Entity;
using EasyCode.IService;
using EasyCode.ViewModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EasyCode.Service
{
    public class TokenProviderService : ITokenProviderService
    {
        private TokenProviderOptions _options;
        private IUnitOfWork<EasyCodeContext> _unitOfWork;
        private IRepository<SysApplication> _repository;
        public TokenProviderService(TokenProviderOptions options, IUnitOfWork<EasyCodeContext> unitOfWork)
        {
            _options = options;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<SysApplication>();
        }
        public async Task<SysApplication> GetApplicationAsync()
        {
            return await _repository.FirstAsync(a=>a.Id=="1");
        }
        public async Task AddApplication(SysApplication application)
        {
            await _repository.AddAsync(application);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateApplication(SysApplication application)
        {
            _repository.Update(application);
            await _unitOfWork.SaveChangesAsync();
        }
        public ResponseTokenViewModel GeTokenViewModel(string username,string userid)
        {
            var refreshtoken = Guid.NewGuid().ToString("N");
            var now = DateTime.UtcNow;
            
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId,""),
                new Claim(JwtRegisteredClaimNames.Jti, ""),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Sub, username),
            };
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims.ToArray(),
                notBefore: now,
                expires: now.AddMinutes(_options.Expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            _unitOfWork.DbContext.SysUserTokenses.Add(new SysUserTokens(){ ExpiresUtc = DateTime.UtcNow,Token = encodedJwt,UserId = userid, Value = refreshtoken,LoginProvider = "",Name = username});
            _unitOfWork.SaveChanges();
            ResponseTokenViewModel model = new ResponseTokenViewModel()
            {
                access_token = encodedJwt,
                refresh_token = refreshtoken,
                expires_in = (int)TimeSpan.FromSeconds(_options.Expiration*60).TotalSeconds,
                userName = username,
                firstname = username,
                lastname = username,
                createtime = DateTime.Now
            };
            return model;
        }

        public ResponseTokenViewModel GetRefreshToken(string refreshToken)
        {
            var model = GeTokenViewModel("","");
            return model;
        }
    }
}
