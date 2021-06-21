using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        public StorageController(ILogger<StorageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("DirectDownloadFile")]
        public FileStreamResult DirectDownloadFile(Guid? id)
        {
            byte[] body = Convert.FromBase64String("RGl0IGlzIGVlbiB0ZXN0IQ==");
            return new FileStreamResult(new MemoryStream(body), "text/plain")
            {
                FileDownloadName = "Test.txt"
            };
        }

        [HttpGet]
        [Route("DownloadFile")]
        public FileResponse DownloadFile(Guid? id)
        {
            return new FileResponse
            {
                Succeed = true,
                Name = "Test.txt",
                Type = "text/plain",
                BodyBase64 = "RGl0IGlzIGVlbiB0ZXN0IQ=="
            };
        }
    }
}
