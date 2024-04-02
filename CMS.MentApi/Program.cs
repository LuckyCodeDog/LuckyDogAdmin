
using CMS.MentApi.Untility.SwaggerExt;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Configuration;
using SqlSugar;
using CMS.Models.Entity;
using CMS.BusinessInterface;
using CMS.BusinessService;
using CMS.MentApi.Untility.DatabaseExt;
using CMS.BusinessInterface.MapConfig;
using CMS.MentApi.Untility.RegisterExt;
using CMS.Common.JwtService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using CMS.MentApi.Untility.AuthorizeExt;

namespace CMS.MentApi
{
    /// <summary>
    /// entrance to the App
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //config sqlsugar 
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // Add services to the container.
            if (builder.Configuration["initDb"] == "1")
            {
                builder.InitDb();
            }
            builder.IniSqlSugar();
            // config cros
            builder.AllCrosDomainsPolicy();
            
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleServicce, RoleService>();
            builder.Services.AddScoped<IUserRoleMapService, UserRoleMapService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<IButtonService, ButtonService>();
            builder.Services.AddScoped<IRoleButtonMapService, RoleButtonMapService>();
            builder.Services.AddScoped<IRoleMenuMapService, RoleMenuMapService>();
            builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));
            builder.AddJwtAuthorize();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.AddSwaggerExt();
            var app = builder.Build();
            //cros
            app.UserCrosDomainsPolicy();
            // Configure the HTTP request pipeline.
            /*     if (app.Environment.IsDevelopment())
                 {
                     app.UseSwagger();

                     app.UseSwaggerExt();
                 }*/
            app.UseSwagger();

            app.UseSwaggerExt();
            app.UseHttpsRedirection();
         
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
