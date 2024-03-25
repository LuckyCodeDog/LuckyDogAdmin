using CMS.Common.ValidateRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DTO
{
    public class Sys_RoleDto
    {
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        /// 
        [CustomRequiiredValidation("A role name is needed here.")]
        public string? RoleName { set; get; }

        /// <summary>
        /// 用户状态  0正常 1冻结 2删除
        /// </summary>     
        /// 
        [CustomRequiiredValidation("A role status is needed here.")]
        public int Status { set; get; }
    }
}
