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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;
        private readonly ILoggingManager _loggingManager;

        public LoggingController(ILogger<LoggingController> logger, ILoggingManager loggingManager)
        {
            _logger = logger;
            _loggingManager = loggingManager;
        }

        [HttpPost]
        [Route("GetAll")]
        public LoggingResponse GetAll(LoggingRequest request)
        {
            return new LoggingResponse
            {
                Succeed = true,
                Logs = _loggingManager.GetLogging(request.StartTime, request.EndTime)
            };
        }
    }
}
