using CMS.DTO;
using CMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessInterface
{
    public interface IRoleServicce : IBusinessService
    {

      Task<List<TreeSelectDto>> GetRoleMenu(int  roleId);

       Task<bool>  AssignMenus(List<Sys_Menu_List_Dto> menus , int  roleId) ;


        Task<bool> AssignUsers(int roleId, List<int> userIds);

        Task<bool> SetStatus(int roleId); 
    }
}
