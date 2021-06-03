using System;
using System.Collections.Generic;
using Website.Shared.Models.BackendInfo;

namespace Business.Interfaces
{
    public interface ILoggingManager
    {
        List<LogItem> GetLogging(DateTime? startDate, DateTime? endDate);
    }
}
