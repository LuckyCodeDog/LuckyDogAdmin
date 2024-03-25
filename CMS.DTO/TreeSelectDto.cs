using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DTO
{
    public class TreeSelectDto
    {
        public string? key { get; set; }

        public string? title {  get; set; }

        public int? type { get; set; }  
        public bool Selected { get; set; }

        public List<TreeSelectDto>? Children { get; set; }
    }
}
