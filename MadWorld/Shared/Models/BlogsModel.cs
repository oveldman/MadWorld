using System;
using System.Collections.Generic;
using Website.Shared.Models;

namespace Website.Shared.Models
{
    /// <summary>
    /// Gets all posts and basic blog information
    /// </summary>
    public class BlogsModel : BaseModel
    {
        /// <summary>
        /// Gets the current page which is requested. 
        /// </summary>
        /// <example>1</example>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Set the amount of posts that the backend gives back 
        /// </summary>
        /// <example>10</example>
        public int MaxRetrievedPosts { get; set; }
        /// <summary>
        /// Gets the total amount of pages.
        /// </summary>
        /// <example>10</example>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets the posts with HTML
        /// </summary>
        /// <example>new PostModel()</example>
        public List<PostModel> Posts { get; set; } = new();
    }
}
