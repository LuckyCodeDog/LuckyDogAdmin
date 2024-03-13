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
