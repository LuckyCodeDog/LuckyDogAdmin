using AutoMapper;
using CMS.DTO;
using CMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessInterface.MapConfig
{
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {

            CreateMap<Sys_User, Sys_UserDto>().ReverseMap();


            CreateMap<PagingData<Sys_User>, PagingData<Sys_UserDto>>().ReverseMap();


        }
    }
}
