using CMS.Common.JwtService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CMS.MentApi.Untility.AuthorizeExt
{
    /// <summary>
    /// authorize 
    /// </summary>
    public  static class AuthorizeExt
    {
        /// <summary>
        /// jwt
        /// </summary>
        /// <param name="builder"></param>
        public static void AddJwtAuthorize(this WebApplicationBuilder builder)
        {
            JWTTokenOptions jWTTokenOptions = new JWTTokenOptions();
            builder.Configuration.Bind("JWTTokenOptions", jWTTokenOptions);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateSignatureLast = true,
                    ValidAudience = jWTTokenOptions.Audience,
                    ValidIssuer = jWTTokenOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTTokenOptions.SecurityKey)),
                    ClockSkew = TimeSpan.FromSeconds(0)
                };
            });
            builder.Services.AddTransient<IAuthorizationHandler, MenuAuthorizeHandler>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("btn", policyBuilder => policyBuilder
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .AddRequirements(new MenuAuthorizeRequirement()));
            });
        }
    }

}
