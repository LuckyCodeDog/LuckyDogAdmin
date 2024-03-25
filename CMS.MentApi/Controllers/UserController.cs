using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using CMS.Models.Entity;
using CMS.BusinessInterface;
using CMS.MentApi.Untility.SwaggerExt;
using CMS.Common.Result;
using AutoMapper;
using CMS.DTO;
using CMS.MentApi.Untility.Filters;
using CMS.MentApi.Untility.DatabaseExt;
using System.Linq.Expressions;
using CMS.Common.UserStateEnum;
using SqlSugar;
using CMS.Common.Enum;
namespace CMS.MentApi.Controllers
{
    /// <summary>
    /// user 
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.v1))]
    [CustomExceptionFilter]
    [Function(MenuType.Menu, "User Management", "/user/info", "../views/Home/user/index.vue")]
    public class UserController : ControllerBase
    {
        private IUserService _userManageService;

        private readonly IMapper _mapper;

        /// <summary>
        /// inject service
        /// </summary>
        /// <param name="userManageService"></param>
        public UserController(IUserService userManageService, IMapper mapper)
        {
            _userManageService = userManageService;
            _mapper = mapper;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchByName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageIndex:int}/{pageSize:int}/{searchByName}")]
        [Route("{pageIndex:int}/{pageSize:int}")]
        [Function(MenuType.Button, "Page Query Users")]

        public async Task<JsonResult> PageQuery(int pageIndex, int pageSize, string? searchByName = null)
        {
            //if search words is null or not 
            //create a expression  to if searchByname is null then is null or it is s=>s.name.contains(searchByName)
            Expression<Func<Sys_User, bool>> funcWhere;
            if (string.IsNullOrEmpty(searchByName))
            {
                funcWhere = s => s.IsDeleted == false;
            }
            else
            {
                funcWhere = s => s.Name.Contains(searchByName) && s.IsDeleted == false;
            }
            PagingData<Sys_User> pagingData = _userManageService.QueryPage<Sys_User>(funcWhere, pageSize, pageIndex, s => s.UserId);
            // map to dto 
            return await Task.FromResult(new JsonResult(new ApiResult<PagingData<Sys_UserDto>>()
            {
                Data = _mapper.Map<PagingData<Sys_User>, PagingData<Sys_UserDto>>(pagingData),                       // _mapper.Map<PagingData<Sys_UserDto>>(pagingData),
                Success = true,
                Message = "returned paging result",
                OValue = null
            }));
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomValidationActionFilter]
        [Function(MenuType.Button, "Add User")]
        public async Task<JsonResult> Add(Sys_UserDto userDto)
        {
            Sys_User user = _mapper.Map<Sys_User>(userDto);
            Sys_User returnedUser = await _userManageService.InsertAsync<Sys_User>(user);
            Sys_UserDto returnedUserDto = _mapper.Map<Sys_UserDto>(returnedUser);
            return new JsonResult(new ApiResult<Sys_UserDto>()
            {
                Data = returnedUserDto,
                Success = true,
                Message = $"successfully add user: {returnedUserDto.Name}"
            });
        }


        /// <summary>
        /// to freeze or activate user
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPut("{userid:int}")]
        [CustomValidationActionFilter]
        [Function(MenuType.Button, "Freeze or Activate User")]
        public async Task<JsonResult> HandleUserFreeze(int userid)
        {
            Sys_User userToHandle = await _userManageService.FindAsync<Sys_User>(userid);
            if (userToHandle != null)
            {
                userToHandle.Status = userToHandle.Status == (int)ActiveStateEnum.Active ? (int)ActiveStateEnum.Fronzen : (int)ActiveStateEnum.Active;
                await _userManageService.UpdateAsync<Sys_User>(userToHandle);
                string finalStatus = userToHandle.Status == 0 ? "Activeated" : "Fronzen";
                return new JsonResult(new ApiResult()
                {
                    Message = $@"user: {userToHandle.Name} status has {finalStatus}",
                    Success = true,
                });
            }

            return new JsonResult(new ApiResult()
            {
                Message = $@"failed to cahnge user: {userToHandle?.Name} Status ",
                Success = false,
            });
        }

        /// <summary>
        /// delete user set isdeletd  = true
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{userId:int}")]
        [Function(MenuType.Button, "Delete User")]
        public async Task<JsonResult> Delete(int userId)
        {

            bool success = await _userManageService.Delete(userId);
            ApiResult result = new ApiResult()
            {
                Message = "cont find user, failed to delete",
                Success = success,
            };

            if (success)
            {
                result.Message = "Deleted User Successfully";
                return new JsonResult(result);

            }
            return new JsonResult(result);

        }

        /// <summary>
        /// get user`s roles with selected
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userRoleMenuServicce"></param>
        /// <param name="userRoleMapService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId:int}")]
        [Function(MenuType.Button, "Get User`s Role")]
        public async Task<JsonResult> GetUserRole(int userId, [FromServices] IRoleServicce userRoleMenuServicce, [FromServices] IUserRoleMapService userRoleMapService)
        {
            // get all roles_name and roles_id  in role_table and put them in UserRoleMap formy
            List<Sys_Role> totalRoles = userRoleMenuServicce.Set<Sys_Role>().Where(r=>r.IsDeleted !=true ).ToList();

            // to tell if  it is selected by the user 

            //get  role_ids  in user_role_map table by userId
            List<int> selectedRoleId = userRoleMapService.Query<Sys_UserRoleMap>(u => u.UserId == userId).Select(u => u.RoleId).ToList();
            //get selected role names  from role_table by roles is from above result 
            List<String> selectedRoleNmaes = new List<string>();
            if (selectedRoleId.Count > 0)
            {
                foreach (int roleId in selectedRoleId)
                {
                    Sys_Role selectedRole = await userRoleMenuServicce.FindAsync<Sys_Role>(roleId);
                    selectedRoleNmaes.Add(selectedRole.RoleName);
                }

            }
            List<UserRoleInfoDto> finalResult = totalRoles.Select(r => new UserRoleInfoDto()
            {
                role_id = r.RoleId,
                role_name = r.RoleName,
                selected = selectedRoleNmaes.Any(n => n == r.RoleName),
                user_id = userId,
            }).ToList();

            return new JsonResult(new ApiResult<List<UserRoleInfoDto>>
            {
                Data = finalResult,
                Message = "user  role info with selected roles",
                Success = true
            });



            //form user role map dto result from above


        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="userRoleInfo"></param>
      /// <param name="userRoleMapService"></param>
      /// <param name="userRoleMenuServicce"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
        [HttpPost("/api/User/HandleRoles")]
        [Function(MenuType.Button,"Set Roles")]
        public async Task<JsonResult> HandleRoles(List<UserRoleInfoDto> userRoleInfo, [FromServices] IUserRoleMapService userRoleMapService, [FromServices] IRoleServicce userRoleMenuServicce)
        {
            //validate if they are all in db 
            if (userRoleInfo.Count <= 0 || userRoleInfo.Any(u => u.role_id <= 0)) throw new Exception("params are in validate");
            List<int> roleIds = userRoleInfo.Where(u => u.selected == true).Select(u => u.role_id).ToList();


            foreach (int roleId in roleIds)
            {
                Sys_Role role = await userRoleMenuServicce.FindAsync<Sys_Role>(roleId);
                if (role == null || role.RoleId <= 0)
                {
                    throw new Exception("cant find  this role ");
                }
            }
            ApiResult result = new ApiResult()
            {
                Message = "failed to handle roles",
                Success = false,
            };

            int userId = userRoleInfo.FirstOrDefault().user_id;
            //
            bool success = await userRoleMapService.HandleUserRoles(roleIds, userId);

            if (success)
            {
                result.Success = true;
                result.Message = "successfully to handle roles";
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }
    }
}
