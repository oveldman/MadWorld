using System;
using System.Text.Json.Serialization;

namespace Website.Shared.Models
{
    public class ResumeModel : BaseModel
    {
        public string FullName { get; set; }
    }
}
