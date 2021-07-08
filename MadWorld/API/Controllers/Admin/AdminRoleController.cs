using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Admin;
using Website.Shared.Opions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.Admin
{
    /// <summary>
    /// Admin module to manage all roles in the backend
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminRoleController : ControllerBase
    {
        private readonly ILogger<AdminRoleController> _logger;

        private RoleManager<IdentityRole> _roleManager;

        public AdminRoleController(ILogger<AdminRoleController> logger, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Add the standard roles from the code.
        /// </summary>
        /// <remarks>This adds the minimum roles to the backend that needs to function normal.
        /// At the moment the roles are: "Admin"</remarks>
        /// <response code="200">Return a message if everything went okay.</response>
        /// <response code="400">You don't need to send data. Just don't do it then.</response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [HttpPost]
        [Route("AddStandard")]
        public async Task<BaseModel> AddStandardRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                foreach (string roleName in UserRoles.GetAllPropertyNames())
                {
                    IdentityRole identityRole = new() {
                        Name = roleName
                    };

                    await _roleManager.CreateAsync(identityRole);
                }
            };

            return new BaseModel
            {
                Succeed = true
            };
        }

        /// <summary>
        /// Add a new role to the system. 
        /// </summary>
        /// <remarks>Add a new role. A role grants access of a part of the system. </remarks>
        /// <response code="200">Return a message if everything went okay.</response>
        /// <response code="400">The name is required with a minimum of one character. Send a random ID.</response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [HttpPost]
        [Route("Add")]
        public async Task<BaseModel> AddRole(AdminRoleModel model)
        {
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.Name,
                };

                await _roleManager.CreateAsync(role);

                return new BaseModel
                {
                    Succeed = true
                };
            }

            return new BaseModel
            {
                ErrorMessage = "Role already exists"
            };
        }

        /// <summary>
        /// Remove a role from the system
        /// </summary>
        /// <remarks>Remove the role from the system. If the role doesn't exist, then you get still a succeed. </remarks>
        /// <response code="200">Return a message if everything went okay.</response>
        /// <response code="400">The id is required. </response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [HttpDelete]
        [Route("Delete")]
        public async Task<BaseModel> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role is not null)
            {
                await _roleManager.DeleteAsync(role);
            }

            return new BaseModel
            {
                Succeed = true
            };
        }


        /// <summary>
        /// Get a list of all a available roles 
        /// </summary>
        /// <remarks>Get a list of all a available roles from the backend</remarks>
        /// <response code="200">List of roles with names and ID's</response>
        /// <response code="400">The id is required. </response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [HttpGet]
        [Route("GetAll")]
        public List<AdminRoleModel> GetAllRoles()
        {
            return _roleManager.Roles.Select(r => new AdminRoleModel {
                ID = r.Id,
                Name = r.Name
            }).ToList();
        }
    }
}
