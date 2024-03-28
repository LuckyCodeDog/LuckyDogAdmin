namespace CMS.IdentityApi.Utility
{
    /// <summary>
    /// cors
    /// </summary>
    public static class CrosServiceExtension
    {
        /// <summary>
        /// config cors policy
        /// </summary>
        /// <param name="builder"></param>
        public static void CrosDomainsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllCrosDomainsPolicy", builder =>
                { 
                    builder.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowAnyOrigin(); 
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void UseCrosDomainsPolicy(this WebApplication app)
        {
            app.UseCors("AllCrosDomainsPolicy");
        }

    }
}
