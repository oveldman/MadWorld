﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.Admin
{
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
            }

            return new BaseModel
            {
                ErrorMessage = "Role already exists"
            };
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<BaseModel> DeleteRole(AdminRoleModel model)
        {
            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                IdentityRole role = await _roleManager.FindByIdAsync(model.ID);

                if (role is not null) {
                    await _roleManager.DeleteAsync(role);
                }
            }

            return new BaseModel
            {
                Succeed = true
            };
        }

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