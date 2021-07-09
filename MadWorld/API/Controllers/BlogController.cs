using System.Collections.Generic;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                }
            };
        }
    }
}
