using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UploadFiles.RequestModels;

namespace UploadFiles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadFilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public UploadFilesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> PostFiles([FromForm] MultipleFilesRequest request)
        {
            if (request.Files == null || request.Files.Count == 0) return Content("files not selected");
            long size = request.Files.Sum(f => f.Length);
            var filePaths = new List<string>();
            foreach (var formFile in request.Files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.Combine(_environment.WebRootPath, "files");

                    var fileNameWithPath = string.Concat(filePath,"\\",formFile.FileName);
                    filePaths.Add(fileNameWithPath); 

                    await using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                    await formFile.CopyToAsync(stream);
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = request.Files.Count, size, filePaths });
        }
    }
}