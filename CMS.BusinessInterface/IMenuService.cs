using CMS.Common.DTO.menu;
using CMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessInterface
{
    public interface IMenuService : IBusinessService
    {
        Task<PagingData<Sys_Menu>> PagingQueryMenu(int pageIndex, int pageSize);

        Task<List<Sys_Menu>> GetUserMenus(int userId);

        Task<bool> DeleteMenu(Guid menuId);

        Task<List<MenuRoleInfoDto>> ViewMenuRoles(Guid menuId, int menuType);

        Task<bool> SetRoles(List<int> roleIds, string menuId, int menuType);

        Task<List<Sys_Menu>> GetAllMenus();

        Task<bool> AddMenu(MenuToAddDto menuToAdd);
    }
}
