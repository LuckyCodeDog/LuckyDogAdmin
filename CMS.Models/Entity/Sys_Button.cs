using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{

    /// <summary>
    /// 按钮的表
    /// </summary>

    [SugarTable("Sys_Button")]
    public class Sys_Button : Sys_BaseModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 父级Id---按钮归属于那个菜单
        /// </summary>
        [SugarColumn(ColumnName = "ParentId")]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string? BtnText { get; set; }

        /// <summary>
        /// 记录一个按钮的别名--这个别名是唯一的；专门用在前端判断用户是否具备某个按钮
        /// </summary>
        public string? BtnValue { get; set; }

        /// <summary>
        /// 全名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? FullName { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? ControllerName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? ActionName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Icon { get; set; }

        /// <summary>
        /// 尺寸大小
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Size { get; set; }


        /// <summary>
        /// 背景颜色
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? BackColor { get; set; }

        /// <summary>
        /// 按钮描述
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Description { get; set; }
    }
}
