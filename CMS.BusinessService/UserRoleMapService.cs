using CMS.BusinessInterface;
using CMS.Models.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessService
{
    public class UserRoleMapService : BaseService, IUserRoleMapService
    {

        public UserRoleMapService(ISqlSugarClient client) : base(client)
        {
        }

        public async Task<bool> HandleUserRoles(List<int> roleIds, int userId)
        {
            List<Sys_UserRoleMap> mapList = roleIds.Select(id => new Sys_UserRoleMap()
            {
                RoleId = id,
                UserId = userId,
                CreateTime = DateTime.Now,
                IsEnabled = true,
                IsDeleted = false,
                ModifyTime = DateTime.Now,
            }).ToList();
            try
            {
                _client.Ado.BeginTran();
                List<Sys_UserRoleMap> foundMap = this.Query<Sys_UserRoleMap>(m => m.UserId == userId).ToList();

                bool flag = true;
                bool flag2 = true;
                if (foundMap.Count > 0)
                {
                    flag = this.DeleteList<Sys_UserRoleMap>(foundMap);
                }
                if (mapList.Count > 0)
                {
                    flag2 = await this.InsertList<Sys_UserRoleMap>(mapList);
                }
                _client.Ado.CommitTran();
                return flag && flag2;

            }
            catch (Exception ex)
            {
                _client.Ado.RollbackTran();
                throw ex;
            }

        }

    }
}
