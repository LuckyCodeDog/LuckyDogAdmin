using CMS.Common.DTO;
using CMS.Common.DTO.user;
using CMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DTO;

namespace CMS.BusinessInterface
{
    public interface IUserService : IBusinessService
    {
        Task<bool> Delete(int userId);
        Task<List<TreeSelectDto>> ViewMenus(int userId);

        Task<SysUserInfo?> Login(UserAccount account);

        Task<bool> ValidateBtnAsync(int userId, string btnValue);
    }
}
