using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    /// <summary>
    /// 角色按钮关联表
    /// </summary>
    [SugarTable("Sys_RoleBtnMap")]
    public class Sys_RoleBtnMap : Sys_BaseModel
    {
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "RoleId")]
        public int RoleId { get; set; }

        [SugarColumn(ColumnName = "BtnId")]
        public Guid BtnId { get; set; }
    }
}
