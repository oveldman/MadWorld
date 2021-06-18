using System;
using System.Collections.Generic;
using System.Linq;

namespace Website.Shared.Models
{
    public class BaseModel
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
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
        public List<string> ErrorMessages { get; set; }
    }
}
