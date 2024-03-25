using CMS.BusinessInterface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessService
{
    public class RoleButtonMapService : BaseService, IRoleButtonMapService
    {
        public RoleButtonMapService(ISqlSugarClient client) : base(client)
        {

        }

    }
}
