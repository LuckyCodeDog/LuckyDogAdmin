using CMS.Common.Result;
using CMS.MentApi.Untility.DatabaseExt;
using CMS.MentApi.Untility.Filters;
using CMS.MentApi.Untility.SwaggerExt;
using Microsoft.AspNetCore.Mvc;

namespace CMS.MentApi.Controllers
{
    /// <summary>
    /// to up load file
    /// </summary>
    [Route("/api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = nameof(ApiVersions.v1), IgnoreApi = false)]
    [CustomExceptionFilter]
    [MenuOrButton(MenuType.Menu, "Upload Files Api")]
    public class UpLoadFileController : ControllerBase
    {

        /// <summary>
        /// to upload avantar
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [MenuOrButton(MenuType.Button, "Upload User Avantar")]
        public async Task<JsonResult> UploadAvatar([FromForm] IFormFile file)
        {
            return await Task.FromResult(new JsonResult(new ApiResult()
            {
                Message = file.FileName,
                Success = true
            }));

        }
    }
}
