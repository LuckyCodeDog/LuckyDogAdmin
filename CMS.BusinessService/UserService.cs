using CMS.BusinessInterface;
using CMS.Models.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessService
{
    public class UserService : BaseService, IUserService
    {
        public UserService(ISqlSugarClient client) : base(client)
        {
        }

        public async Task<bool> Delete(int userId)
        {
            // find user by user id  
            //  update user s colum  isDeleted = ture 
            Sys_User userToDelete = await this.FindAsync<Sys_User>(userId);
            if (userToDelete != null)
            {
                userToDelete.IsDeleted = true;
                return await this.UpdateAsync<Sys_User>(userToDelete);
            }
            return false;
        }
    }
}
