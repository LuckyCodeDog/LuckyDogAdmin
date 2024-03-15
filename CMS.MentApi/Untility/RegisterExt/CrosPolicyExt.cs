namespace CMS.MentApi.Untility.RegisterExt
{
    /// <summary>
    /// config cros policy
    /// </summary>
    public static class CrosPolicyExt
    {
         public static void AllCrosDomainsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllCrosDomainsPolicy", option =>
                {
                    option.AllowAnyOrigin()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }


        public static void UserCrosDomainsPolicy(this  WebApplication app)
        {

            app.UseCors("AllCrosDomainsPolicy");
        }

    }
}
