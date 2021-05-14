using System;
namespace Website.Shared.Models
{
    public class BaseModel
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
    }
}
