using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Shared.Models.Admin
{
    public class UserModel
    {
        /// <summary>
        /// Indentifier of the account. The database saves a GUID. 
        /// </summary>
        /// <example>760a3510-562f-4feb-8450-ad8c7feac7a6</example>
        public string Id { get; set; }

        /// <summary>
        /// Email adress
        /// </summary>
        /// <example>test@test.nl</example>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The login name of the account
        /// </summary>
        /// <example>test@test.nl</example>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// The password of the account. Use it only for the post request.
        /// </summary>
        /// <example>test1234!</example>
        public string Password { get; set; }

        /// <summary>
        /// Show if two factor is enabled. The admin can only turn two facor off.
        /// </summary>
        /// <example>true</example>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// When the account needs to be saved. This property shows if the account needs to be created or updated. 
        /// </summary>
        /// <example>true</example>
        public bool IsNew { get; set; }

        /// <summary>
        /// This list show all possible roles on the account. There is a property to turn the role on or off for this account.
        /// </summary>
        /// <example>new RoleModel()</example>
        public List<RoleModel> Roles { get; set; }
    }
}
