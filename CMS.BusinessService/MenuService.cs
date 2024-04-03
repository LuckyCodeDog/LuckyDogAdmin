using AutoMapper;
using CMS.BusinessInterface;
using CMS.Common.DTO.menu;
using CMS.Common.Enum;
using CMS.Models.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessService
{
    public class MenuService : BaseService, IMenuService
    {
        private IMapper _mapper;

        public MenuService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public async Task<bool> AddMenu(MenuToAddDto menuToAddDto)
        {
            //tell if it is leaf , need to be without submenus 
            Sys_Menu menuToInsert = new Sys_Menu();
            if (menuToAddDto.ParentId == Guid.Empty)
            {
                    //add a  new menu direactly  
                    menuToInsert.ParentId = Guid.Empty;
                    menuToInsert.Id =  Guid.NewGuid();
                    menuToInsert.MenuText = menuToAddDto.MenuText;
                    menuToInsert.IsLeafNode = true;
                    menuToInsert.Icon = menuToAddDto.Icon;  
                    menuToInsert.MenuType = (int)MenuType.Menu;
                    menuToInsert.CreateTime = DateTime.Now;
                    return   await _client.Insertable<Sys_Menu>(menuToInsert).ExecuteCommandIdentityIntoEntityAsync();
            }

            // validate if parent menu exsist in database 
            Sys_Menu menuInDb = await _client.Queryable<Sys_Menu>().Where(m => m.Id == menuToAddDto.ParentId).FirstAsync();
            if (menuInDb == null)
            {
                throw new Exception("Parent Menus Does not Exsist!");
            }
            menuToInsert.ParentId = menuToAddDto.ParentId;
            menuToInsert.Id = Guid.NewGuid();
            menuToInsert.MenuText = menuToAddDto.MenuText;
            menuToInsert.IsLeafNode = true;
            menuToInsert.Icon = menuToAddDto.Icon;
            menuToInsert.MenuType = (int)MenuType.Menu;
            menuToInsert.CreateTime = DateTime.Now;
            // tracn begiin 
            try
            {
                _client.Ado.BeginTran();
                menuInDb.IsLeafNode = false;
                await _client.Updateable<Sys_Menu>(menuInDb).ExecuteCommandAsync();
                await _client.Insertable<Sys_Menu>(menuToInsert).ExecuteCommandAsync();
                _client.Ado.CommitTran();
                return true;
            }catch (Exception ex)
            {
               await  _client.Ado.RollbackTranAsync();
               throw  new Exception("Failed To Add Menu.",ex);
            }
       
        }

        public async Task<bool> DeleteMenu(Guid menuId)
        {
            //validate  if it is null find in menu_btn_map  if i has buttons  return  false 
            // tell if it is leafnode
            Sys_Menu menu = await _client.Queryable<Sys_Menu>().Where(m => m.Id == menuId).FirstAsync();
            if (menu == null) throw new Exception("Menu Does not Exsist.");
            List<Guid> menuIdsOfBtn = await _client.Queryable<Sys_Button>().Where(b => true).Select(b => b.ParentId).ToListAsync();
            //need to st parent leaf node ture if IT donesn1t have any sub menus
            if (menuIdsOfBtn.Any(id => id == menuId) == true)
            {
                throw new Exception("This Menu Contains Buttons Can not Delete.");
            }
            return await _client.Deleteable<Sys_Menu>(menu).ExecuteCommandHasChangeAsync();
        }

        public async  Task<List<Sys_Menu>> GetAllMenus()
        {
            return  await _client.Queryable<Sys_Menu>().Where(m=>true).ToListAsync();
        }

        public async Task<List<Sys_Menu>> GetUserMenus(int userId)
        {
            return await _client.Queryable<Sys_UserRoleMap, Sys_RoleMenuMap, Sys_Menu>((urm, rmm, m) => new object[]
               {
                 JoinType.Inner, urm.RoleId == rmm.RoleId,
                 JoinType.Inner, rmm.MenuId == m.Id,
               }).Where((urm, rmm, m) => urm.UserId == userId).Select((urm, rmm, m) => m).ToTreeAsync(m => m.Children, m => m.ParentId, default(Guid));
        }

        public async Task<PagingData<Sys_Menu>> PagingQueryMenu(int pageIndex, int pageSize)
        {
            ISugarQueryable<Sys_Menu> menuList = _client
                .Queryable<Sys_Menu>()
                 .Select(m1 => new Sys_Menu
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

            ISugarQueryable<Sys_Menu> btnList = _client.Queryable<Sys_Button>().Select(b => new Sys_Menu()
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

            // form a treee 

            var treeData = _client.UnionAll(menuList, btnList);
            List<Sys_Menu> treeDataList = await treeData.ToTreeAsync(t => t.Children, t => t.ParentId, default(Guid));

            return await Task.FromResult(new PagingData<Sys_Menu>()
            {
                DataList = treeDataList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            });

        }

        public async Task<bool> SetRoles(List<int> roleIds, string menuId, int menuType)
        {

            // validate all role ids in database 
            List<Sys_Role> roleList = new List<Sys_Role>();
            if (roleList.Count > 0)
            {
                foreach (int roleId in roleIds)
                {
                    Sys_Role role = await _client.Queryable<Sys_Role>().Where(r => r.RoleId == roleId).FirstAsync();
                    if (role == null)
                    {
                        throw new Exception("Role Ids contain Invalid ones.");
                    }
                }

            }
         

            

            if (menuType == (int) MenuType.Button)
            {
                //validate menuid 
               Sys_Button btn = await   _client.Queryable<Sys_Button>().Where(b => b.Id == Guid.Parse(menuId)).FirstAsync();
                if (btn == null) throw new Exception("Menu id is invalid.");

                // tranc in role btn map 
                try
                {
                    //delte  all in role_btn_map with menuId 
                    //insert  role id - menu id  in role_btn_map 
                    _client.Ado.BeginTran();
                     await  _client.Deleteable<Sys_RoleBtnMap>().Where(b=>b.BtnId == Guid.Parse(menuId)).ExecuteCommandAsync();
                    if(roleIds.Count > 0)
                    {
                        foreach (int roleId in roleIds)
                        {
                            await _client.Insertable<Sys_RoleBtnMap>(new Sys_RoleBtnMap()
                            {
                                RoleId = roleId,
                                BtnId = btn.Id,
                            }).ExecuteCommandAsync();
                        }
                    }
                    //commit tranc 
                    _client.Ado.CommitTran();
                    return true;

                }catch (Exception ex)
                {
                    //catch error and roll back 
                    _client.Ado.RollbackTran();
                    throw new Exception("Failed to Set Roles", ex);
                }
               
              
      
            }
            if (menuType == (int)MenuType.Menu)
            {
                //validate menuid 
                Sys_Menu  menu = await _client.Queryable<Sys_Menu>().Where(b => b.Id == Guid.Parse(menuId)).FirstAsync();
                if (menu == null) throw new Exception("Menu id is invalid.");
                // tranc in role btn map 
                try
                {
                    //delte  all in role_btn_map with menuId 
                    //insert  role id - menu id  in role_btn_map 
                    _client.Ado.BeginTran();
                    await _client.Deleteable<Sys_RoleMenuMap>().Where(b => b.MenuId == Guid.Parse(menuId)).ExecuteCommandAsync();
                    if(roleIds.Count>0)
                    {
                        foreach (int roleId in roleIds)
                        {
                            await _client.Insertable<Sys_RoleMenuMap>(new Sys_RoleMenuMap()
                            {
                                RoleId = roleId,
                                MenuId = menu.Id,
                            }).ExecuteCommandAsync();
                        }
                    }
                    //commit tranc 
                    _client.Ado.CommitTran();
                    return true;

                }
                catch (Exception ex)
                {
                    //catch error and roll back 
                    _client.Ado.RollbackTran();
                    throw new Exception("Failed to Set Roles", ex);
                }

            }
           return false;
        }

        public async Task<List<MenuRoleInfoDto>> ViewMenuRoles(Guid menuId, int menuType)
            
        {
            Sys_Menu menu = new Sys_Menu();
            Sys_Button button = new Sys_Button();
            List<int> roleIds = new List<int>();
            List<Sys_Role> roles = await _client.Queryable<Sys_Role>().Where(r => true).ToListAsync();
            List<MenuRoleInfoDto> selectedRoles = new List<MenuRoleInfoDto>();
            if (menuType == (int)MenuType.Menu)
            {
                //validate if menu exsist 
                menu = this.Query<Sys_Menu>(m => m.Id == menuId).First();

                if (menu == null) throw new Exception("Menu Id is invalid.");
                // get role ids in menu_role_map 
                  roleIds = await _client.Queryable<Sys_RoleMenuMap>()
                    .Where(m => m.MenuId == menuId)
                    .Select(m => m.RoleId).ToListAsync();
                //get all  roles in roles table 

                // to the  menurole info dto 
               selectedRoles = roles.Select(r => new MenuRoleInfoDto()
                {
                    menuId = menuId,
                    roleId = r.RoleId,
                    menuText = menu.MenuText,
                    roleName = r.RoleName,
                    selected = roleIds.Contains(r.RoleId)
                }).ToList();

                return selectedRoles;
            }
            else if(menuType == (int)MenuType.Button)
            {
                button  = this.Query<Sys_Button>(b=>b.Id== menuId).First();
                if (button == null) throw new Exception("Menu Id is invalid.");
                roleIds = await _client.Queryable<Sys_RoleBtnMap>()
                  .Where(m => m.BtnId == menuId)
                  .Select(m => m.RoleId).ToListAsync();
                 selectedRoles = roles.Select(r => new MenuRoleInfoDto()
                {
                    menuId = menuId,
                    roleId = r.RoleId,
                    menuText = menu.MenuText,
                    roleName = r.RoleName,
                    selected = roleIds.Contains(r.RoleId)
                }).ToList();

                return selectedRoles;
            }
            else
            {
                throw new Exception("Invalid MenuType");
            }
        }
    }
}
