using CMS.BusinessInterface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CMS.MentApi.Untility.AuthorizeExt
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuAuthorizeHandler : AuthorizationHandler<MenuAuthorizeRequirement>
    {
        private IUserService _userService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public MenuAuthorizeHandler(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MenuAuthorizeRequirement requirement)
        {
            // get user claimaas from context 
            if (context.User.Claims == null || context.User.Claims.Count() <= 0)
            {
                context?.Fail();
            }
            else
            {
                HttpContext httpContext = (HttpContext)context.Resource!;
                object? controllerName =   httpContext.GetRouteValue("controller");
                object? actionName = httpContext.GetRouteValue("action");

                 string?  strUserId  =  context.User?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Sid))?.Value;
                if(strUserId ==null)
                {
                    context?.Fail();
                }
                else
                {
                    //validate find buttons 
                   bool result =    await  _userService.ValidateBtnAsync(int.Parse(strUserId),$"{controllerName}Controller_{actionName}");

                    if (result)
                    {
                        context?.Succeed(requirement);
                    }else
                    {
                        context.Fail();
                    }
                }
            }
            await Task.CompletedTask;
        }
    }
}
