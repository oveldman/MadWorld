using System;
using System.Text.Json.Serialization;

namespace Website.Shared.Models
{
    public class ResumeModel
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
    }
}
