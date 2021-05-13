using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Nice!";
        }
    }
}
