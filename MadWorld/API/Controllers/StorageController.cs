using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Business.Interfaces;
using Datalayer.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        private IFileManager _fileManager;

        public StorageController(ILogger<StorageController> logger, IFileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        [HttpGet]
        [Route("DirectDownloadFile")]
        public IActionResult DirectDownloadFile(Guid? id)
        {
            if (id.HasValue)
            {
                FileItem item = _fileManager.GetFile(id.Value, FileType.Anonymous);

                if (item is not null) {
                    byte[] body = Convert.FromBase64String(item.BodyBase64);
                    return new FileStreamResult(new MemoryStream(body), item.Type)
                    {
                        FileDownloadName = item.Name
                    };
                }
            }

            return NotFound();
        }

        [HttpGet]
        [Route("DownloadFile")]
        public FileResponse DownloadFile(Guid? id)
        {
            if (id.HasValue) {
                FileItem item = _fileManager.GetFile(id.Value, FileType.Anonymous);

                return new FileResponse
                {
                    Succeed = item is not null,
                    File = item
                };
            }

            return new FileResponse
            {
                ErrorMessage = "ID is required"
            };
        }
    }
}
