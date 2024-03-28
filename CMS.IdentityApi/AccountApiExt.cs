using CMS.BusinessInterface;
using CMS.Common.DTO.user;
using CMS.Common.JwtService;
using CMS.Common.Result;
using Dm.Config;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CMS.IdentityApi
{
    public static class AccountApiExt
    {

        public static void Login(this WebApplication app)
        {
            //登录使用用户名和密码获取token
            app.MapPost("auth/Account", async ([Microsoft.AspNetCore.Mvc.FromServices] CustomJWTService _iJWTService, [Microsoft.AspNetCore.Mvc.FromServices] IUserService userService,  UserAccount user) =>
            {
                SysUserInfo? userInfo = await userService.Login(user);
                if (userInfo == null)
                {
                    return new ApiResult()
                    {
                        Success = false,
                        Message = "Username or password is incorrect.",
                    };
                }


                string accesstoken = _iJWTService.CreateToken(userInfo, out string refreshToken);
                return new ApiResult<object>()
                {
                    Success = true,
                    Message = "Login successful.",
                    Data = new
                    {
                        UserId = userInfo.UserId,
                        UserName = userInfo.Name,
                        Imageurl = userInfo.Imageurl
                    },
                    OValue = new
                    {
                        RefreshToken = refreshToken,
                        Accesstoken = accesstoken
                    }
                };
            }).WithGroupName("v1").WithDescription("login-to get token");
        }
    }
}
