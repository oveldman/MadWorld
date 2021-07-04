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

        [HttpPost]
        [Route("Create")]
        public BaseModel CreateFile(AddFileRequest request)
        {
            return _fileManager.CreateFile(request.ID, request.Name, request.Type, request.BodyBase64);
        }

        [HttpDelete]
        [Route("Delete")]
        public BaseModel DeleteFile(Guid? id)
        {
            if (id.HasValue) {
                return _fileManager.DeleteFile(id.Value);
            }

            return new BaseModel
            {
                ErrorMessage = "Id required"
            };
        }

        [HttpGet]
        [Route("GetAllFiles")]
        public FilesResponse GetAllFiles()
        {
            List<FileEditItem> files = _fileManager.GetFiles();

            return new FilesResponse
            {
                Succeed = true,
                Files = files
            };
        }
    }
}
