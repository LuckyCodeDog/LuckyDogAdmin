using AutoMapper;
using CMS.BusinessInterface;
using CMS.BusinessService;
using CMS.Common.Enum;
using CMS.Common.Result;
using CMS.DTO;
using CMS.MentApi.Untility.DatabaseExt;
using CMS.MentApi.Untility.Filters;
using CMS.MentApi.Untility.SwaggerExt;
using CMS.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq.Expressions;

namespace CMS.MentApi.Controllers
{

    /// <summary>
    /// user role menu 
    /// </summary>
    [Controller]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.v1))]
    [CustomExceptionFilter]
    [Function(MenuType.Menu, "Role Management", "/role/list", "../views/Home/role/list/index.vue")]
    public class RoleController : ControllerBase
    {
        /// <summary>
        /// page query  roles
        /// </summary>
        /// <param name="userRoleMenuServicce"></param>
        /// <param name="mapper"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchWords"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageIndex:int}/{pageSize:int}/{searchwords=null}")]
        [Route("{pageIndex:int}/{pageSize:int}")]
        [Function(MenuType.Button, "Page Query Roles")]
        public async Task<JsonResult> PageQuery([FromServices] IRoleServicce userRoleMenuServicce, [FromServices] IMapper mapper, int pageIndex, int pageSize, string? searchWords = null)
        {
            //define  expression 
            Expression<Func<Sys_Role, bool>> whereFunc;
            if (string.IsNullOrEmpty(searchWords))
            {
                whereFunc = r => r.IsDeleted == false;
            }
            else
            {
                whereFunc = r => r.RoleName.Contains(searchWords) && r.IsDeleted == false;
            }
            PagingData<Sys_Role> pagingRoles = userRoleMenuServicce.QueryPage<Sys_Role>(whereFunc, pageSize, pageIndex, r => r.RoleId);


            return await Task.FromResult(new JsonResult(new ApiResult<PagingData<Sys_RoleDto>>()
            {
                Data = mapper.Map<PagingData<Sys_Role>, PagingData<Sys_RoleDto>>(pagingRoles),
                Success = true,
                Message = "roles paging result"
            }));
        }

        /// <summary>
        /// add a new role 
        /// </summary>
        /// <param name="roleDto"></param>
        /// <param name="UserRoleMenuService"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomValidationActionFilter]
        [Function(MenuType.Button, "Add Roles")]
        public async Task<JsonResult> Add([FromBody] Sys_RoleDto roleDto, [FromServices] IRoleServicce UserRoleMenuService, [FromServices] IMapper mapper)
        {
            //validate  
            Sys_Role roleToAdd = mapper.Map<Sys_RoleDto, Sys_Role>(roleDto);
            Sys_Role returnedRole = await UserRoleMenuService.InsertAsync<Sys_Role>(roleToAdd);
            if (returnedRole != null && returnedRole.RoleId > 0)
            {
                return await Task.FromResult(new JsonResult(new ApiResult()
                {
                    Message = $"add user role: {returnedRole.RoleName} successfully",
                    Success = true,
                }));
            }

            return new JsonResult(new ApiResult()
            {
                Message = "failed to add user ",
                Success = false,
            });
        }


        /// <summary>
        /// delete role 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userRoleMenuServicce"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{roleId:int}")]
        [Function(MenuType.Button, "Delete Role")]
        public async Task<JsonResult> Delete(int roleId, [FromServices] IRoleServicce userRoleMenuServicce)
        {
            Sys_Role roleFound = await userRoleMenuServicce.FindAsync<Sys_Role>(roleId);
            if (roleFound != null && roleFound.RoleId > 0)
            {
                roleFound.IsDeleted = true;
                bool result = await userRoleMenuServicce.UpdateAsync<Sys_Role>(roleFound);

                if (result)
                {
                    return new JsonResult(new ApiResult()
                    {
                        Message = $"succeeded to delete role  {roleFound.RoleName}",
                        Success = true,
                    });
                }
            }

            return new JsonResult(new ApiResult()
            {
                Message = "fail to delete  role ",
                Success = false,
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManageService"></param>
        /// <param name="mapper"></param>
        /// <param name="userRoleMap"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchWords"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("{roleId:int}/{pageIndex:int}/{pageSize:int}/{searchWords}")]
        [Route("{roleId:int}/{pageIndex:int}/{pageSize:int}")]
        [Function(MenuType.Button, "Page Query Users")]
        public async Task<JsonResult> PageQueryUsers([FromServices] IUserService userManageService,[FromServices]IMapper mapper, [FromServices] IUserRoleMapService userRoleMap,int  roleId,int pageIndex, int pageSize, string searchWords = null)
        {
            //get users  id with this role by user id in  user_role_map table => userIdsSelected list 
          
        
            if (roleId <= 0) throw new Exception("role id is invalidate");
            List<int> selectedUserIds =   userRoleMap.Query<Sys_UserRoleMap>(m=>m.RoleId==roleId).Select(m=>m.UserId).ToList();

            //get paging users  in user table 
            Expressionable<Sys_User> funcWhere = Expressionable.Create<Sys_User>();
            Expression<Func<Sys_User,bool>> whereExpress =  funcWhere.AndIF(!string.IsNullOrEmpty(searchWords), u=>u.Name.Contains(searchWords)).ToExpression();
            PagingData<Sys_User> pagingAllUserResult = userManageService.QueryPage<Sys_User>(whereExpress, pageSize, pageIndex, u => u.UserId);

            // return all users  and  all selected userds 
                


            return await Task.FromResult(new JsonResult(new ApiResult<PagingData<Sys_UserDto>>()
            {
                Data = mapper.Map<PagingData<Sys_User>, PagingData<Sys_UserDto>>(pagingAllUserResult),
                Message = "paging quering result ",
                Success = true,
                OValue = selectedUserIds
            }));
        }


        /// <summary>
        /// get  current role`s menu by role id 
        /// </summary>
        /// <param name="roleServicce"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{roleId:int}")]
        public async Task<JsonResult> GetRoleMenu([FromServices]IRoleServicce roleServicce, int roleId)
        {

            List<TreeSelectDto > treeMenuDto = await  roleServicce.GetRoleMenu(roleId);

            return await Task.FromResult(new JsonResult(new ApiResult<List<TreeSelectDto>>() {
              Data =treeMenuDto,
              Message="get current role s menu ",
              Success = true
            }));
        }
        /// <summary>
        /// set menus for current role id 
        /// </summary>
        /// <param name="roleServicce"></param>
        /// <param name="roleId"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        [HttpPost("{roleId:int}")]
        public async Task<JsonResult> SetMenus([FromServices]IRoleServicce roleServicce, int roleId,  List<Sys_Menu_List_Dto> menus)
        {

            bool result =  await roleServicce.AssignMenus(menus,roleId);

            ApiResult apiResult = new ApiResult()
            {
                Message = result == true ? "set menus successfully" : "failed to set menus ",
                Success = result
            };
            return await Task.FromResult(new JsonResult(apiResult));
        }

        /// <summary>
        /// AssignUser for current role 
        /// </summary>
        /// <param name="roleServicce"></param>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpPost("/api/Role/AssignUsers/{roleId:int}")]
        public async Task<JsonResult> AssignUsers([FromServices]IRoleServicce roleServicce,  int roleId, List<int> userIds)
        {
            //validate role id 
            bool resuult =      await roleServicce.AssignUsers(roleId,userIds);
            ApiResult result = new ApiResult()
            {
                Success = resuult,
                Message = resuult == true ? "Assign Users Successfully." : "Failed to Set Users."
            };
            return await Task.FromResult(new JsonResult(result));
        }
    }
}
