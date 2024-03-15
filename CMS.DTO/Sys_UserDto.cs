using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;
using CMS.Common.ValidateRules;
using Zhaoxi.Manage.Common.ValidateRules;
namespace CMS.DTO
{
    /// <summary>
    ///  user dto
    /// </summary>
    public class Sys_UserDto
    {
        public int? UserId { get; set; }
        [CustomRequiiredValidationAttribute("user name can not be null")]
        public string? Name { set; get; }

        [CustomPasssWordValidation("Password`s length needs to be >=6.")]
        public string? Password { set; get; }


        public int Status { set; get; }
        [CustomNumerValidation("A valid phone number is needed")]
        public string? Phone { set; get; }
        [CustomNumerValidation("A valid phone number is needed.")]
        public string? Mobile { set; get; }

        public string? Address { set; get; }
        [CustomEmailValidation("A valid email is needed.")]
        public string? Email { set; get; }

        [CustomNumerValidation("A valid qq number is needed.")]
        public long QQ { set; get; }

        public string? ImageUrl { set; get; }
        public string? WeChat { set; get; }

        [CustomRequiiredValidation("A valid gender is needed.")]
        public int Sex { set; get; }

    }
}
