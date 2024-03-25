using AutoMapper;
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
    public class MenuService : BaseService, IMenuService
    {
        private IMapper _mapper;

        public MenuService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public async Task<PagingData<Sys_Menu>> PagingQueryMenu(int pageIndex, int pageSize)
        {
            ISugarQueryable<Sys_Menu> menuList = _client
                .Queryable<Sys_Menu>()
                 .Select(m1 => new Sys_Menu
                 {
                     Id = m1.Id,
                     ParentId = m1.ParentId,
                     MenuText = m1.MenuText,
                     WebUrlName = m1.WebUrlName,
                     VueFilePath = m1.VueFilePath,
                     MenuType = m1.MenuType,
                     Icon = m1.Icon,
                     WebUrl = m1.WebUrl,
                     IsLeafNode = m1.IsLeafNode,
                     OrderBy = m1.OrderBy,
                     CreateTime = m1.CreateTime,
                     ModifyTime = m1.ModifyTime,
                     IsDeleted = m1.IsDeleted,
                     IsEnabled = m1.IsEnabled
                 });

            ISugarQueryable<Sys_Menu> btnList = _client.Queryable<Sys_Button>().Select(b=> new Sys_Menu()
            {
                Id = b.Id,
                ParentId = b.ParentId,
                MenuText = b.BtnText,
                WebUrlName = null,
                VueFilePath = null,
                MenuType = 2,
                Icon = b.Icon,
                WebUrl = null,
                IsLeafNode = true,
                OrderBy = 0,
                CreateTime = b.CreateTime,
                ModifyTime = b.ModifyTime,
                IsDeleted = b.IsDeleted,
                IsEnabled = b.IsEnabled

            }); 

            // form a treee 

          var  treeData = _client.UnionAll(menuList,btnList);
          List<Sys_Menu> treeDataList =   await  treeData.ToTreeAsync(t => t.Children, t => t.ParentId, default(Guid));

          return await Task.FromResult(new PagingData<Sys_Menu>()
          {
              DataList = treeDataList.Skip((pageIndex-1)*pageSize).Take(pageSize) .ToList(),
              PageIndex = pageIndex,
              PageSize = pageSize   
          });

        }

      
    }
}
