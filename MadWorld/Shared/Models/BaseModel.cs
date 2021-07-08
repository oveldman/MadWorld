using System;
using System.Collections.Generic;
using System.Linq;

namespace Website.Shared.Models
{
    /// <summary>
    /// This is the minimum response from the API 
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// The current process went well and don't have any errors. 
        /// </summary>
        /// <example>true</example>
        public bool Succeed { get; set; }

        /// <summary>
        /// This gives the client a message when the process succeed.
        /// </summary>
        /// <example>The operations succeed. </example>
        public string Message { get; set; }

        /// <summary>
        /// When the client only expect one error message at max. The backend will set the most importent message at the first of the list.
        /// </summary>
        /// <example>There are not results found. </example>
        public string ErrorMessage
        {
            get
            {
                return ErrorMessages?.Any() ?? false ? ErrorMessages.First() : string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value)) {
                    ErrorMessages = new List<string>()
                    {
                        value
                    };
                }
            }
        }

        /// <summary>
        /// When something went wrong and the process gives multiple error messages. 
        /// </summary>
        /// <example>There are not results found. </example>
        public List<string> ErrorMessages { get; set; }
    }
}
