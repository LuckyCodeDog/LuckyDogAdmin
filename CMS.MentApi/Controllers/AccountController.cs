
using CMS.MentApi.Untility.Filters;
using CMS.MentApi.Untility.SwaggerExt;
using Microsoft.AspNetCore.Mvc;

namespace CMS.MentApi.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = nameof(ApiVersions.v1), IgnoreApi = false)]
    [CustomExceptionFilter]
    public class AccountController: ControllerBase
    {

     /*   [HttpPost]
        public async Task<JsonResult> Login([]UserAccount userAccount);*/

    }
}
