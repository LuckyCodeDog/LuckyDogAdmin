using CMS.Common.DTO.user;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.JwtService
{
    public class CustomHSJWTService : CustomJWTService
    {
        private readonly JWTTokenOptions _jWTTokenOptions;




        public CustomHSJWTService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions)
        {
            _jWTTokenOptions = jwtTokenOptions.CurrentValue;
        }


        public override string CreateToken(SysUserInfo user, out string token)
        {
            //preapre payload 
            List<Claim> claims = base.CliamsToUser(user);
            // preapare secret key 
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTTokenOptions.SecurityKey));
            //sha256
            SigningCredentials credentials =  new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtAccessToken = new JwtSecurityToken(
                issuer: _jWTTokenOptions.Issuer,
                audience: _jWTTokenOptions.Audience,
                claims: claims.ToArray(),
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
             );

            return  token =  new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);
        }
    }
}
