using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.Result
{
    public class ApiResult
    {
        /// <summary>
        /// if successed?
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// msg carried
        /// </summary>
        public string? Message { get; set; }
    }

    public class ApiResult<T> : ApiResult where T : class
    {

        /// <summary>
        /// result
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Object? OValue { get; set; }

    }


}
