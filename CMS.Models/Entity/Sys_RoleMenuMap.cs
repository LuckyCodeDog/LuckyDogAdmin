using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    /// <summary>
    /// 角色菜单关联表
    /// </summary>
    [SugarTable("Sys_RoleMenuMap")]
    public class Sys_RoleMenuMap : Sys_BaseModel
    {
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "RoleId")]
        public int RoleId { get; set; }

        [SugarColumn(ColumnName = "MenuId")]
        public Guid MenuId { get; set; }
    }
}
