using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.DTO.menu
{
    public class RolesToSetDto
    {
        public string? menuId { get; set; }

        public int menuType { get; set; }

        public List<int>? roleId { get; set; }
    }
}
