using CMS.Common.DTO.log;
using CMS.Common.Enum;
using CMS.MentApi.Untility.DatabaseExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CMS.MentApi.Untility.Filters
{
    /// <summary>
    /// log filter
    /// </summary>
    public class CustomLog4NetActionFilter : Attribute, IActionFilter
    {
        private ILogger<CustomLog4NetActionFilter> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CustomLog4NetActionFilter(ILogger<CustomLog4NetActionFilter> logger)
        {
            _logger = logger;   
        }

        /// <summary>
        /// before function 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {

            LogMsgDTO logMsgMode = new LogMsgDTO()
            {
                OperationType = (int)OperationTypeEnum.Start,
            };

            //get user name and id 
             string? userName =  context.HttpContext.User?.Claims.Where(c=>c.Type ==ClaimTypes.Name).First().Value;
             string?  userId  =context.HttpContext.User?.Claims.Where(c=>c.Type==ClaimTypes.Sid).First().Value;   
              logMsgMode.CurrentUseId = userId;
             logMsgMode.CurrentUseName = userName;

            //get action target 
            object? oAttribute =    context?.ActionDescriptor.EndpointMetadata.FirstOrDefault(c => c.GetType().Equals(typeof(FunctionAttribute)));
            if (oAttribute != null) { 
                FunctionAttribute functionAttribute = (FunctionAttribute)oAttribute;
                logMsgMode.ActionDescription = functionAttribute.GetMenuName();
            }
            object?  controller =  context?.HttpContext.GetRouteValue("controller");
            object? action = context?.HttpContext.GetRouteValue("action");

            logMsgMode.ApiName = $"[{controller}_${action}]";


            //  get params 

            string actionParams =  Newtonsoft.Json.JsonConvert.SerializeObject(context?.ActionArguments);
            logMsgMode.StringParmeter = actionParams;

            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(logMsgMode));

        


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            LogMsgDTO logMsgMode = new LogMsgDTO()
            {
                OperationType = (int)OperationTypeEnum.Start
            };

            string? currentUsername = context?.HttpContext?.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string? currentUserid = context?.HttpContext?.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            logMsgMode.CurrentUseName = currentUsername;
            logMsgMode.CurrentUseId = currentUserid;


        
            object? oAttribute = context.ActionDescriptor.EndpointMetadata
               .FirstOrDefault(c => c.GetType().Equals(typeof(FunctionAttribute)));

            if (oAttribute != null)
            {
                FunctionAttribute? attribute = oAttribute as FunctionAttribute;
                logMsgMode.ActionDescription = attribute.GetMenuName();
            }
            object? controllerName = context.HttpContext.GetRouteValue("controller");
            object? actionName = context.HttpContext.GetRouteValue("action");
            logMsgMode.ApiName = $"[{controllerName}-{actionName}]";

            // result 
            ObjectResult? objectResult = context.Result as ObjectResult;

            if (objectResult != null)
            {
                logMsgMode.StringResult= Newtonsoft.Json.JsonConvert.SerializeObject(objectResult.Value);
            }
            else
            {
                JsonResult? jsonResult = context.Result as JsonResult;
                if (jsonResult != null)
                {
                    logMsgMode.StringResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Value);
                }
            }
           //  _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(logMsgMode));

        }


    }
}
