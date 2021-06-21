using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;

namespace API.Controllers.Admin
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminStorageController : ControllerBase
    {
        private readonly ILogger<AdminStorageController> _logger;

        public AdminStorageController(ILogger<AdminStorageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllFiles")]
        public FilesResponse GetAllFiles()
        {
            return new FilesResponse
            {
                Succeed = true,
                Files = new List<FileItem>()
            };
        }
    }
}
