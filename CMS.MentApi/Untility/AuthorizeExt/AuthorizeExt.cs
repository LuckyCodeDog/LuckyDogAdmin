using CMS.Common.JwtService;
using CMS.Common.Result;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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

                //config event 
                options.Events = new JwtBearerEvents()
                {
                    // activited when validate fails including  401 403  token is wrong or without token 
                    OnChallenge = context =>
                    {
                        // stop res
                        context.HandleResponse();
                        var payLoad = JsonConvert.SerializeObject(new ApiResult<string>()
                        {
                            Success = false,
                            Message = "Without Permission and token.",
                            Data = "401"

                        }, new JsonSerializerSettings
                        {
                            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                        });
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        //output json result 
                        context.Response.WriteAsync(payLoad);
                        return Task.FromResult(0);
                    },
                    OnForbidden = context =>
                    {
                        var payload = JsonConvert.SerializeObject(new ApiResult<string>()
                        {
                            Success = false,
                            Message = "Sorry, You Dont Have The Permission.",
                            Data = "403"
                        }, new JsonSerializerSettings
                        {
                            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                        });
                        //自定义返回的数据类型
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        //context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        //输出Json数据结果
                        context.Response.WriteAsync(payload);
                        return Task.FromResult(0);
                    }
                   

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
