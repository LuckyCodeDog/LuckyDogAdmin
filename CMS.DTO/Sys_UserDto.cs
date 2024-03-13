using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;
using CMS.Common.ValidateRules;
namespace CMS.DTO
{
    /// <summary>
    ///  user dto
    /// </summary>
    public class Sys_UserDto
    {
        [CustomRequiiredValidation("user id can not be null")]
        public int? UserId { get; set; }
        [CustomRequiiredValidation("user name can not be null")]
        public string? Name { set; get; }

        public string? Password { set; get; }


        public int Status { set; get; }

        public string? Phone { set; get; }

        public string? Mobile { set; get; }

        public string? Address { set; get; }

        public string? Email { set; get; }

        public long QQ { set; get; }

        public string? WeChat { set; get; }

        public int Sex { set; get; }

        public DateTime LastLoginTime { set; get; }
    }
}
