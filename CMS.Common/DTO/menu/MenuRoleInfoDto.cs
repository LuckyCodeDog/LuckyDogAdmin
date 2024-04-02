using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.DTO.menu
{
    public class MenuRoleInfoDto
    {
        public Guid menuId { get; set; }

        public string? menuText { get; set; }

        public int roleId { get; set; }
        public string? roleName { get; set; }

        public bool selected { get; set; }
    }
}
