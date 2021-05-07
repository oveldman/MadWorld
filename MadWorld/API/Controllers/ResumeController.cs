using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(ILogger<ResumeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ResumeModel Get()
        {
            return new ResumeModel
            {
                FullName = "Oscar Veldman"
            };
        }
    }
}
