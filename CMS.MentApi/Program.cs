
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
            builder.InitDb();
            builder.IniSqlSugar();
            builder.Services.AddScoped<IUserManageService, UserManageService>();


            builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.AddSwaggerExt();
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerExt();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
