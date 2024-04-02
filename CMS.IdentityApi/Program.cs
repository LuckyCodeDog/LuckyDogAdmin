
using CMS.BusinessInterface;
using CMS.BusinessService;
using CMS.Common.JwtService;
using CMS.Common.Result;
using CMS.Common.DTO;
using CMS.Common.DTO.user;
using CMS.IdentityApi.Utility;
using CMS.MentApi.Untility.DatabaseExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Zhaoxi.Manage.IdentityApi;
using CMS.BusinessInterface.MapConfig;
namespace CMS.IdentityApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //add swagger
            builder.SwaggerExt();
            //add cors
            builder.CrosDomainsPolicy();
            builder.IniSqlSugar();
             builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleServicce, RoleService>();
            builder.Services.AddScoped<IUserRoleMapService, UserRoleMapService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<IButtonService, ButtonService>();
            builder.Services.AddScoped<IRoleButtonMapService, RoleButtonMapService>();
            builder.Services.AddScoped<IRoleMenuMapService, RoleMenuMapService>();
            builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));
            builder.Services.AddTransient<CustomJWTService, CustomHSJWTService>();
            builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            /*    if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }*/
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCrosDomainsPolicy();
            app.Login();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
