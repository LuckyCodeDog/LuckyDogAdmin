using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{

    /// </summary>
    [SugarTable("Sys_UserRoleMap")]
    public class Sys_UserRoleMap : Sys_BaseModel
    {
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "UserId")]
        public int UserId { get; set; }

        [SugarColumn(ColumnName = "RoleId")]
        public int RoleId { get; set; }
    }
}
