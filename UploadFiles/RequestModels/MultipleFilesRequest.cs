using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UploadFiles.RequestModels
{
    public class MultipleFilesRequest
    {
        public int Id { get; set; }
        [Required] public List<IFormFile> Files { get; set; }
    }
}