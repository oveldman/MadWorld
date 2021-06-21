using System;
using System.Net.Http;

namespace Website.Shared.Models
{
    public class AddFileRequest
    {
        public Guid? ID { get; set; }
        public StreamContent Body { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
