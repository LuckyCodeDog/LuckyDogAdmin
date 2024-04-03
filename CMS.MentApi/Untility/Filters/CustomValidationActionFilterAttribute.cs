using CMS.Common.Result;
using CMS.Common.ValidateRules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace CMS.MentApi.Untility.Filters
{
    /// <summary>
    ///  to validate properties in  dto entity
    /// </summary>
    public class CustomValidationActionFilterAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// get  entity from the context and filter those with attributes can 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //1. get action entity params
            //2. loop entity list find those are with attribute
            //3. activate attribute onvalidate method  and pass  entity instance 
            //4. encapsulate return into a result 
            //got the entity list 

            List<(bool, string?)> result = new List<(bool, string?)>();
            List<object?> actionParams = context.ActionArguments.Values.ToList();
            //find prop defined validation attribute
            foreach (var param in actionParams)
            {
                foreach (var prop in param.GetType().GetProperties())
                {
                    //这里存在不完善的地方如果一个property用了多个验证标记则无法读取 
                    if (prop.IsDefined(typeof(BaseValidateAtrribute), true))
                    {
                        var oValue = prop.GetValue(param);
                        BaseValidateAtrribute? customValidation = prop?.GetCustomAttribute<BaseValidateAtrribute>();
                        result.Add(customValidation.DoValidate(oValue));
                    }
                }

            }
            //5. loop result to see if any false 
            //6.  
            if (result.Any(r => r.Item1 == false))
            {
                context.Result = new JsonResult(new ApiResult()
                {
                    Message = string.Join(",", result.Where(r => r.Item1 == false).Select(r => r.Item2)),
                    Success = false
                });
            }
        }


    }
}
