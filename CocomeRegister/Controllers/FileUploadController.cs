using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// class <c>FileUploadController</c> provides a REST
    /// endpoint to upload files to server and therefor requires
    /// authorization
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        /// <summary>
        /// method <c>UploadAsync</c> is a http post
        /// endpoint to upload a form file to server
        /// </summary>
        /// <param name="file">
        /// file object to upload
        /// </param>
        /// <returns>response with path of uploaded file</returns>
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var folderName = Path.Combine("StaticFiles", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { path = Path.Combine(folderName, fileName) });
        }
    }
}
