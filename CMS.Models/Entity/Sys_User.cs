using SqlSugar;

namespace CMS.Models.Entity
{
    /// <summary>
    ///  用户信息
    /// </summary>
    [SugarTable("Sys_User")]
    public class Sys_User : Sys_BaseModel
    {
        [SugarColumn(ColumnName = "UserId", IsIdentity = true, IsPrimaryKey = true)]
        public int UserId { get; set; }
        public string? Name { set; get; }
        public string? Password { set; get; }

        /// <summary>
        /// 用户状态  0正常 1冻结 2删除
        /// </summary>
        public int Status { set; get; }
        public string? Phone { set; get; }

        public string? Mobile { set; get; }

        public string? Address { set; get; }

        public string? Email { set; get; }

        public int Sex { set; get; }
        public string? ImageUrl { set; get; }
        public DateTime? LastLoginTime { set; get; } = DateTime.Now;
    }
}
