using CMS.BusinessInterface;
using CMS.Common.Enum;
using CMS.DTO;
using CMS.Models.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CMS.BusinessService
{
    public class RoleService : BaseService, IRoleServicce
    {
        private IRoleMenuMapService _roleMenuMapService;
        private IRoleButtonMapService _roleButtonMapService;
        private IButtonService _buttonService;
        private IMenuService _menuService;
        private IUserRoleMapService _UserRoleMapService;

        public RoleService(ISqlSugarClient client,IRoleMenuMapService roleMenuMapService, IUserRoleMapService userRoleMap, IRoleButtonMapService roleButtonMapService, IButtonService buttonService, IMenuService menuService) : base(client)
        {
            _roleMenuMapService= roleMenuMapService;
            _roleButtonMapService = roleButtonMapService;
            _buttonService = buttonService;
            _menuService = menuService;
            _UserRoleMapService = userRoleMap;
        }

    
        public async Task<List<TreeSelectDto>> GetRoleMenu(int roleId)
        {
            //validate rolid exsist in role table 
            Sys_Role role =  await  this.FindAsync<Sys_Role>(roleId);
            if (role==null) throw new Exception("Role doesnot exsist in system.");

            // get all buttons in button table 
            // get  alll menus in menus table 
            //  con unio two all  reslut lists  and to  tree them 

            
            // get current role`s buttons  by roleId
            // get current role `s menus by roleId 


            // get buttons id   in role_buton table by role id  cast them to Sys_menu 
               List<Guid> buttonIds=  _roleButtonMapService
                                      .Query<Sys_RoleBtnMap>(m => m.RoleId == roleId)
                                      .Select(m=>m.BtnId)
                                      .ToList();
            // get buttons in button table 
          var buttons = _buttonService.Query<Sys_Button>(s=>true).Select(b=>new Sys_Menu()
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
            // get menus in role_menu table by role  id 

            List<Guid> menuIds = _roleMenuMapService
                                   .Query<Sys_RoleMenuMap>(m => m.RoleId == roleId)
                                   .Select(m=>m.MenuId)
                                   .ToList();
            // get menus in menus table 
         var menus =    _menuService.Query<Sys_Menu>(m=> true).Select(m1 =>new Sys_Menu()
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

            var dataList = _client.Union(buttons, menus);
            List<Sys_Menu>  treeDataList = await dataList.ToTreeAsync(m => m.Children, m => m.ParentId, default(Guid));
             List<TreeSelectDto> treeDto =  TreeHelper.TreeHelper.ToSelectedTree(treeDataList, buttonIds, menuIds);
            return  treeDto;
        }

        public async Task<bool> AssignMenus(List<Sys_Menu_List_Dto> menus, int roleId)
        {   
            if (menus != null )
            {   
                // short cut if list is empty 
                if(menus.Count == 0)
                {
                    try
                    {
                        _client.Ado.BeginTran();
                        //delete by role id  in role_button_table  and role_menu_table
                        _client.Deleteable<Sys_RoleMenuMap>().Where(m => m.RoleId == roleId);
                        _client.Deleteable<Sys_RoleBtnMap>().Where(m => m.RoleId == roleId);

                        _client.Ado.CommitTran();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        _client.Ado.RollbackTran();
                        throw ex;
                    }
                }
               
                // divid menus into buttons and menus  buy type
               List<Guid> menuIds  =   menus.Where(m => m.Type == (int)MenuType.Menu).Select(m => Guid.Parse(m.MenuId)).ToList();
               List<Guid> buttonIds  =   menus.Where(m => m.Type == (int)MenuType.Button).Select(m => Guid.Parse(m.MenuId)).ToList();
                // validate buttons id by select them fron buttons_table 
                // validate menus id by select them from menus_table 
                List<Sys_RoleMenuMap> menuMaps = new List<Sys_RoleMenuMap>();
                List<Sys_RoleBtnMap> btnMaps = new List<Sys_RoleBtnMap>();
                if (menuIds.Count>0) 
                {

                    foreach (Guid menuId in menuIds)
                    {
                        Sys_Menu menu = this.Query<Sys_Menu>(m => m.Id == menuId).First();
                        if (menu == null) throw new Exception("contains invalidate menu id");
                        /* menuList.Add(menu);*/
                    }
                    menuMaps = menuIds.Select(menuId => new Sys_RoleMenuMap()
                    {

                        MenuId = menuId,
                        RoleId = roleId
                    }).ToList();
                }
                if(buttonIds.Count >0)
                {
                    foreach (Guid buttonId in buttonIds)
                    {
                        Sys_Button button = this.Query<Sys_Button>(m => m.Id == buttonId).First();
                        if (button == null) throw new Exception("contains invalidate menu id");
                        /*  buttonList.Add(button);*/
                    }
                    btnMaps = buttonIds.Select(buttonId => new Sys_RoleBtnMap()
                    {
                        BtnId = buttonId,
                        RoleId = roleId
                    }).ToList();
                }
                 
            
                // start db trac instert buttons and menus with roleId     
                // delete  all in role_button map  and role _menu _map 
                //insert  
                try
                {
                    _client.Ado.BeginTran();
                   //delete by role id  in role_button_table  and role_menu_table
                      _client.Deleteable<Sys_RoleMenuMap>().Where(m => m.RoleId == roleId).ExecuteCommand();  
                    _client.Deleteable<Sys_RoleBtnMap>().Where(m => m.RoleId ==roleId).ExecuteCommand();
               
                    if (menuMaps!= null&&menuMaps.Count>0) {     await  this.InsertList(menuMaps); }
                    if(btnMaps!= null&&btnMaps.Count>0) {   await this.InsertList(btnMaps); }
                
          
                    _client.Ado.CommitTran();

                    return true;
                }
                catch (Exception ex)
                {
                    _client.Ado.RollbackTran();
                    throw ex;
                }
            }
               return false;

        }

        public async Task<bool> AssignUsers(int roleId, List<int> userIds)
        {
            if(userIds == null||userIds.Contains(0)||userIds.Count!=userIds.Distinct().Count()) throw new Exception("invalidate user ids.");
             Sys_Role role =  await this.FindAsync<Sys_Role>(roleId);
            if (role == null) throw new Exception($"Did not find any role who`s id is {roleId} ");
            if (userIds.Count == 0)
            {
                //shortcut 
                try
                {
                    userIds.ForEach(user =>
                    {
                        _UserRoleMapService.Delete<Sys_User>(user);
                    });

                    return true;
                }catch (Exception ex)
                {
                    throw ex;
                }
            }
            // delete all where  role id  == current id 
            // and insert  current role id - user id  
            List<Sys_UserRoleMap> mapToInsert =   userIds.Select(userId => new Sys_UserRoleMap()
            {
                UserId = userId,
                RoleId = roleId,
            }).ToList();
            try
            {
                _client.Ado.BeginTran();
                //need to refind 
                        _client.Deleteable<Sys_UserRoleMap>().Where(s => s.RoleId == roleId).ExecuteCommand();
                 await this.InsertList<Sys_UserRoleMap>(mapToInsert);

                _client.Ado.CommitTran();
                return true;
            }catch(Exception ex)
            {
                _client.Ado.RollbackTran();
                return false;
            }

        }
    }


}
