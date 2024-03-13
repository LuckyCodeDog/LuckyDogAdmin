using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CMS.Common.Result;

namespace CMS.MentApi.Untility.Filters
{
    /// <summary>
    /// catch exception
    /// </summary>
    public class CustomExceptionFilterAttribute : Attribute, IAsyncExceptionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                //return info 
                context.Result = new JsonResult(new ApiResult()
                {
                    Message = context.Exception.Message,
                    Success = false,
                });
                context.ExceptionHandled = true;
            }
            return Task.CompletedTask;
        }

    }
}