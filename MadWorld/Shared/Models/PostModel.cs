using System;

namespace Website.Shared.Models
{
    /// <summary>
    /// Get a single post information with the html
    /// </summary>
    public class PostModel
    {
        /// <summary>
        /// Gets the summary of the post.
        /// </summary>
        /// <example>Welcome post!</example>
        public string Title { get; set; }

        /// <summary>
        /// The creation date of the post.
        /// </summary>
        /// <example>7/9/2021 4:28:05 PM</example>
        public DateTime Created { get; set; }

        /// <summary>
        /// The name of the writer of this post.
        /// </summary>
        /// <example>Oscar Veldman</example>
        public string WriterName { get; set; }

        /// <summary>
        /// Gets the base64 of the html of the post.
        /// </summary>
        /// <example>PGgxPldlbGNvbWU8L2gxPg==</example>
        public string Body { get; set; }
    }
}
