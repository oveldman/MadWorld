using System;
namespace Website.Shared.Models.BackendInfo
{
    public class ApplicationStatusResponse
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime StartTime { get; set; }
        public string Host { get; set; }
    }
}
