using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
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
        private IFileManager _fileManager;

        public AdminStorageController(ILogger<AdminStorageController> logger, IFileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
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
