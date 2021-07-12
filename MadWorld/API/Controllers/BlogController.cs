using System.Collections.Generic;
using System.Linq;
using Business.Interfaces;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    /// <summary>
    /// An anonymous user can gets here all the blog information
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;

        private readonly IBlogManager _blogManager;

        public BlogController(ILogger<BlogController> logger, IBlogManager blogManager)
        {
            _logger = logger;

            _blogManager = blogManager;
        }

        /// <summary>
        /// Retrieves Blog posts from the backend.
        /// </summary>
        /// <remarks>Retrieves Blog posts from the backend. You can query with amount of posts and which page.
        /// The maximum of posts at the same time is 50. </remarks>
        /// <response code="200">Return all the posts and basic information about the posts</response>
        /// <response code="400">Page and totalPosts are numbers. </response>
        /// <response code="500">This is weird. There is no logic behind this endpoint.</response>
        [ProducesResponseType(typeof(BlogsModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("GetAll")]
        public BlogsModel GetAll(int page, int totalPosts)
        {
            int maxTotalPosts = 50;
            totalPosts = totalPosts > maxTotalPosts ? maxTotalPosts : totalPosts;

            BlogsModel model = _blogManager.GetBlog(page, totalPosts);

            if (model?.Posts?.Any() ?? false)
            {
                return model;
            }

            return new BlogsModel
            {
                Succeed = true,
                MaxRetrievedPosts = totalPosts,
                CurrentPage = page,
                TotalPages = 10,
                Posts = new()
                {
                    new PostModel
                    {
                        Title = "Welcome everyone!",
                        Created = SystemTime.Now(),
                        WriterName = "Oscar Veldman",
                        Body = "PHA+VGhpcyBpcyBhIHRlc3QgbWVzc2FnZTwvcD4="
                    },
                    new PostModel
                    {
                        Title = "Today is a warm day!",
                        Created = SystemTime.Now(),
                        WriterName = "Oscar Veldman",
                        Body = "PHA+VGhpcyBpcyBzbyB3YXJtLi4uLi48L3A+"
                    },
                    new PostModel
                    {
                        Title = "Script test",
                        Created = SystemTime.Now(),
                        WriterName = "Oscar Veldman",
                        Body = "PHA+SGFsbG8gQWxsZW1hYWwuIERlIGlzIGVlbiA8Yj5zY3JpcHQ8L2I+IHRlc3QuIDwvcD4KCjxzY3JpcHQ+ZnVuY3Rpb24gdGVzdCgpIHsgQ29uc29sZS5Xcml0ZSgnMScpIH08L3NjcmlwdD4="
                    },
                }
            };
        }
    }
}
