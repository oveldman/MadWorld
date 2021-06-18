using System;
using System.Threading.Tasks;
using Website.Shared.Models.BackendInfo;

namespace Website.Services.Interfaces
{
    public interface ILoggingService
    {
        Task<LoggingResponse> GetLogging(DateTime? startDate, DateTime? endDate);
        Task<LogResponse> GetLog(Guid? logID);
    }
}
