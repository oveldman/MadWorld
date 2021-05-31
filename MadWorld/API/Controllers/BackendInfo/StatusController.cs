using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models.BackendInfo;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.BackendInfo
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly IStatusManager _statusManager;


        public StatusController(ILogger<StatusController> logger, IStatusManager statusManager)
        {
            _logger = logger;
            _statusManager = statusManager;
        }

        [HttpGet]
        [Route("CheckConnectionAPI")]
        public IActionResult CheckConnectionAPI()
        {
            return Ok();
        }

        [HttpGet]
        [Route("CheckConnectionDatabaseAuthentication")]
        public DatabaseStatus CheckConnectionDatabaseAuthentication()
        {
            return new DatabaseStatus
            {
                IsOnline = _statusManager.IsDatabaseAuthenticationOnline()
            };
        }

        [HttpGet]
        [Route("CheckConnectionDatabaseMadWorld")]
        public DatabaseStatus CheckConnectionDatabaseMadWorld()
        {
            return new DatabaseStatus
            {
                IsOnline = _statusManager.IsDatabaseMadWorldOnline()
            };
        }
    }
}
