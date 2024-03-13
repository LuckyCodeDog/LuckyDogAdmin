using CMS.BusinessInterface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessService
{
    public class UserManageService : BaseService, IUserManageService
    {
        public UserManageService(ISqlSugarClient client) : base(client)
        {
        }
    }
}
