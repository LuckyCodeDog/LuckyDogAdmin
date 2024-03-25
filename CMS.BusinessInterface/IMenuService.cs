using CMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessInterface
{
    public interface IMenuService : IBusinessService
    {
        Task<PagingData<Sys_Menu>> PagingQueryMenu(int pageIndex, int pageSize);
    }
}
