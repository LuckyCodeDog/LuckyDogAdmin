using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    /// <summary>
    /// 角色
    /// </summary>
    [SugarTable("Sys_Role")]
    public class Sys_Role : Sys_BaseModel
    {
        [SugarColumn(ColumnName = "RoleId", IsPrimaryKey = true, IsIdentity = true)]
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(ColumnName = "RoleName")]
        public string? RoleName { set; get; }

        /// <summary>
        /// 用户状态  0正常 1冻结 2删除
        /// </summary>
        [SugarColumn(ColumnName = "Status")]
        public int Status { set; get; }
    }
}
