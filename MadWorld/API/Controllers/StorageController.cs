using System;
using System.Collections.Generic;
using System.Linq;
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
