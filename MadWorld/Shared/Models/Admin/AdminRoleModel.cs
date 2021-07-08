using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Shared.Models.Admin
{
    /// <summary>
    /// Indentifier the role with the information
    /// </summary>
    public class AdminRoleModel
    {
        /// <summary>
        /// Indentifier of the role. The database saves a GUID. 
        /// </summary>
        /// <example>760a3510-562f-4feb-8450-ad8c7feac7a6</example>
        [Required]
        public string ID { get; set; }

        /// <summary>
        /// The name of the role.  
        /// </summary>
        /// <example>Admin</example>
        [Required]
        public string Name { get; set; }
    }
}
