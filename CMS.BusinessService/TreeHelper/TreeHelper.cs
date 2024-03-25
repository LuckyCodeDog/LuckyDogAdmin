using CMS.Common.Enum;
using CMS.DTO;
using CMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessService.TreeHelper
{
    public static class TreeHelper
    {
   
        public static List<TreeSelectDto> ToSelectedTree(List<Sys_Menu> menuList, List<Guid> buttonIds, List<Guid> menuIds)
        {
            return menuList.Select(m => new TreeSelectDto()
            {
                key = m.Id.ToString(),
                title = m.MenuText,
                type = m.MenuType,
                Selected = m.MenuType == (int)MenuType.Menu ? menuIds.Contains(m.Id) : buttonIds.Contains(m.Id),
                Children = m.Children == null ? null : ToSelectedTree(m.Children, buttonIds, menuIds),
            }).ToList();
        }


    }
}
