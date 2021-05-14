using System;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly ILogger<ResumeController> _logger;

        private readonly IResumeManager _resumeManager;

        public ResumeController(ILogger<ResumeController> logger, IResumeManager resumeManager)
        {
            _logger = logger;

            _resumeManager = resumeManager;
        }

        [HttpGet]
        public ResumeModel Get()
        {
            var resume = _resumeManager.GetLastResume();

            return new ResumeModel
            {
                Succeed = true,
                FullName = resume?.FullName
            };
        }
    }
}
