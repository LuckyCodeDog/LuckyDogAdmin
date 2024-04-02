using CMS.BusinessInterface;
using CMS.Common.DTO;
using CMS.Common.DTO.user;
using CMS.Models.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Enum;
using CMS.Common.UserStateEnum;
namespace CMS.BusinessService
{
    public class UserService : BaseService, IUserService
    {
        private IMenuService _menuService;
        private IButtonService _buttonService;

        public UserService(ISqlSugarClient client, IMenuService menuService, IButtonService buttonService) : base(client)
        {
            _menuService = menuService;
            _buttonService = buttonService; 
        }

        public async Task<bool> Delete(int userId)
        {
            // find user by user id  
            //  update user s colum  isDeleted = ture 
            Sys_User userToDelete = await this.FindAsync<Sys_User>(userId);
            if (userToDelete != null)
            {
                userToDelete.IsDeleted = true;
                return await this.UpdateAsync<Sys_User>(userToDelete);
            }
            return false;
        }

        public async Task<SysUserInfo?> Login(UserAccount account)
        {
            // 密码加密
          SysUserInfo? user = await _client.Queryable<Sys_User>().Where(u => u.Password == account.Password && u.Email == account.Username).Select(u=> new SysUserInfo()
          {
              UserId = u.UserId,
              Name = u.Name,
              Imageurl = u.ImageUrl,
          }).FirstAsync();
   
        return await Task.FromResult(user);
        }

     

        public async Task<List<CMS.DTO.TreeSelectDto>> ViewMenus(int userId)
        {

            // validate user id 
            if (userId <= 0) throw new Exception("Invalidate User Id");
            Sys_User foundUser = await this.FindAsync<Sys_User>(userId);
            if (foundUser == null) throw new Exception($"Did not find any user with id : {userId}");

            // find buttons ids in role_btn_map by role ids 
          List<Guid> buttonsIds =    _client.Queryable<Sys_User, Sys_UserRoleMap, Sys_Role, Sys_RoleBtnMap, Sys_Button>(
                (u, urm, r, rbm, b) => new object[]
                {
                    JoinType.Inner, u.UserId ==urm.UserId,
                    JoinType.Inner, urm.RoleId == r.RoleId,
                    JoinType.Inner, r.RoleId == rbm.RoleId,
                    JoinType.Inner,  rbm.BtnId == b.Id,
                }

                ).Where((u, urm, r, rbm, b)=> u.UserId == userId&& r.Status == (int)ActiveStateEnum.Active).Select((u, urm, r, rbm, b)=> b.Id).Distinct().ToList();

            //find menus ids in role_menu_map by role ids 
            List<Guid> menusIds = _client.Queryable<Sys_User, Sys_UserRoleMap, Sys_Role, Sys_RoleMenuMap, Sys_Menu>(
           (u, urm, r, rmm, m) => new object[]
           {
                    JoinType.Inner, u.UserId ==urm.UserId,
                    JoinType.Inner, urm.RoleId == r.RoleId,
                    JoinType.Inner, r.RoleId == rmm.RoleId,
                    JoinType.Inner,  rmm.MenuId == m.Id,
           }

           ).Where((u, urm, r, rmm, m) => u.UserId == userId&&r.Status==(int)ActiveStateEnum.Active).Select((u, urm, r, rmm, m) => m.Id).Distinct().ToList();
            // find all menus 
            var menus = _menuService.Query<Sys_Menu>(m => true).Select(m1 => new Sys_Menu()
            {
                Id = m1.Id,
                ParentId = m1.ParentId,
                MenuText = m1.MenuText,
                WebUrlName = m1.WebUrlName,
                VueFilePath = m1.VueFilePath,
                MenuType = m1.MenuType,
                Icon = m1.Icon,
                WebUrl = m1.WebUrl,
                IsLeafNode = m1.IsLeafNode,
                OrderBy = m1.OrderBy,
                CreateTime = m1.CreateTime,
                ModifyTime = m1.ModifyTime,
                IsDeleted = m1.IsDeleted,
                IsEnabled = m1.IsEnabled
            });

            //find all  buttons 
            var buttons = _buttonService.Query<Sys_Button>(s => true).Select(b => new Sys_Menu()
            {
                Id = b.Id,
                ParentId = b.ParentId,
                MenuText = b.BtnText,
                WebUrlName = null,
                VueFilePath = null,
                MenuType = 2,
                Icon = b.Icon,
                WebUrl = null,
                IsLeafNode = true,
                OrderBy = 0,
                CreateTime = b.CreateTime,
                ModifyTime = b.ModifyTime,
                IsDeleted = b.IsDeleted,
                IsEnabled = b.IsEnabled
            });
            // to tree them 
             var menuList =   _client.Union(buttons , menus);
             List<Sys_Menu> menuTree =   await  menuList.ToTreeAsync(m => m.Children, m => m.ParentId, default(Guid));
            return  TreeHelper.TreeHelper.ToSelectedTree(menuTree, buttonsIds, menusIds);
           
        }



        public async Task<bool> ValidateBtnAsync(int userId, string btnValue)
        {
          // get user`s button value  using join
         bool iscontain =  await   _client.Queryable<Sys_UserRoleMap, Sys_RoleBtnMap, Sys_Button>((urm, r, b) =>
            new object[]
            {
                 JoinType.Inner, urm.RoleId ==r.RoleId ,
                 JoinType.Inner, r.BtnId == b.Id,
            }).Where((urm, r, b) =>  urm.UserId.Equals(userId)&&b.FullName==btnValue ).AnyAsync();

            return iscontain;
        }

    }
}
