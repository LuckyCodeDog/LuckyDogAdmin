using CMS.Common.Enum;
using CMS.Common.Result;
using CMS.MentApi.Untility.DatabaseExt;
using CMS.MentApi.Untility.Filters;
using CMS.MentApi.Untility.SwaggerExt;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
namespace CMS.MentApi.Controllers
{
    /// <summary>
    /// to up load file
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = nameof(ApiVersions.v1), IgnoreApi = false)]
    [CustomExceptionFilter]
    [Function(MenuType.Menu, "File Management")]
    public class FileController : ControllerBase
    {

        /// <summary>
        /// to upload avantar
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Function(MenuType.Button, "Upload User Avantar")]
        public async Task<JsonResult> UploadAvantar([FromForm] IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            //if (!file.Name.EndsWith(".jpg")) throw new Exception("File format must be JPG.");


            string currentDirectoryPath = Directory.GetCurrentDirectory();
            string uploadDirectory = @"\Upload";
            string savePath = @$"{currentDirectoryPath}{uploadDirectory}\{DateTime.Now.ToString("yyyy-MM-dd")}";

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string saveFileName = $@"{Guid.NewGuid().ToString()}{DateTime.Now.ToString("yyyy-MM-dd")}.jpg";
            using (var stream = new FileStream($@"{savePath}\{saveFileName}", FileMode.Create))
            {
                file.CopyTo(stream);
            }

            //return  saved  file path
            return await Task.FromResult(new JsonResult(new ApiResult()
            {

                Message = $@"{DateTime.Now.ToString("yyyy-MM-dd")}%2F{saveFileName}",
                Success = true
            }));

        }

        /// <summary>
        /// download user avatar
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet("{filePath}")]
        [Function(MenuType.Button, "download user avantar")]
        public IActionResult Download(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new Exception("Invalid file path.");
            filePath = WebUtility.UrlDecode(filePath);
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
            string fullPath = Path.GetFullPath(Path.Combine(uploadsFolder, filePath)); 

   

            if (System.IO.File.Exists(fullPath))
            {
                return PhysicalFile(fullPath, "application/octet-stream");
            }

            return new JsonResult(
                    new ApiResult()
                    {
                        Message = "file not found",
                        Success = false

                    }
                );
        }
    }
}
