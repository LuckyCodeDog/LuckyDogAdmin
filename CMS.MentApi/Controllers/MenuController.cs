using AutoMapper;
using CMS.BusinessInterface;
using CMS.Common.DTO.menu;
using CMS.Common.Enum;
using CMS.Common.Result;
using CMS.DTO;
using CMS.MentApi.Untility.DatabaseExt;
using CMS.MentApi.Untility.Filters;
using CMS.MentApi.Untility.SwaggerExt;
using CMS.Models.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CMS.MentApi.Controllers
{
    /// <summary>
    /// Menu Management Controller
    /// </summary>
    [Route("/api/[controller]/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = nameof(ApiVersions.v1), IgnoreApi = false)]
    [CustomExceptionFilter]
    [Function(MenuType.Menu, "Menu Management", "/menu", "MenuUnfoldOutlined")]
    [Authorize(Policy = "btn")]
    public class MenuController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMenuService _menuManageService;
        /// <summary>
        /// DI 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="menuManageService"></param>
        public MenuController(IMapper mapper, IMenuService menuManageService)
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
        [Function(MenuType.Button, "Page Query Menu")]
        public async Task<ApiResult<PagingData<SysMenuDTO>>> PagingQueryMenu(int pageIndex, int pageSize)
        {
            PagingData<Sys_Menu> pagingTreeMenu = await _menuManageService.PagingQueryMenu(pageIndex, pageSize);
            PagingData<SysMenuDTO> dotMenuTree = _mapper.Map<PagingData<Sys_Menu>, PagingData<SysMenuDTO>>(pagingTreeMenu);

            return new ApiResult<PagingData<SysMenuDTO>>()
            {
                Data = dotMenuTree,
                Success = true,
                Message = "Paging Menu Tree Result"
            };
        }

        /// <summary>
        /// get curren user`s menus 
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetUserMenus([FromServices] IMenuService menuService, [FromServices] IMapper mapper)
        {
            string? strUserId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
            if (strUserId.IsNullOrEmpty())
            {
                return new JsonResult(new ApiResult<string>()
                {
                    Message = "Dont Have the Permission.",
                    Success = false,
                    OValue = 401
                });
            }
            List<Sys_Menu> treeMneus = await menuService.GetUserMenus(int.Parse(strUserId!));
            List<SysMenuDTO> menuDTOs = mapper.Map<List<SysMenuDTO>>(treeMneus);


            return new JsonResult(new ApiResult<List<SysMenuDTO>>()
            {
                Data = menuDTOs,
                Success = true,
                Message = "Current User`s Menu."
            });
        }

        /// <summary>
        /// DeleteMenu
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete]
        [Route("{menuid}")]
        [AllowAnonymous]
        public async Task<JsonResult> DelteMenu([FromServices] IMenuService menuService, string menuId)

        {
            if (menuId == null) { throw new Exception("Menu Id is Invalid."); }
            ApiResult apiResult = new ApiResult() {

                Success = false,
                Message = "Failed to Delete the Menu."
            };
            bool falg = await menuService.DeleteMenu(Guid.Parse(menuId));
            if (falg)
            {
                apiResult.Success = true;
                apiResult.Message = "Delete Menu Successful.";

            }

            return new JsonResult(apiResult);

        }

        /// <summary>
        /// View Roles
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="menuId"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Menu/ViewRoles/{menuId:guid}/{menuType:int}")]
        [AllowAnonymous]
        public async Task<JsonResult> ViewMenuRoles([FromServices] IMenuService menuService, Guid menuId,int menuType)
        {
            
           
             List<MenuRoleInfoDto> selectedRoles   =    await menuService.ViewMenuRoles(menuId,menuType);

            return new JsonResult(new ApiResult<List<MenuRoleInfoDto>>()
            {
                Data = selectedRoles,
                Success = true,
                Message = " Selected Roles."
            });
        }

        /// <summary>
        /// Set Roles
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="rolesToSet"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut]
        [AllowAnonymous]
        public async Task<JsonResult> SetRoles([FromServices] IMenuService menuService, RolesToSetDto rolesToSet)
        {
            if(rolesToSet == null || rolesToSet.roleId ==null ||rolesToSet.menuId == null||rolesToSet.roleId.Contains(0))
            {
                throw new Exception("Contians Invalid Params.");
            }
            bool result =      await  menuService.SetRoles(rolesToSet.roleId, rolesToSet.menuId, rolesToSet.menuType);

            return new JsonResult(new ApiResult()
            {
                Message = result == true ? "Set Roles Successful." : "Failed To Set Roles.",
                Success = result
            });
        }

        /// <summary>
        /// Get All Menus Info
        /// </summary>
        /// <param name="menuService"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/Menu/All")]
        public async Task<JsonResult> GetAllMenus([FromServices]IMenuService menuService)
        {
                List<Sys_Menu>  menus =    await  menuService.GetAllMenus();

            //add Dto later
            return new JsonResult(new ApiResult<List<Sys_Menu>>()
            {
                Data = menus,
                Success = true,
                Message = "All Menus Info"
            });
                

        }


        /// <summary>
        /// Add Menu
        /// </summary>
        /// <param name="menuService"></param>
        /// <param name="menuToAddDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> AddMenu([FromServices]IMenuService menuService,MenuToAddDto  menuToAddDto)
        {
            bool   result  =  await menuService.AddMenu(menuToAddDto);
            return new JsonResult(new ApiResult()
            {
                Message = result == true ? "Add Menu Successful." : "Failed To Add Menu.",
                Success = result
            });
        }
    } 
}
