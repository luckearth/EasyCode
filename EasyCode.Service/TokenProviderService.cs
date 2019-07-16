using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasyCode.Core;
using EasyCode.IService;
using EasyCode.ViewModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EasyCode.Service
{
    public class TokenProviderService : ITokenProviderService
    {
        private TokenProviderOptions _options;
        public TokenProviderService(IOptions<TokenProviderOptions> options)
        {
            _options = options.Value;
        }
        public ResponseTokenViewModel GeTokenViewModel(string username)
        {
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

            ResponseTokenViewModel model = new ResponseTokenViewModel()
            {
                access_token = encodedJwt,
                refresh_token = Guid.NewGuid().ToString("N"),
                expires_in = (int)TimeSpan.FromSeconds(_options.Expiration*60).TotalSeconds,
                userName = username,
                firstname = username,
                lastname = username,
            };
            return model;
        }

        public ResponseTokenViewModel GetRefreshToken(string refreshToken)
        {
            var model = GeTokenViewModel("");
            return model;
        }
    }
}
