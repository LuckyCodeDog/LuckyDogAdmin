using AutoMapper;
using CMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.DTO;
using CMS.Common.DTO.user;
using CMS.DTO;
using CMS.Common.DTO.menu;
namespace CMS.BusinessInterface.MapConfig
{
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {

            CreateMap<Sys_User, Sys_UserDto>().ReverseMap();

            CreateMap<PagingData<Sys_User>, PagingData<Sys_UserDto>>().ReverseMap();

            CreateMap<Sys_Role, Sys_RoleDto>().ReverseMap();

            CreateMap<PagingData<Sys_Role>, PagingData<Sys_RoleDto>>().ReverseMap();

            CreateMap<PagingData<Sys_Menu>, PagingData<SysMenuDTO>>().ReverseMap();

            CreateMap<Sys_Menu,SysMenuDTO>().ReverseMap();  

        }
    }
}
