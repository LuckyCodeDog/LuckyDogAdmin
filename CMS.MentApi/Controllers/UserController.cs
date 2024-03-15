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
namespace CMS.MentApi.Controllers
{
    /// <summary>
    /// user 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.v1))]
    [CustomExceptionFilter]
    [MenuOrButton(MenuType.Menu, "User Management")]
    public class UserController : ControllerBase
    {
        private IUserManageService _userManageService;

        private readonly IMapper _mapper;
        /// <summary>
        /// inject service
        /// </summary>
        /// <param name="userManageService"></param>
        public UserController(IUserManageService userManageService, IMapper mapper)
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
        [MenuOrButton(MenuType.Button, " User")]

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
        [MenuOrButton(MenuType.Button, "Add User")]
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
        [MenuOrButton(MenuType.Button, "Freeze or Activate User")]
        public async Task<JsonResult> HandleUserFreeze(int userid)
        {
            Sys_User userToHandle = await _userManageService.FindAsync<Sys_User>(userid);
            if (userToHandle != null)
            {
                userToHandle.Status = userToHandle.Status == (int)UserStateEnum.Active ? (int)UserStateEnum.Fronzen : (int)UserStateEnum.Active;
                await _userManageService.UpdateAsync<Sys_User>(userToHandle);

                return new JsonResult(new ApiResult()
                {
                    Message = "user status has changed",
                    Success = true,
                });
            }

            return new JsonResult(new ApiResult()
            {
                Message = "failed to cahnge state",
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
        [MenuOrButton(MenuType.Button, "Delete User")]
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
    }
}
