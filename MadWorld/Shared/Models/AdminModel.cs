using System;
namespace Website.Shared.Models
{
    public class AdminModel : BaseModel
    {
        /// <summary>
        /// Greeting for the admin user
        /// </summary>
        /// <example>Welcome dude!</example>
        public string WelcomeMessage { get; set; }
    }
}
