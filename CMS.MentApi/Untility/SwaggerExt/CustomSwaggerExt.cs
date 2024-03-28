using Microsoft.OpenApi.Models;

namespace CMS.MentApi.Untility.SwaggerExt
{/// <summary>
/// to config swagger
/// </summary>
    public static class CustomSwaggerExt
    {
        public static void AddSwaggerExt(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    option.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = $"CMS api doc",
                        Version = version.ToString(),
                        Description = $"net core api {version}"
                    });
                });

                var file = Path.Combine(AppContext.BaseDirectory, "CMS.MentApi.xml");

                option.IncludeXmlComments(file, true);

                /* option.OrderActionsBy*/

                //添加安全定义
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入token,格式为 Bearer xxxxxxxx（注意中间必须有空格）",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                //添加安全要求
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {
                        new OpenApiSecurityScheme
                        {
                            Reference =new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });
        }


        public static void UseSwaggerExt(this WebApplication app)
        {
            app.UseSwaggerUI(option =>
            {
                foreach (string version in typeof(ApiVersions).GetEnumNames())
                {
                    option.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"this is {version}");


                }
            });
        }
    }
}
