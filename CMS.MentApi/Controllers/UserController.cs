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
using System.Net.NetworkInformation;
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
        /// get a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        [MenuOrButton(MenuType.Button, "Find User")]
        public IActionResult Get(int id)
        {


            Sys_User user = _userManageService.Find<Sys_User>(id);
            return new JsonResult(new ApiResult<Sys_User>()
            {
                Data = user,
                Message = user == null ? " user not fount" : null,
            });
        }


        [HttpDelete]
        [CustomValidationActionFilter]
        [MenuOrButton(MenuType.Button, "Delete User")]
        public IActionResult Delete(Sys_UserDto user)
        {
            return Ok(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchByName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageindex:int}/{pageSize:int}/{searchaString}")]
        [Route("{pageindex:int}/{pageSize:int}")]
        [MenuOrButton(MenuType.Button, "Query User info")]
        public async Task<JsonResult> PageQuery(int pageIndex, int pageSize, string? searchByName = null)
        {
            //if search words is null or not 
            //create a expression  to if searchByname is null then is null or it is s=>s.name.contains(searchByName)
            Expression<Func<Sys_User, bool>> funcWhere;
            if (string.IsNullOrEmpty(searchByName))
            {
                funcWhere = null;
            }
            else
            {
                funcWhere = s => s.Name.Contains(searchByName);
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
    }
}
