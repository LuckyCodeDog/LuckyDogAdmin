using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DTO
{
    /// <summary>
    ///  for retuen 
    /// </summary>
    public class UserRoleInfoDto
    {
        public int user_id { get; set; }

        public string? user_name { get; set; }
    
        public int role_id { get; set; }
        public string? role_name { get; set; }

        public bool selected { get; set; }
    }
}
