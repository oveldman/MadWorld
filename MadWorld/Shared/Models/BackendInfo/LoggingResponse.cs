using System;
using System.Collections.Generic;

namespace Website.Shared.Models.BackendInfo
{
    public class LoggingResponse : BaseModel
    {
        public List<LogItem> Logs { get; set; }
    }
}
