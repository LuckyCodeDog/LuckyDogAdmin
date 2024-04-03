using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.DTO.menu
{
    public class MenuToAddDto
    {
        public Guid ParentId { get; set; }

        public string? WebUrl {  get; set; }

        public string? MenuText { get; set; }

        public string? Icon {  get; set; }
    }
}
