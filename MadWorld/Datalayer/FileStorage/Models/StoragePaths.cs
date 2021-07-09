using System;
namespace Datalayer.FileStorage.Models
{
    /// <summary>
    /// Hardcoded file paths to find the file locations
    /// </summary>
    public class StoragePaths
    {
        /// <summary>
        /// Path to save html blog posts
        /// </summary>
        public static string BlogFiles { get; private set; } = "Blogs";

        /// <summary>
        /// Path to save free download to anonymous users
        /// </summary>
        public static string FreeFiles { get; private set; } = "FreeDownload";
    }
}
