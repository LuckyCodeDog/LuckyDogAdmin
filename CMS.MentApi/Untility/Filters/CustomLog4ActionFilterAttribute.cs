using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using CMS.Common.Enum;
using CMS.Common.DTO.log;
using log4net.Ext.Json;
using CMS.MentApi.Untility.DatabaseExt;

namespace Zhaoxi.Manage.MentApi.Utility.Filters
{
    /// <summary>
    /// 全局保存日志
    /// </summary>
    public class CustomLog4ActionFilterAttribute : Attribute, IActionFilter
    {

        private readonly ILogger<CustomLog4ActionFilterAttribute> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CustomLog4ActionFilterAttribute(ILogger<CustomLog4ActionFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 在XXXAction执行前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        { 
            LogMsgDTO logMsgMode = new LogMsgDTO()
            {
                OperationType = (int)OperationTypeEnum.Start
            };
            #region 1. 操作人
            string? currentUsername = context?.HttpContext?.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string? currentUserid = context?.HttpContext?.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            logMsgMode.CurrentUseName = currentUsername;
            logMsgMode.CurrentUseId = currentUserid;
            #endregion

            #region 2.操作目标
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
            #endregion

            #region 3.参数
            string strParameter = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments);
            logMsgMode.StringParmeter = strParameter;
            #endregion 
            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(logMsgMode));

        }

        /// <summary>
        /// 在XXXAction执行后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            LogMsgDTO logMsgMode = new LogMsgDTO()
            {
                OperationType = (int)OperationTypeEnum.Start
            };
            #region 1. 操作人
            string? currentUsername = context?.HttpContext?.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string? currentUserid = context?.HttpContext?.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            logMsgMode.CurrentUseName = currentUsername;
            logMsgMode.CurrentUseId = currentUserid;
            #endregion

            #region 2.操作目标
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
            #endregion

            #region 3.操作结果 
            ObjectResult? objectResult = context.Result as ObjectResult;
            if (objectResult != null)
            {
                logMsgMode.StringResult = Newtonsoft.Json.JsonConvert.SerializeObject(objectResult.Value);
            }
            else
            {
                JsonResult? jsonResult = context.Result as JsonResult;
                if (jsonResult != null)
                {
                    logMsgMode.StringResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Value);
                } 
            }
            #endregion 
            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(logMsgMode));

        }


    }
}
