using System;
namespace Website.Shared.Models.BackendInfo
{
    public class LogItem
    {
        public Guid ID { get; set; }
        public string Application { get; set; }
        public string Text { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string StackTrace { get; set; }
        public string Exception { get; set; }
        public DateTime Created { get; set; }
    }
}
