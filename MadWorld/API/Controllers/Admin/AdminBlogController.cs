using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.Admin
{
    /// <summary>
    /// Admin module to manage all blogs post in the backend
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminBlogController : Controller
    {
        private readonly ILogger<AdminBlogController> _logger;

        private IBlogManager _blogManager;

        public AdminBlogController(ILogger<AdminBlogController> logger, IBlogManager blogManager)
        {
            _logger = logger;
            _blogManager = blogManager;
        }

        /// <summary>
        /// Retrieves all blog posts from the backend.
        /// </summary>
        /// <remarks>Retrieves all Blog posts from the backend. The purpose of the request is to select
        /// one of the posts to manage.</remarks>
        /// <response code="200">Return all the posts and basic information (Creation data, ID, Title) about the posts</response>
        /// <response code="400">You don't need to send data. Just don't do it then.</response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [ProducesResponseType(typeof(AdminBlogModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("GetAll")]
        public AdminBlogModel GetAll()
        {
            return _blogManager.GetBlog();
        }

        /// <summary>
        /// Retrieve one post from the database.
        /// </summary>
        /// <remarks>Gives all the infromation you need to edit the post.</remarks>
        /// <response code="200">Gives all the infromation you need to edit the post.</response>
        /// <response code="400">The ID of the post is required. </response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [HttpGet]
        [Route("Get")]
        public AdminPostModel Get(string id)
        {
            if (Guid.TryParse(id, out Guid guidID)) {
                return _blogManager.GetPost(guidID);
            }

            return new AdminPostModel
            {
                ErrorMessage = "ID needs to be a GUID and it required. "
            };
        }

        /// <summary>
        /// Delete an old post from the backend.
        /// </summary>
        /// <remarks>Remove a post from the database. If the post doesn't exist then gives still an okay back. </remarks>
        /// <response code="200">Return a OK message back.</response>
        /// <response code="400">The ID of the post is required. </response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [ProducesResponseType(typeof(AdminBlogModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpDelete]
        [Route("Delete")]
        public BaseModel Delete(string id)
        {
            if (Guid.TryParse(id, out Guid guidID))
            {
                return _blogManager.DeletePost(guidID);
            }

            return new BaseModel
            {
                ErrorMessage = "ID needs to be a GUID and it required. "
            };
        }

        /// <summary>
        /// Save a post
        /// </summary>
        /// <remarks>Create a new post or update an old post.</remarks>
        /// <response code="200">Return a OK message back.</response>
        /// <response code="400">Send all the information to the backend. Otherwise the data will be deleted. </response>
        /// <response code="500">There went something wrong on the backendside. </response>
        [ProducesResponseType(typeof(AdminBlogModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpPost]
        [Route("Save")]
        public BaseModel Save(AdminPostModel model)
        {
            return _blogManager.SavePost(model);
        }
    }
}
