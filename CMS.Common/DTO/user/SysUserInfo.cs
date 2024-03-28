
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Enum;
namespace CMS.Common.DTO.user
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class SysUserInfo
    {
        public int UserId { set; get; }

        public string? Name { set; get; }

        public string? Password { set; get; }

        /// <summary>
        /// 用户类型--UserTypeEnum  
        /// 1:管理员 系统默认生成
        /// 2:普通用户  添加的或者注册的用户都为普通用户
        /// </summary>
        public int UserType { get; set; } = (int)UserTypeEnum.GeneralUser;

        public int Status { set; get; }

        public string? Phone { set; get; }

        public string? Mobile { set; get; }

        public string? Address { set; get; }

        public string? Email { set; get; }

        public long QQ { set; get; }

        public string? WeChat { set; get; }

        public int Sex { set; get; }

        public string? Imageurl { set; get; }

        /// <summary>
        /// 角色Id集合
        /// </summary>
        public List<int>? RoleIdList { get; set; } = new List<int>();

        /// <summary>
        /// 用户拥有菜单
        /// </summary>
        public List<UserContainsMenus>? UserMenuList { get; set; } = new List<UserContainsMenus>();

        /// <summary>
        /// 拥有拥有的按钮
        /// </summary>
        public List<UserContainsMenus>? UserBtnList { get; set; } = new List<UserContainsMenus>();
    }
}
