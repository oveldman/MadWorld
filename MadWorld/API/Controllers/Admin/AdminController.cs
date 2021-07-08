using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Business.Interfaces;
using Datalayer.Database.Tables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace API.Controllers.Admin
{
    /// <summary>
    /// Admin module with account manager
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IAccountManager _accountManager;
        private readonly IUserExtremeManager _userExtremeManager;

        public AdminController(ILogger<AdminController> logger, IUserExtremeManager userExtremeManager, IAccountManager accountManager)
        {
            _logger = logger;
            _accountManager = accountManager;
            _userExtremeManager = userExtremeManager;
        }

        /// <summary>
        /// Retrieves an awesome welcome message for the admin page.
        /// </summary>
        /// <remarks>It is freaking awesome!</remarks>
        /// <response code="200">Return a fun message</response>
        /// <response code="400">You don't need to send data. Just don't do it then.</response>
        /// <response code="500">This is weird. There is no logic behind this endpoint.</response>
        [ProducesResponseType(typeof(AdminModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        public AdminModel Index()
        {
            return new AdminModel()
            {
                Succeed = true,
                WelcomeMessage = "Welcome dude!"
            };
        }

        /// <summary>
        /// Delete an user account from server
        /// </summary>
        /// <remarks>Delete an user account with userid</remarks>
        /// <response code="200">Return a message if everything went okay.</response>
        /// <response code="400">The id needs to be a string GUID</response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [ProducesResponseType(typeof(BaseModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<BaseModel> DeleteAccount(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return await _accountManager.DeleteAccount(id);
            }

            return new BaseModel
            {
                ErrorMessage = "ID is required"
            };
        }

        /// <summary>
        /// Gets every account with basic information
        /// </summary>
        /// <remarks>Returns accounts with the ID, Username and TwoFactorEnabled. The rest is empty. </remarks>
        /// <response code="200">Return a list with accounts</response>
        /// <response code="400">You don't need to send data. Just don't do it then.</response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [ProducesResponseType(typeof(IEnumerable<UserModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("GetAllAccounts")]
        public IEnumerable<UserModel> GetAllAccounts()
        {
            List<User> users = _userExtremeManager.GetAllUsers();

            return users.Select(u => new UserModel {
                Id = u.Id,
                Username = u.UserName,
                TwoFactorEnabled = u.TwoFactorOn
            });
        }

        /// <summary>
        /// Gets a specific account with all account information
        /// </summary>
        /// <remarks>Returns a account by Id with all information in the object </remarks>
        /// <response code="200">Returns one account</response>
        /// <response code="400">The id needs to be a string GUID</response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("GetAccount")]
        public async Task<UserModel> GetAccount(Guid? id)
        {
            if (id.HasValue)
            {
                UserModel user = _userExtremeManager.GetUser(id.Value);
                return await _accountManager.GetRoles(user);
            }

            return new UserModel();
        }

        /// <summary>
        /// Create or update account
        /// </summary>
        /// <remarks>Create or update all the properties of the account except for the ID. Set IsNew is true when you need to create an account. You cannot set an ID.
        /// This property will be ignored. </remarks>
        /// <response code="200">Return a message if everything went okay.</response>
        /// <response code="400">All the properties can be filled. </response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [HttpPost]
        [Route("SaveAccount")]
        public async Task<BaseModel> SaveAccount(UserModel userModel)
        {
            if (userModel is not null)
            {
                return await _accountManager.SaveAccount(userModel);
            }

            return new BaseModel
            {
                Succeed = false
            };
        }
    }
}
