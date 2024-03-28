using CMS.Common.DTO.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace CMS.Common.JwtService
{
    public abstract class CustomJWTService
    {
        public abstract string CreateToken(SysUserInfo user, out string token);


        public virtual   List<Claim>  CliamsToUser(SysUserInfo user)
        {
            List<Claim> claims = new List<Claim>()
            {
               new Claim(ClaimTypes.Sid, user.UserId.ToString()),
               new Claim(ClaimTypes.Name, user.Name?? string.Empty),
               new Claim(ClaimTypes.MobilePhone, user.Mobile?? string.Empty),
               new Claim(ClaimTypes.OtherPhone, user.Phone?? string.Empty),
               new Claim(ClaimTypes.StreetAddress, user.Address?? string.Empty),
               new Claim(ClaimTypes.Email, user.Email?? string.Empty),
               new Claim("QQ", user.QQ.ToString()),
               new Claim("WeChat", user.WeChat?? string.Empty),
               new Claim("Sex", user.Sex.ToString())

            };
            foreach(var roleId in user.RoleIdList!)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleId.ToString()));
            }

            foreach(var menu in user.UserMenuList!)
            {
                claims.Add(new Claim("Menus",Newtonsoft.Json.JsonConvert.SerializeObject(menu)));
            }

            foreach(var btn in user.UserBtnList!)
            {
                claims.Add(new Claim("Btns", btn.Value));
            }

            return claims;
        }
    }
}
