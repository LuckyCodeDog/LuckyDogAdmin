using AutoMapper;
using CMS.BusinessInterface;
using CMS.Common.Enum;
using CMS.Common.Result;
using CMS.DTO;
using CMS.MentApi.Untility.DatabaseExt;
using CMS.MentApi.Untility.Filters;
using CMS.MentApi.Untility.SwaggerExt;
using CMS.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.MentApi.Controllers
{
    /// <summary>
    /// Menu Management Controller
    /// </summary>
    [Route("/api/[controller]/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = nameof(ApiVersions.v1), IgnoreApi = false)]
    [CustomExceptionFilter]
    [Function(MenuType.Menu, "Menu Management", "/menu", "../views/Home/menu/index.vue")]
    public class MenuController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMenuService _menuManageService;
        /// <summary>
        /// DI 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="menuManageService"></param>
        public MenuController(IMapper mapper,IMenuService menuManageService)
        {
            _mapper = mapper;
            _menuManageService = menuManageService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageIndex:int}/{pageSize:int}")]
        [Function(MenuType.Button,"Page Query Menu")]
        public async Task<ApiResult<PagingData<SysMenuDTO>>> PagingQueryMenu(int pageIndex, int pageSize)
        {
              PagingData<Sys_Menu> pagingTreeMenu =    await  _menuManageService.PagingQueryMenu(pageIndex, pageSize);
              PagingData<SysMenuDTO>   dotMenuTree=   _mapper.Map<PagingData<Sys_Menu> ,PagingData<SysMenuDTO>>(pagingTreeMenu);

            return new ApiResult<PagingData<SysMenuDTO>>()
            {
                Data = dotMenuTree,
                Success = true,
                Message = "Paging Menu Tree Result"
            };
        }

    }
}
