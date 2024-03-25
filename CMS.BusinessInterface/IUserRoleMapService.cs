using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessInterface
{
    public interface IUserRoleMapService : IBusinessService
    {
        Task<bool> HandleUserRoles(List<int> roleIds, int userId);
    }
}
